import { defineConfig } from 'astro/config';
import sitemap from '@astrojs/sitemap';
import tailwindcss from '@tailwindcss/vite';

export default defineConfig({
  integrations: [sitemap()],
  output: 'static',
  outDir: 'out',
  // The custom domain serves this project at the host root. A repository
  // base path would make every asset 404 once GitHub Pages leaves
  // jonathanperis.github.io/cpnucleo/.
  site: 'https://cpnucleo.jonathanperis.tech',
  base: '',
  markdown: {
    // Paper and night listings; docs.css swaps to the dark palette with prefers-color-scheme.
    shikiConfig: { themes: { light: 'rose-pine-dawn', dark: 'rose-pine' } },
  },
  vite: {
    plugins: [tailwindcss()],
  },
});
