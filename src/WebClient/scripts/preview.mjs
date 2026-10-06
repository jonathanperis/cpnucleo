import { createServer } from 'node:http';
import { join } from 'node:path';
import { recordHttpError, recordHttpRequest, startHttpRequestSpan } from './otel.mjs';
import { buildSecurityHeaders, readCspManifest } from './csp.mjs';
import { createStaticHandler } from './static-server.mjs';

const root = join(process.cwd(), 'dist');
const port = Number(process.env.PORT ?? '5030');
const host = process.env.HOST ?? '0.0.0.0';

// Content-Security-Policy connect-src and inline script hashes come from dist/csp-manifest.json,
// which `astro build` derives from PUBLIC_WEBAPI_BASE_URL / PUBLIC_IDENTITY_API_BASE_URL and the
// generated HTML. Changing runtime environment variables does not change a static build.
const securityHeaders = buildSecurityHeaders(readCspManifest(root));

createServer(createStaticHandler({
  root,
  securityHeaders,
  telemetry: { startHttpRequestSpan, recordHttpRequest, recordHttpError },
})).listen(port, host, () => {
  console.log(`Preview server listening on http://${host}:${port}`);
  console.log(`Content-Security-Policy: ${securityHeaders['Content-Security-Policy']}`);
});
