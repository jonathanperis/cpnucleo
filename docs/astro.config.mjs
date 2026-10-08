import { defineConfig } from 'astro/config';
import sitemap from '@astrojs/sitemap';
import tailwindcss from '@tailwindcss/vite';

const isProd = process.env.NODE_ENV === 'production';

export default defineConfig({
  integrations: [sitemap()],
  output: 'static',
  outDir: 'out',
  site: 'https://jonathanperis.github.io',
  base: isProd ? '/cpnucleo' : '',
  markdown: {
    // Paper and night listings; docs.css swaps to the dark palette with prefers-color-scheme.
    shikiConfig: { themes: { light: 'rose-pine-dawn', dark: 'rose-pine' } },
  },
  vite: {
    plugins: [tailwindcss()],
  },
});
