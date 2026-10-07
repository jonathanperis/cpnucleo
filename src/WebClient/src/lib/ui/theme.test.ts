// @vitest-environment jsdom
import { afterEach, describe, expect, it, vi } from 'vitest';
import { applyThemePreference, readThemePreference, resolveTheme, themeStorageKey } from './theme';

afterEach(() => { localStorage.clear(); vi.unstubAllGlobals(); });

describe('theme preference', () => {
  it('is dark by default and follows the operating system only when asked to', () => {
    vi.stubGlobal('matchMedia', (query: string) => ({ matches: query.includes('light') }));
    expect(readThemePreference()).toBe('dark');
    expect(resolveTheme('system')).toBe('light');
    applyThemePreference('system');
    expect(localStorage.getItem(themeStorageKey)).toBe('system');
    expect(readThemePreference()).toBe('system');
    expect(document.documentElement.dataset.theme).toBe('light');
    applyThemePreference('dark');
    expect(document.documentElement.dataset.theme).toBe('dark');
    expect(document.documentElement.style.colorScheme).toBe('dark');
  });
});
