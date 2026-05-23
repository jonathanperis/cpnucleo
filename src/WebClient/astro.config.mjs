import { defineConfig } from 'astro/config';
import qwikdev from '@qwikdev/astro';

export default defineConfig({
  output: 'static',
  integrations: [qwikdev()],
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
