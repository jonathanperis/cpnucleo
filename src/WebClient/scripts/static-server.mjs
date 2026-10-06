import { createReadStream, existsSync, statSync } from 'node:fs';
import { extname, join, normalize, sep } from 'node:path';

export const contentTypes = {
  '.css': 'text/css; charset=utf-8',
  '.html': 'text/html; charset=utf-8',
  '.js': 'text/javascript; charset=utf-8',
  '.json': 'application/json; charset=utf-8',
  '.svg': 'image/svg+xml',
  '.webp': 'image/webp',
  '.png': 'image/png',
  '.jpg': 'image/jpeg',
  '.jpeg': 'image/jpeg',
  '.woff': 'font/woff',
  '.woff2': 'font/woff2',
};

const noopTelemetry = {
  startHttpRequestSpan: () => undefined,
  recordHttpRequest: () => undefined,
  recordHttpError: () => undefined,
};

export const resolveStaticPath = (root, urlPath) => {
  const decoded = decodeURIComponent(urlPath.split('?')[0] ?? '/');
  const safe = normalize(decoded).replace(/^(\.\.[/\\])+/, '');
  const requested = join(root, safe);
  if (requested !== root && !requested.startsWith(`${root}${sep}`)) return join(root, 'index.html');
  if (existsSync(requested) && statSync(requested).isDirectory()) return join(requested, 'index.html');
  if (existsSync(requested)) return requested;
  return join(root, 'index.html');
};

/**
 * @typedef {object} StaticHandlerOptions
 * @property {string} root Absolute path of the Astro build output.
 * @property {Record<string, string>} securityHeaders Headers added to every response.
 * @property {{ startHttpRequestSpan: Function, recordHttpRequest: Function, recordHttpError: Function }} [telemetry]
 * @property {(path: string) => import('node:stream').Readable} [openFile]
 */

/**
 * Static file handler used by the preview server. `openFile` is injectable so tests can prove
 * that a read failure after the status line was sent destroys the response instead of trying to
 * write a second set of headers.
 * @param {StaticHandlerOptions} options
 * @returns {(request: import('node:http').IncomingMessage, response: import('node:http').ServerResponse) => void}
 */
export const createStaticHandler = ({ root, securityHeaders, telemetry = noopTelemetry, openFile = createReadStream }) => (request, response) => {
  const startTime = process.hrtime.bigint();
  const span = telemetry.startHttpRequestSpan(request);
  let finalized = false;
  const finalize = (record) => {
    if (finalized) return;
    finalized = true;
    record();
  };
  const fail = (error, status = 500, body = 'internal server error') => {
    finalize(() => telemetry.recordHttpError(request, error, span));
    if (response.headersSent) {
      response.destroy(error);
      return;
    }
    response.writeHead(status, { ...securityHeaders, 'Content-Type': 'text/plain; charset=utf-8' });
    response.end(body);
  };

  response.on('finish', () => finalize(() => telemetry.recordHttpRequest(request, response, startTime, span)));
  response.on('error', (error) => finalize(() => telemetry.recordHttpError(request, error, span)));
  response.on('close', () => finalize(() => {
    if (response.writableFinished) {
      telemetry.recordHttpRequest(request, response, startTime, span);
      return;
    }

    telemetry.recordHttpError(request, new Error('response closed before finish'), span);
  }));

  try {
    if (request.url === '/healthz') {
      response.writeHead(200, { ...securityHeaders, 'Content-Type': 'text/plain; charset=utf-8' });
      response.end('ok');
      return;
    }

    let filePath;
    try {
      filePath = resolveStaticPath(root, request.url ?? '/');
    } catch (error) {
      if (error instanceof URIError) {
        fail(error, 400, 'bad request');
        return;
      }
      throw error;
    }
    if (!existsSync(filePath)) {
      response.writeHead(404, { ...securityHeaders, 'Content-Type': 'text/plain; charset=utf-8' });
      response.end('not found');
      return;
    }

    const extension = extname(filePath);
    response.writeHead(200, {
      ...securityHeaders,
      'Content-Type': contentTypes[extension] ?? 'application/octet-stream',
      ...(extension && extension !== '.html' ? { 'Cache-Control': 'public, max-age=31536000, immutable' } : {}),
    });
    openFile(filePath)
      .on('error', (error) => fail(error))
      .pipe(response);
  } catch (error) {
    fail(error);
  }
};
