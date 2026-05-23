import { createReadStream, existsSync, statSync } from 'node:fs';
import { createServer } from 'node:http';
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
  createReadStream(filePath).pipe(response);
}).listen(port, host, () => {
  console.log(`Preview server listening on http://${host}:${port}`);
});
