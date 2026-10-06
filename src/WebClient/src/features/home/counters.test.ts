// @vitest-environment jsdom
import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { afterEach, beforeEach, expect, it, vi } from 'vitest';
import { webApiClient } from '~/lib/api/webapi-client';
import { resourceMetadata } from '~/lib/api/resource-metadata';
import { loadHomeCounters } from './counters';

beforeEach(() => {
  document.body.innerHTML = new DOMParser().parseFromString(readFileSync(resolve('dist/index.html'), 'utf8'), 'text/html').body.innerHTML;
});
afterEach(() => vi.restoreAllMocks());

it('loads every generated home card with minimal pages and distinguishes failures from zero', async () => {
  vi.spyOn(webApiClient, 'list').mockImplementation(async key => {
    if (key === 'users') throw new Error('Forbidden');
    return { items: [], totalCount: 7 };
  });
  await loadHomeCounters(document.querySelector<HTMLElement>('[data-home]')!, new AbortController().signal, { sub: 'admin', login: 'admin', isAdmin: true });
  expect(webApiClient.list).toHaveBeenCalledTimes(resourceMetadata.length);
  for (const resource of resourceMetadata) expect(webApiClient.list).toHaveBeenCalledWith(resource.key, 1, 1, expect.any(AbortSignal));
  expect(document.querySelector('[data-count="users"]')?.textContent).toBe('Unavailable');
  expect(document.querySelector('[data-count="projects"]')?.textContent).toBe('7');
  expect(document.querySelector('[data-home-summary]')?.textContent).toBe(`Record counts loaded; 1 of ${resourceMetadata.length} work areas are unavailable.`);
});

it('announces one summary instead of eleven live counters', async () => {
  expect(document.querySelectorAll('[data-count][aria-live]')).toHaveLength(0);
  expect(document.querySelectorAll('[aria-live], [role="status"], [role="alert"]').length).toBeLessThanOrEqual(2);
  const summary = document.querySelector('[data-home-summary]')!;
  expect(summary.getAttribute('role')).toBe('status');
  vi.spyOn(webApiClient, 'list').mockResolvedValue({ items: [], totalCount: 3 });
  await loadHomeCounters(document.querySelector<HTMLElement>('[data-home]')!, new AbortController().signal, { sub: 'admin', login: 'admin', isAdmin: true });
  expect(summary.textContent).toBe(`Record counts loaded for ${resourceMetadata.length} work areas.`);
  expect(document.querySelector('[data-work-areas]')?.getAttribute('aria-busy')).toBe('false');
});

it('does not request the admin-only team count for non-admin sessions', async () => {
  vi.spyOn(webApiClient, 'list').mockResolvedValue({ items: [], totalCount: 2 });
  await loadHomeCounters(document.querySelector<HTMLElement>('[data-home]')!, new AbortController().signal, { sub: 'user-7', login: 'learner', isAdmin: false });
  expect(vi.mocked(webApiClient.list).mock.calls.some(([key]) => key === 'users')).toBe(false);
  expect(document.querySelector('[data-count="users"]')?.textContent).toBe('Admin only');
  expect(document.querySelector('[data-count="organizations"]')?.textContent).toBe('2');
});
