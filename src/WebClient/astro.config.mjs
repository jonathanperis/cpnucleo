import { defineConfig } from 'astro/config';
import { satteri } from '@astrojs/markdown-satteri';
import tailwindcss from '@tailwindcss/vite';
import { loadEnv } from 'vite';
import { cspManifestIntegration } from './scripts/csp.mjs';

// Same PUBLIC_* values Astro/Vite inline into the browser bundle (process env + .env files).
const publicEnv = loadEnv(process.env.NODE_ENV === 'development' ? 'development' : 'production', process.cwd(), 'PUBLIC_');

export default defineConfig({
  output: 'static',
  integrations: [cspManifestIntegration({ env: publicEnv })],
  vite: {
    plugins: [tailwindcss()],
    // Fonts stay same-origin files: inlined data: URLs would be blocked by the CSP (no font-src data:).
    build: { assetsInlineLimit: file => (file.endsWith('.woff2') ? false : undefined) },
  },
  markdown: {
    processor: satteri(),
  },
  server: {
    host: '0.0.0.0',
    port: 5030,
  },
});
