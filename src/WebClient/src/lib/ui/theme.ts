export type ThemePreference = 'light' | 'dark' | 'system';
export const themeStorageKey = 'cpnucleo-theme';

/** The stored choice; dark is the workspace default when nothing was chosen. */
export const readThemePreference = (): ThemePreference => {
  try {
    const stored = localStorage.getItem(themeStorageKey);
    return stored === 'light' || stored === 'system' ? stored : 'dark';
  } catch {
    return 'dark';
  }
};

const systemPrefersLight = () => typeof matchMedia === 'function' && matchMedia('(prefers-color-scheme: light)').matches;

export const resolveTheme = (preference: ThemePreference): 'light' | 'dark' =>
  preference === 'system' ? (systemPrefersLight() ? 'light' : 'dark') : preference;

/** Applies and remembers a choice; "system" follows the operating system while the page is open. */
export const applyThemePreference = (preference: ThemePreference) => {
  const theme = resolveTheme(preference);
  document.documentElement.dataset.theme = theme;
  document.documentElement.style.colorScheme = theme;
  try {
    localStorage.setItem(themeStorageKey, preference);
  } catch {
    // Storage can be unavailable (privacy mode); the theme still applies to this page.
  }
};
