import { mkdtempSync, rmSync, writeFileSync } from 'node:fs';
import { createServer, get, type Server } from 'node:http';
import type { AddressInfo } from 'node:net';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { Readable } from 'node:stream';
import { afterEach, describe, expect, it, vi } from 'vitest';
import { createStaticHandler, resolveStaticPath } from './static-server.mjs';

type StaticHandlerOptions = Parameters<typeof createStaticHandler>[0];

const securityHeaders = { 'Content-Security-Policy': "default-src 'self'", 'X-Frame-Options': 'DENY' };
let server: Server | undefined;
let root = '';

const start = async (options: Omit<Partial<StaticHandlerOptions>, 'root' | 'securityHeaders'> = {}) => {
  root = mkdtempSync(join(tmpdir(), 'cpnucleo-static-'));
  writeFileSync(join(root, 'index.html'), '<!doctype html><title>home</title>');
  writeFileSync(join(root, 'app.js'), 'console.log(1)');
  server = createServer(createStaticHandler({ root, securityHeaders, ...options }));
  await new Promise<void>(resolve => server!.listen(0, '127.0.0.1', resolve));
  return `http://127.0.0.1:${(server.address() as AddressInfo).port}`;
};

const request = (url: string) => new Promise<{ status?: number; headers: Record<string, unknown>; body: string }>((resolve, reject) => {
  get(url, response => {
    let body = '';
    response.setEncoding('utf8');
    response.on('data', chunk => { body += chunk; });
    response.on('end', () => resolve({ status: response.statusCode, headers: response.headers, body }));
    response.on('error', reject);
    response.on('aborted', () => reject(new Error('aborted')));
  }).on('error', reject);
});

afterEach(async () => {
  await new Promise<void>(resolve => (server ? server.close(() => resolve()) : resolve()));
  server = undefined;
  if (root) rmSync(root, { recursive: true, force: true });
});

describe('preview static handler', () => {
  it('serves files and the health endpoint with the security headers', async () => {
    const base = await start();
    const health = await request(`${base}/healthz`);
    expect(health).toMatchObject({ status: 200, body: 'ok' });
    expect(health.headers['content-security-policy']).toBe("default-src 'self'");
    const asset = await request(`${base}/app.js`);
    expect(asset.status).toBe(200);
    expect(asset.headers['cache-control']).toContain('immutable');
    expect((await request(`${base}/projects/`)).body).toContain('home');
  });

  it('destroys the response instead of rewriting headers when the file stream fails mid-response', async () => {
    const recordHttpError = vi.fn();
    const failingStream = () => new Readable({ read() { this.destroy(new Error('disk read failed')); } });
    const base = await start({
      openFile: failingStream,
      telemetry: { startHttpRequestSpan: () => undefined, recordHttpRequest: () => undefined, recordHttpError },
    });
    const uncaught = vi.fn();
    process.on('uncaughtException', uncaught);
    try {
      await expect(request(`${base}/app.js`)).rejects.toThrow();
    } finally {
      process.off('uncaughtException', uncaught);
    }
    expect(uncaught).not.toHaveBeenCalled();
    expect(recordHttpError).toHaveBeenCalledWith(expect.anything(), expect.objectContaining({ message: 'disk read failed' }), undefined);
    // The server keeps serving after the failure.
    expect((await request(`${base}/healthz`)).status).toBe(200);
  });

  it('rejects malformed escapes and keeps paths inside the build output', async () => {
    const base = await start();
    expect((await request(`${base}/%E0%A4%A`)).status).toBe(400);
    expect(resolveStaticPath(root, '/../../etc/passwd')).toBe(join(root, 'index.html'));
  });
});
