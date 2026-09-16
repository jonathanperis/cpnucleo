// @vitest-environment jsdom
import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { afterEach, beforeEach, expect, it, vi } from 'vitest';
import { webApiClient } from '~/lib/api/webapi-client';
import { mountCrudPage } from './crud-controller';

let stop = () => {};
const pageMarkup = (route: string) => new DOMParser().parseFromString(
  readFileSync(resolve('dist', route, 'index.html'), 'utf8'), 'text/html').body.innerHTML;

beforeEach(() => {
  vi.spyOn(webApiClient, 'list').mockResolvedValue({ items: [{ id: 'org', name: 'School' }], totalCount: 1 });
  vi.spyOn(webApiClient, 'lookup').mockResolvedValue([{ id: 'org', name: 'School' }]);
  vi.spyOn(webApiClient, 'subscribeList').mockImplementation(async (_key, _page, _size, onPage, signal) => {
    onPage({ items: [{ id: 'one', createdAt: '2026-09-16T10:00:00Z', name: '<img src=x onerror=alert(1)>', login: 'learner', organizationId: 'org' }], totalCount: 55 });
    await new Promise<void>(resolve => signal?.addEventListener('abort', () => resolve(), { once: true }));
  });
});
afterEach(() => { stop(); vi.restoreAllMocks(); document.body.replaceChildren(); });

it('uses generated Astro controls to prefill and save, without rendering API data as HTML', async () => {
  document.body.innerHTML = pageMarkup('projects');
  const root = document.querySelector<HTMLElement>('[data-crud]')!;
  const update = vi.spyOn(webApiClient, 'update').mockResolvedValue({ success: true });
  stop = mountCrudPage(root);
  await vi.waitFor(() => expect(root.querySelector('[data-records]')?.textContent).toContain('School'));
  expect(root.querySelector('[data-records] img')).toBeNull();
  root.querySelector<HTMLButtonElement>('[data-action="edit"]')!.click();
  const form = root.querySelector<HTMLFormElement>('[data-form]')!;
  const name = form.elements.namedItem('name') as HTMLInputElement;
  expect(name.value).toContain('<img');
  name.value = 'Updated project';
  await vi.waitFor(() => expect((form.elements.namedItem('organizationId') as HTMLSelectElement).value).toBe('org'));
  form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
  await vi.waitFor(() => expect(update).toHaveBeenCalledWith('projects', 'one', { name: 'Updated project', organizationId: 'org', expectedVersion: '2026-09-16T10:00:00Z' }));
  await vi.waitFor(() => expect(form.hidden).toBe(true));
});

it('keeps the password blank and requires it for creation, but not ordinary user edits', async () => {
  document.body.innerHTML = pageMarkup('users');
  const root = document.querySelector<HTMLElement>('[data-crud]')!;
  stop = mountCrudPage(root);
  root.querySelector<HTMLButtonElement>('[data-action="create"]')!.click();
  const password = root.querySelector<HTMLInputElement>('[name="password"]')!;
  expect(password.required).toBe(true); expect(password.value).toBe('');
  root.querySelector<HTMLButtonElement>('[data-action="edit"]')!.click();
  expect(password.required).toBe(false); expect(password.value).toBe('');
});

it('ignores responses from an aborted page and updates the accessible current-page control', async () => {
  document.body.innerHTML = pageMarkup('projects');
  const root = document.querySelector<HTMLElement>('[data-crud]')!;
  stop = mountCrudPage(root);
  const previous = vi.mocked(webApiClient.subscribeList).mock.calls[0][3];
  root.querySelector<HTMLButtonElement>('[data-action="page"][data-page="2"]')!.click();
  previous({ items: [{ id: 'stale', name: 'Stale page' }], totalCount: 1 });
  expect(root.querySelector('[data-records]')?.textContent).not.toContain('Stale page');
  expect(root.querySelector('[aria-current="page"]')?.getAttribute('aria-label')).toBe('Page 2');
  expect(vi.mocked(webApiClient.subscribeList).mock.calls[0][4]?.aborted).toBe(true);
});

it('searches and paginates relation options beyond the first page', async () => {
  document.body.innerHTML = pageMarkup('projects');
  const root = document.querySelector<HTMLElement>('[data-crud]')!;
  vi.mocked(webApiClient.list).mockResolvedValue({ items: [{ id: 'org', name: 'School' }], totalCount: 150 });
  stop = mountCrudPage(root);
  root.querySelector<HTMLButtonElement>('[data-action="create"]')!.click();
  const container = root.querySelector<HTMLElement>('[data-relation]')!;
  await vi.waitFor(() => expect(container.querySelector<HTMLButtonElement>('[data-action="more"]')!.hidden).toBe(false));
  container.querySelector<HTMLButtonElement>('[data-action="more"]')!.click();
  await vi.waitFor(() => expect(webApiClient.list).toHaveBeenCalledWith('organizations', 2, 100, expect.any(AbortSignal), ''));
  container.querySelector<HTMLInputElement>('[data-query]')!.value = 'School';
  container.querySelector<HTMLButtonElement>('[data-action="search"]')!.click();
  await vi.waitFor(() => expect(webApiClient.list).toHaveBeenCalledWith('organizations', 1, 100, expect.any(AbortSignal), 'School'));
});
