import type { Config } from 'tailwindcss';

export default {
  content: ['./src/**/*.{tsx,ts,jsx,js,mdx}'],
  theme: {
    extend: {
      colors: {
        canvas: 'oklch(var(--canvas) / <alpha-value>)',
        surface: 'oklch(var(--surface) / <alpha-value>)',
        raised: 'oklch(var(--raised) / <alpha-value>)',
        ink: 'oklch(var(--ink) / <alpha-value>)',
        muted: 'oklch(var(--muted) / <alpha-value>)',
        line: 'oklch(var(--line) / <alpha-value>)',
        accent: 'oklch(var(--accent) / <alpha-value>)',
        danger: 'oklch(var(--danger) / <alpha-value>)',
        success: 'oklch(var(--success) / <alpha-value>)',
      },
      boxShadow: {
        soft: '0 1px 2px rgb(15 23 42 / 0.06), 0 8px 24px rgb(15 23 42 / 0.05)',
      },
    },
  },
  plugins: [],
} satisfies Config;
