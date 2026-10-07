// @vitest-environment jsdom
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import type { SessionClaims } from '~/lib/api/http-client';
import { webApiClient } from '~/lib/api/webapi-client';
import { polyfillDialogs, showBuiltPage } from '~/test/page-markup';
import { mountCommandPalette, searchableResources, SEARCH_DEBOUNCE_MS } from './command-palette';

const member: SessionClaims = { sub: 'user-7', login: 'learner', isAdmin: false };
let stop = () => {};
beforeEach(() => { polyfillDialogs(); showBuiltPage('projects'); });
afterEach(() => { stop(); vi.restoreAllMocks(); vi.useRealTimers(); document.body.replaceChildren(); });

describe('command palette', () => {
  it('searches only areas with their own text that the person can read', () => {
    const keys = searchableResources(member).map(resource => resource.key);
    expect(keys).not.toContain('users');
    expect(keys).not.toContain('userProjects');
    expect(keys).toContain('assignments');
    expect(searchableResources({ ...member, isAdmin: true }).map(resource => resource.key)).toContain('users');
  });

  it('opens with Ctrl+K, lists pages, then searches records and opens the highlighted result with Enter', async () => {
    vi.useFakeTimers();
    vi.spyOn(webApiClient, 'list').mockImplementation(async key => ({ items: key === 'projects' ? [{ id: 'p-1', name: 'Atlas' }] : [], totalCount: 1 }));
    const dialog = document.querySelector<HTMLDialogElement>('[data-palette]')!;
    stop = mountCommandPalette(dialog, member);
    document.dispatchEvent(new KeyboardEvent('keydown', { key: 'k', ctrlKey: true, bubbles: true }));
    expect(dialog.open).toBe(true);
    const input = dialog.querySelector<HTMLInputElement>('[data-palette-input]')!;
    expect(dialog.textContent).toContain('Board');
    input.value = 'atl';
    input.dispatchEvent(new Event('input'));
    await vi.advanceTimersByTimeAsync(SEARCH_DEBOUNCE_MS);
    await vi.waitFor(() => expect(dialog.querySelector('[data-palette-status]')?.textContent).toBe('1 record found.'));
    expect(webApiClient.list).toHaveBeenCalledWith('projects', 1, 5, expect.any(AbortSignal), 'atl');
    const result = dialog.querySelector<HTMLAnchorElement>('a[href="/projects/view/?id=p-1"]')!;
    expect(result.textContent).toContain('Atlas');
    expect(result.getAttribute('aria-selected')).toBe('true');
    expect(input.getAttribute('aria-activedescendant')).toBe(result.id);
    const click = vi.spyOn(result, 'click').mockImplementation(() => undefined);
    input.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter', bubbles: true, cancelable: true }));
    expect(click).toHaveBeenCalled();
    input.dispatchEvent(new KeyboardEvent('keydown', { key: 'Escape', bubbles: true, cancelable: true }));
    expect(dialog.open).toBe(false);
  });
});
