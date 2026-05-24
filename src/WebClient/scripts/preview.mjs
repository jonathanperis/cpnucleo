import { createReadStream, existsSync, statSync } from 'node:fs';
import { createServer } from 'node:http';
import { recordHttpError, recordHttpRequest, startHttpRequestSpan } from './otel.mjs';
import { extname, join, normalize } from 'node:path';

const root = join(process.cwd(), 'dist');
const port = Number(process.env.PORT ?? '5030');
const host = process.env.HOST ?? '0.0.0.0';

const contentTypes = {
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

const resolvePath = (urlPath) => {
  const decoded = decodeURIComponent(urlPath.split('?')[0] ?? '/');
  const safe = normalize(decoded).replace(/^(\.\.[/\\])+/, '');
  const requested = join(root, safe);
  if (existsSync(requested) && statSync(requested).isDirectory()) return join(requested, 'index.html');
  if (existsSync(requested)) return requested;
  return join(root, 'index.html');
};

createServer((request, response) => {
  const startTime = process.hrtime.bigint();
  const span = startHttpRequestSpan(request);
  let finalized = false;
  const finalize = (record) => {
    if (finalized) return;
    finalized = true;
    record();
  };

  response.on('finish', () => finalize(() => recordHttpRequest(request, response, startTime, span)));
  response.on('error', (error) => finalize(() => recordHttpError(request, error, span)));
  response.on('close', () => finalize(() => {
    if (response.writableFinished) {
      recordHttpRequest(request, response, startTime, span);
      return;
    }

    recordHttpError(request, new Error('response closed before finish'), span);
  }));

  try {
    if (request.url === '/healthz') {
      response.writeHead(200, { 'Content-Type': 'text/plain; charset=utf-8' });
      response.end('ok');
      return;
    }

    const filePath = resolvePath(request.url ?? '/');
    if (!existsSync(filePath)) {
      response.writeHead(404, { 'Content-Type': 'text/plain; charset=utf-8' });
      response.end('not found');
      return;
    }

    const extension = extname(filePath);
    response.writeHead(200, {
      'Content-Type': contentTypes[extension] ?? 'application/octet-stream',
      ...(extension && extension !== '.html' ? { 'Cache-Control': 'public, max-age=31536000, immutable' } : {}),
    });
    createReadStream(filePath)
      .on('error', (error) => {
        finalize(() => recordHttpError(request, error, span));
        response.writeHead(500, { 'Content-Type': 'text/plain; charset=utf-8' });
        response.end('internal server error');
      })
      .pipe(response);
  } catch (error) {
    finalize(() => recordHttpError(request, error, span));
    response.writeHead(500, { 'Content-Type': 'text/plain; charset=utf-8' });
    response.end('internal server error');
  }
}).listen(port, host, () => {
  console.log(`Preview server listening on http://${host}:${port}`);
});
