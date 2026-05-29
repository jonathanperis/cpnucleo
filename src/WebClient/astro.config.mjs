import { defineConfig } from 'astro/config';
import { satteri } from '@astrojs/markdown-satteri';
import qwikdev from '@qwikdev/astro';

export default defineConfig({
  output: 'static',
  integrations: [qwikdev()],
  markdown: {
    processor: satteri(),
  },
  server: {
    host: '0.0.0.0',
    port: 5030,
  },
  vite: {
    build: {
      rollupOptions: {
        output: {
          manualChunks: undefined,
          onlyExplicitManualChunks: false,
        },
      },
    },
  },
});
