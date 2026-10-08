import { globSync, readFileSync } from 'node:fs';
import { parse } from 'postcss';
import { expect, it } from 'vitest';

it('emits themed and responsive utilities used by the static templates', () => {
  const source = globSync('dist/_astro/*.css').map(path => readFileSync(path, 'utf8')).join('\n');
  // The CSP allows fonts only from 'self'; Vite must not inline them as data: URLs.
  expect(source).not.toContain('url(data:font');
  expect(source).toMatch(/url\("?\/_astro\/ibm-plex-sans-[^)]+\.woff2/);
  expect(source).toMatch(/url\("?\/_astro\/newsreader-[^)]+\.woff2/);
  const css = parse(source);
  const declarations = new Map<string, string>();
  css.walkRules(rule => {
    rule.walkDecls(declaration => {
      for (const selector of rule.selectors) {
        declarations.set(`${selector}:${declaration.prop}`, declaration.value);
      }
    });
  });

  expect(declarations.get('.text-muted:color')).toContain('var(--muted)');
  expect(declarations.get('.border-line:border-color')).toContain('var(--line)');
  expect(declarations.get('.bg-surface:background-color')).toContain('var(--surface)');
  expect(declarations.get('.lg\\:flex:display')).toBe('flex');
  expect(declarations.get('.hidden:display')).toBe('none');
  // Shared component classes (buttons, fields, table) resolve to the theme tokens.
  expect(declarations.get('.btn-primary:background')).toContain('var(--action)');
  expect(declarations.get('.brand-mark .nucleus:fill')).toContain('var(--accent)');
  expect(declarations.get('.field:border')).toContain('var(--line-strong)');
  expect(declarations.get('.page-button[aria-current=page]:color')).toContain('var(--accent-text)');
});
