import { defineConfig } from 'astro/config';
import { satteri } from '@astrojs/markdown-satteri';

export default defineConfig({
  output: 'static',
  markdown: {
    processor: satteri(),
  },
  server: {
    host: '0.0.0.0',
    port: 5030,
  },
});
