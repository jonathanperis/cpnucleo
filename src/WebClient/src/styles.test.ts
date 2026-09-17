import { globSync, readFileSync } from 'node:fs';
import { parse } from 'postcss';
import { expect, it } from 'vitest';

it('emits themed and responsive utilities used by the static templates', () => {
  const css = parse(globSync('dist/_astro/*.css').map(path => readFileSync(path, 'utf8')).join('\n'));
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
  expect(declarations.get('.lg\\:block:display')).toBe('block');
  expect(declarations.get('.hidden:display')).toBe('none');
});
