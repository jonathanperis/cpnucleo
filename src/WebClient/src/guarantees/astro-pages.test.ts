import { existsSync, readdirSync, readFileSync } from 'node:fs';
import { join, relative, resolve } from 'node:path';
import { describe, expect, it } from 'vitest';
import { canonicalizeStaticRoute } from '~/lib/auth-navigation';

// Owner requirement: every page of the WebClient and of the docs site is an Astro page, and no
// client UI framework is installed. Also enforced by scripts/check-docs-drift.py and
// tests/Architecture.Tests/AstroPagesTests.cs.

const webClientRoot = resolve('.');
const docsRoot = resolve('../../docs');

const forbiddenFrameworkPackages = [
  'react', 'react-dom', 'preact', 'vue', 'petite-vue', 'svelte',
  'solid-js', 'lit', 'lit-html', 'alpinejs', 'htmx.org', '@angular/core',
  '@stencil/core', '@builder.io/qwik', '@builder.io/qwik-city', '@qwik.dev/core', '@qwik.dev/router', '@qwikdev/astro',
  '@astrojs/react', '@astrojs/vue', '@astrojs/svelte', '@astrojs/solid-js', '@astrojs/preact', '@astrojs/lit',
  '@astrojs/alpinejs', '@analogjs/astro-angular',
];

const sites = [
  { name: 'WebClient', root: webClientRoot },
  { name: 'docs', root: docsRoot },
];

const filesUnder = (directory: string): string[] => (existsSync(directory)
  ? readdirSync(directory, { withFileTypes: true, recursive: true })
    // Symlinks count as files: a linked page or asset must not slip past the checks.
    .filter(entry => entry.isFile() || entry.isSymbolicLink())
    .map(entry => join(entry.parentPath, entry.name))
  : []);

const packageNames = (packageJsonPath: string) => {
  const manifest = JSON.parse(readFileSync(packageJsonPath, 'utf8')) as Record<string, Record<string, string> | undefined>;
  const sections = ['dependencies', 'devDependencies', 'peerDependencies', 'optionalDependencies', 'overrides'];
  const names = sections.flatMap(section => Object.keys(manifest[section] ?? {}));
  // npm aliases ("ui": "npm:react@19") hide the real package behind another name.
  const aliasTargets = sections.flatMap(section => Object.values(manifest[section] ?? {}))
    .filter((value): value is string => typeof value === 'string' && value.startsWith('npm:'))
    .map(value => /^npm:((?:@[^/@]+\/)?[^@]+)/.exec(value)?.[1] ?? '');
  return [...names, ...aliasTargets];
};

const astroGeneratorMeta = /<meta name="generator" content="Astro v\d/;

describe.each(sites)('$name pages', ({ root }) => {
  it('are all .astro files', () => {
    const pages = filesUnder(join(root, 'src/pages'));
    expect(pages.length).toBeGreaterThan(0);
    expect(pages.filter(path => !path.endsWith('.astro')).map(path => relative(root, path))).toEqual([]);
  });

  it('do not ship hand-written HTML pages from public/', () => {
    expect(filesUnder(join(root, 'public')).filter(path => /\.html?$/i.test(path)).map(path => relative(root, path))).toEqual([]);
  });

  it('do not depend on a client UI framework', () => {
    expect(packageNames(join(root, 'package.json')).filter(name => forbiddenFrameworkPackages.includes(name))).toEqual([]);
    // Transitive installs count too: the lockfile lists every resolved package.
    const lockfile = readFileSync(join(root, 'bun.lock'), 'utf8');
    expect(forbiddenFrameworkPackages.filter(name => lockfile.includes(`"${name}@`))).toEqual([]);
    const config = readFileSync(join(root, 'astro.config.mjs'), 'utf8');
    expect(forbiddenFrameworkPackages.filter(name => config.includes(`'${name}'`) || config.includes(`"${name}"`))).toEqual([]);
  });

  it('declare the Astro generator so built HTML can be traced to Astro', () => {
    const layouts = filesUnder(join(root, 'src')).filter(path => path.endsWith('.astro') && readFileSync(path, 'utf8').includes('<html'));
    expect(layouts.length).toBeGreaterThan(0);
    for (const layout of layouts) expect(readFileSync(layout, 'utf8'), relative(root, layout)).toContain('<meta name="generator" content={Astro.generator} />');
  });
});

describe('WebClient build output', () => {
  it('contains only HTML generated from Astro pages', () => {
    const html = filesUnder(resolve('dist')).filter(path => path.endsWith('.html'));
    expect(html.length).toBeGreaterThan(0);
    const notFromAstro = html.filter(path => !astroGeneratorMeta.test(readFileSync(path, 'utf8')));
    expect(notFromAstro.map(path => relative(webClientRoot, path))).toEqual([]);
  });

  it('hydrates no client component islands and loads only Astro-bundled scripts', () => {
    for (const path of filesUnder(resolve('dist')).filter(file => file.endsWith('.html'))) {
      const html = readFileSync(path, 'utf8');
      // Astro alone never emits islands; one means a client framework renderer slipped in.
      expect(html, relative(webClientRoot, path)).not.toContain('<astro-island');
      const foreign = [...html.matchAll(/<script[^>]*\ssrc="([^"]+)"/g)].map(match => match[1]).filter(src => !src.includes('/_astro/'));
      expect(foreign, relative(webClientRoot, path)).toEqual([]);
    }
  });

  it('builds exactly the documented routes', () => {
    const routes = filesUnder(resolve('dist')).filter(path => path.endsWith('index.html')).map(path => `/${relative(resolve('dist'), path).replace(/index\.html$/, '')}`).sort();
    expect(routes).toEqual([
      '/', '/api-health/', '/appointments/', '/assignment-impediments/', '/assignment-types/', '/assignments/', '/impediments/',
      '/login/', '/organizations/', '/projects/', '/signin-callback/', '/user-assignments/', '/user-projects/', '/users/', '/workflows/',
    ]);
    // Every built page is a known route for login return URLs.
    for (const route of routes.filter(route => route !== '/')) expect(canonicalizeStaticRoute(route.slice(0, -1))).toBe(route);
  });
});
