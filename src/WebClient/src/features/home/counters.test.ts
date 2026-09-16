// @vitest-environment jsdom
import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { afterEach, expect, it, vi } from 'vitest';
import { webApiClient } from '~/lib/api/webapi-client';
import { resourceMetadata } from '~/lib/api/resource-metadata';
import { loadHomeCounters } from './counters';

afterEach(() => vi.restoreAllMocks());
it('loads every generated home card with minimal pages and distinguishes failures from zero', async () => {
  document.body.innerHTML = new DOMParser().parseFromString(readFileSync(resolve('dist/index.html'), 'utf8'), 'text/html').body.innerHTML;
  vi.spyOn(webApiClient, 'list').mockImplementation(async key => {
    if (key === 'users') throw new Error('Forbidden');
    return { items: [], totalCount: 7 };
  });
  await loadHomeCounters(document.querySelector<HTMLElement>('[data-home]')!, new AbortController().signal);
  expect(webApiClient.list).toHaveBeenCalledTimes(resourceMetadata.length);
  for (const resource of resourceMetadata) expect(webApiClient.list).toHaveBeenCalledWith(resource.key, 1, 1, expect.any(AbortSignal));
  expect(document.querySelector('[data-count="users"]')?.textContent).toBe('Unavailable');
  expect(document.querySelector('[data-count="projects"]')?.textContent).toBe('7');
});
