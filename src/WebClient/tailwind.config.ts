import type { Config } from 'tailwindcss';

export default {
  content: ['./src/**/*.{astro,tsx,ts,jsx,js,mdx}'],
  theme: {
    extend: {
      colors: {
        canvas: 'oklch(var(--canvas) / <alpha-value>)',
        surface: 'oklch(var(--surface) / <alpha-value>)',
        raised: 'oklch(var(--raised) / <alpha-value>)',
        ink: 'oklch(var(--ink) / <alpha-value>)',
        muted: 'oklch(var(--muted) / <alpha-value>)',
        subtle: 'oklch(var(--subtle) / <alpha-value>)',
        line: 'oklch(var(--line) / <alpha-value>)',
        'line-strong': 'oklch(var(--line-strong) / <alpha-value>)',
        accent: 'oklch(var(--accent) / <alpha-value>)',
        'accent-hover': 'oklch(var(--accent-hover) / <alpha-value>)',
        'accent-ink': 'oklch(var(--accent-ink) / <alpha-value>)',
        'accent-text': 'oklch(var(--accent-text) / <alpha-value>)',
        action: 'oklch(var(--action) / <alpha-value>)',
        'action-ink': 'oklch(var(--action-ink) / <alpha-value>)',
        danger: 'oklch(var(--danger) / <alpha-value>)',
        success: 'oklch(var(--success) / <alpha-value>)',
        warning: 'oklch(var(--warning) / <alpha-value>)',
      },
      fontFamily: {
        sans: ['"IBM Plex Sans Variable"', 'ui-sans-serif', 'system-ui', '-apple-system', 'BlinkMacSystemFont', '"Segoe UI"', 'sans-serif'],
        mono: ['"IBM Plex Mono"', 'ui-monospace', 'SFMono-Regular', 'Menlo', 'monospace'],
        display: ['"Newsreader Variable"', '"Iowan Old Style"', 'Georgia', 'serif'],
      },
      boxShadow: {
        soft: '0 1px 2px oklch(var(--shadow-color) / 0.08), 0 12px 32px -16px oklch(var(--shadow-color) / 0.22)',
      },
    },
  },
  plugins: [],
} satisfies Config;
