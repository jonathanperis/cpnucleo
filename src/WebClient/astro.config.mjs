import { defineConfig } from 'astro/config';
import { satteri } from '@astrojs/markdown-satteri';
import tailwindcss from '@tailwindcss/vite';

export default defineConfig({
  output: 'static',
  vite: { plugins: [tailwindcss()] },
  markdown: {
    processor: satteri(),
  },
  server: {
    host: '0.0.0.0',
    port: 5030,
  },
});
