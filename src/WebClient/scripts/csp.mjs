import { createHash } from 'node:crypto';
import { readdirSync, readFileSync, writeFileSync } from 'node:fs';
import { join } from 'node:path';
import { fileURLToPath } from 'node:url';

// Build-time Content-Security-Policy inputs.
//
// The WebClient is a static Astro build: service URLs are baked into the JavaScript from
// PUBLIC_* values at build time, and Astro may inline small scripts into the HTML. The build
// therefore emits dist/csp-manifest.json with the service origins the bundle will call and the
// SHA-256 hashes of every inline script, and the preview server derives its policy from it.

export const CSP_MANIFEST_FILE = 'csp-manifest.json';

// Keep these defaults identical to src/lib/config.ts (asserted by scripts/csp.test.ts).
export const DEFAULT_WEBAPI_BASE_URL = 'http://localhost:5100/api';
export const DEFAULT_IDENTITY_API_BASE_URL = 'http://localhost:5200/api';

const scriptHashPattern = /^'sha256-[A-Za-z0-9+/]+={0,2}'$/;

export const serviceOrigin = (value) => {
  const url = new URL(value);
  if (url.protocol !== 'http:' && url.protocol !== 'https:') {
    throw new Error(`Service URL must use http or https: ${value}`);
  }
  return url.origin;
};

export const connectSourcesFromEnv = (env = {}) => [...new Set([
  env.PUBLIC_WEBAPI_BASE_URL || DEFAULT_WEBAPI_BASE_URL,
  env.PUBLIC_IDENTITY_API_BASE_URL || DEFAULT_IDENTITY_API_BASE_URL,
].map(serviceOrigin))];

// The sign-in form posts to the identity server (a browser form post, so its cookie is
// first-party there); no other cross-origin form target is allowed.
export const formActionsFromEnv = (env = {}) => [serviceOrigin(env.PUBLIC_IDENTITY_API_BASE_URL || DEFAULT_IDENTITY_API_BASE_URL)];

// Browsers end a script at "</script" followed by whitespace, "/" or ">", even with junk before
// the ">" (e.g. "</script \t\n bar>"), so the end-tag pattern accepts any attributes.
const scriptElementPattern = /<script\b([^>]*)>([\s\S]*?)<\/script\b[^>]*>/gi;

/** Returns the text of every inline (non-src) script element, exactly as the browser hashes it. */
export const extractInlineScripts = (html) => {
  const scripts = [];
  for (const [, attributes, content] of html.matchAll(scriptElementPattern)) {
    if (/\bsrc\s*=/i.test(attributes)) continue;
    scripts.push(content);
  }
  return scripts;
};

export const hashInlineScript = (content) =>
  `'sha256-${createHash('sha256').update(content, 'utf8').digest('base64')}'`;

export const listHtmlFiles = (directory) => readdirSync(directory, { withFileTypes: true, recursive: true })
  .filter((entry) => entry.isFile() && entry.name.endsWith('.html'))
  .map((entry) => join(entry.parentPath, entry.name))
  .sort();

export const buildCspManifest = ({ htmlDocuments, env }) => ({
  connectSrc: connectSourcesFromEnv(env),
  formAction: formActionsFromEnv(env),
  scriptHashes: [...new Set(htmlDocuments.flatMap(extractInlineScripts).map(hashInlineScript))].sort(),
});

export const writeCspManifest = (outDir, env) => {
  const htmlDocuments = listHtmlFiles(outDir).map((path) => readFileSync(path, 'utf8'));
  const manifest = buildCspManifest({ htmlDocuments, env });
  writeFileSync(join(outDir, CSP_MANIFEST_FILE), `${JSON.stringify(manifest, null, 2)}\n`);
  return manifest;
};

/** Validates a manifest so a tampered file cannot inject extra directives into the header. */
export const validateCspManifest = (manifest) => {
  if (!manifest || !Array.isArray(manifest.connectSrc) || !Array.isArray(manifest.scriptHashes)) {
    throw new Error('CSP manifest must contain connectSrc and scriptHashes arrays.');
  }
  if (manifest.formAction !== undefined && !Array.isArray(manifest.formAction)) {
    throw new Error('CSP manifest formAction must be an array.');
  }
  for (const [directive, origins] of [['connect-src', manifest.connectSrc], ['form-action', manifest.formAction ?? []]]) {
    for (const origin of origins) {
      let valid = false;
      try { valid = typeof origin === 'string' && serviceOrigin(origin) === origin; } catch { valid = false; }
      if (!valid) throw new Error(`Invalid ${directive} origin: ${origin}`);
    }
  }
  for (const hash of manifest.scriptHashes) {
    if (typeof hash !== 'string' || !scriptHashPattern.test(hash)) throw new Error(`Invalid script hash: ${hash}`);
  }
  return manifest;
};

export const readCspManifest = (root) => {
  const path = join(root, CSP_MANIFEST_FILE);
  let content;
  try {
    content = readFileSync(path, 'utf8');
  } catch (error) {
    throw new Error(`Missing ${path}. Run "bun run build" before starting the preview server.`, { cause: error });
  }
  return validateCspManifest(JSON.parse(content));
};

/** @param {{ connectSrc: string[], formAction?: string[], scriptHashes: string[] }} sources */
export const buildContentSecurityPolicy = ({ connectSrc, formAction = [], scriptHashes }) => [
  "default-src 'self'",
  ['script-src', "'self'", ...scriptHashes].join(' '),
  "style-src 'self' 'unsafe-inline'",
  "img-src 'self' data:",
  ['connect-src', "'self'", ...connectSrc].join(' '),
  "object-src 'none'",
  "frame-ancestors 'none'",
  "base-uri 'self'",
  ['form-action', "'self'", ...formAction].join(' '),
].join('; ');

export const buildSecurityHeaders = (manifest) => ({
  'Strict-Transport-Security': 'max-age=31536000; includeSubDomains',
  'X-Content-Type-Options': 'nosniff',
  'X-Frame-Options': 'DENY',
  'Referrer-Policy': 'strict-origin-when-cross-origin',
  'Content-Security-Policy': buildContentSecurityPolicy(validateCspManifest(manifest)),
});

/** Astro integration: emits dist/csp-manifest.json after every static build. */
export const cspManifestIntegration = ({ env }) => ({
  name: 'cpnucleo-csp-manifest',
  hooks: {
    'astro:build:done': ({ dir, logger }) => {
      const manifest = writeCspManifest(fileURLToPath(dir), env);
      logger.info(`CSP manifest: connect-src ${manifest.connectSrc.join(' ')}; form-action ${manifest.formAction.join(' ')}; ${manifest.scriptHashes.length} inline script hash(es).`);
    },
  },
});
