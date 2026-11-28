import { defineConfig } from 'astro/config';
import sitemap from '@astrojs/sitemap';
import tailwindcss from '@tailwindcss/vite';

export default defineConfig({
  integrations: [sitemap()],
  output: 'static',
  outDir: 'out',
  // GitHub Pages serves this project site under the owner's custom domain
  // at https://jonathanperis.dev/cpnucleo/, so every route carries the base.
  site: 'https://jonathanperis.dev',
  base: '/cpnucleo',
  markdown: {
    // Paper and night listings; docs.css swaps to the dark palette with prefers-color-scheme.
    shikiConfig: { themes: { light: 'rose-pine-dawn', dark: 'rose-pine' } },
  },
  vite: {
    plugins: [tailwindcss()],
  },
});
