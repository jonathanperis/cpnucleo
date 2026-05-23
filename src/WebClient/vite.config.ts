import { defineConfig } from 'vite';
import { qwikVite } from '@builder.io/qwik/optimizer';
import { qwikCity } from '@builder.io/qwik-city/vite';
import { staticAdapter } from '@builder.io/qwik-city/adapters/static/vite';
import tsconfigPaths from 'vite-tsconfig-paths';

export default defineConfig(({ mode }) => ({
  plugins: mode === 'test' ? [tsconfigPaths()] : [qwikCity(), qwikVite(), staticAdapter({ origin: 'http://localhost:5030' }), tsconfigPaths()],
  preview: {
    host: '0.0.0.0',
    port: 5030,
    strictPort: true,
  },
  server: {
    host: '0.0.0.0',
    port: 5030,
  },
  test: {
    environment: 'node',
    globals: true,
    include: ['src/**/*.test.ts'],
  },
  build: {
    rollupOptions: {
      input: ['src/entry.ssr.tsx', '@qwik-city-plan'],
    },
  },
}));
