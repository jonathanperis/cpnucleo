import { createHash } from 'node:crypto';
import { mkdtempSync, readFileSync, rmSync, mkdirSync, writeFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join, resolve } from 'node:path';
import { afterEach, describe, expect, it, vi } from 'vitest';
import {
  CSP_MANIFEST_FILE,
  buildContentSecurityPolicy,
  buildCspManifest,
  buildSecurityHeaders,
  connectSourcesFromEnv,
  extractInlineScripts,
  hashInlineScript,
  listHtmlFiles,
  readCspManifest,
  validateCspManifest,
  writeCspManifest,
} from './csp.mjs';

const temporaryDirectories: string[] = [];
const temporaryDirectory = () => {
  const directory = mkdtempSync(join(tmpdir(), 'cpnucleo-csp-'));
  temporaryDirectories.push(directory);
  return directory;
};
afterEach(() => { for (const directory of temporaryDirectories.splice(0)) rmSync(directory, { recursive: true, force: true }); });

const directive = (policy: string, name: string) => policy.split('; ').find(part => part.startsWith(`${name} `)) ?? '';

describe('CSP manifest generation', () => {
  it('derives connect-src from the build-time service URLs for the lab and production', () => {
    expect(connectSourcesFromEnv({})).toEqual(['http://localhost:5100', 'http://localhost:5200']);
    expect(connectSourcesFromEnv({
      PUBLIC_WEBAPI_BASE_URL: 'https://api-cpnucleo.jonathanperis.tech/api',
      PUBLIC_IDENTITY_API_BASE_URL: 'https://identity-cpnucleo.jonathanperis.tech/api',
    })).toEqual(['https://api-cpnucleo.jonathanperis.tech', 'https://identity-cpnucleo.jonathanperis.tech']);
    expect(connectSourcesFromEnv({ PUBLIC_WEBAPI_BASE_URL: 'https://same.test/api', PUBLIC_IDENTITY_API_BASE_URL: 'https://same.test/identity' }))
      .toEqual(['https://same.test']);
    expect(() => connectSourcesFromEnv({ PUBLIC_WEBAPI_BASE_URL: 'javascript:alert(1)' })).toThrow(/http or https/);
  });

  it('uses the same default service URLs as the browser configuration', async () => {
    const publicEnv = import.meta.env as Record<string, string | undefined>;
    delete publicEnv.PUBLIC_WEBAPI_BASE_URL;
    delete publicEnv.PUBLIC_IDENTITY_API_BASE_URL;
    vi.resetModules();
    const { WEBAPI_BASE_URL, IDENTITY_API_BASE_URL } = await import('~/lib/config');
    expect(connectSourcesFromEnv({})).toEqual([new URL(WEBAPI_BASE_URL).origin, new URL(IDENTITY_API_BASE_URL).origin]);
  });

  it('hashes only inline scripts, byte-for-byte as the browser does', () => {
    const inline = "\n  console.log('theme');\n";
    const html = `<head><script>${inline}</script><script type="module" src="/_astro/app.js"></script><script type="module">run()</script></head>`;
    expect(extractInlineScripts(html)).toEqual([inline, 'run()']);
    expect(hashInlineScript(inline)).toBe(`'sha256-${createHash('sha256').update(inline).digest('base64')}'`);
    const manifest = buildCspManifest({ htmlDocuments: [html, html], env: {} });
    expect(manifest.scriptHashes).toHaveLength(2);
    expect(manifest.scriptHashes).toContain(hashInlineScript('run()'));
  });

  it('writes and reads a manifest for every HTML file in the build output', () => {
    const outDir = temporaryDirectory();
    mkdirSync(join(outDir, 'nested'));
    writeFileSync(join(outDir, 'index.html'), '<script>one()</script>');
    writeFileSync(join(outDir, 'nested', 'index.html'), '<script>two()</script>');
    expect(listHtmlFiles(outDir)).toHaveLength(2);
    const written = writeCspManifest(outDir, { PUBLIC_WEBAPI_BASE_URL: 'http://localhost:5100/api' });
    expect(JSON.parse(readFileSync(join(outDir, CSP_MANIFEST_FILE), 'utf8'))).toEqual(written);
    expect(readCspManifest(outDir)).toEqual(written);
    expect(written.scriptHashes).toEqual([hashInlineScript('one()'), hashInlineScript('two()')].sort());
  });

  it('fails clearly when the build has not produced a manifest', () => {
    expect(() => readCspManifest(temporaryDirectory())).toThrow(/bun run build/);
  });

  it('rejects manifests that would inject extra policy directives', () => {
    expect(() => validateCspManifest({ connectSrc: ["https://ok.test; script-src 'unsafe-inline'"], scriptHashes: [] })).toThrow();
    expect(() => validateCspManifest({ connectSrc: [], scriptHashes: ["'unsafe-inline'"] })).toThrow(/script hash/);
    expect(() => validateCspManifest({ connectSrc: 'https://ok.test', scriptHashes: [] })).toThrow();
  });
});

describe('preview security headers', () => {
  const manifest = { connectSrc: ['http://localhost:5100', 'http://localhost:5200'], scriptHashes: [hashInlineScript('theme()')] };

  it('replaces unsafe-inline scripts with hashes and keeps the other protections', () => {
    const headers = buildSecurityHeaders(manifest);
    const policy = headers['Content-Security-Policy'];
    expect(directive(policy, 'script-src')).toBe(`script-src 'self' ${hashInlineScript('theme()')}`);
    expect(directive(policy, 'script-src')).not.toContain('unsafe-inline');
    expect(directive(policy, 'connect-src')).toBe("connect-src 'self' http://localhost:5100 http://localhost:5200");
    expect(policy).toContain("frame-ancestors 'none'");
    expect(policy).toContain("default-src 'self'");
    expect(headers).toMatchObject({
      'Strict-Transport-Security': 'max-age=31536000; includeSubDomains',
      'X-Content-Type-Options': 'nosniff',
      'X-Frame-Options': 'DENY',
      'Referrer-Policy': 'strict-origin-when-cross-origin',
    });
  });

  it('keeps production hosts when the build targets production', () => {
    const policy = buildContentSecurityPolicy({ connectSrc: connectSourcesFromEnv({
      PUBLIC_WEBAPI_BASE_URL: 'https://api-cpnucleo.jonathanperis.tech/api',
      PUBLIC_IDENTITY_API_BASE_URL: 'https://identity-cpnucleo.jonathanperis.tech/api',
    }), scriptHashes: [] });
    expect(directive(policy, 'connect-src')).toBe("connect-src 'self' https://api-cpnucleo.jonathanperis.tech https://identity-cpnucleo.jonathanperis.tech");
  });

  it('covers every inline script in the generated Astro pages', () => {
    const outDir = resolve('dist');
    const built = readCspManifest(outDir);
    const inlineHashes = new Set(listHtmlFiles(outDir).flatMap(path => extractInlineScripts(readFileSync(path, 'utf8'))).map(hashInlineScript));
    expect(inlineHashes.size).toBeGreaterThan(0);
    expect(new Set(built.scriptHashes)).toEqual(inlineHashes);
    expect(built.connectSrc.length).toBeGreaterThan(0);
  });
});
