import { component$, useSignal, useVisibleTask$ } from '@builder.io/qwik';

type Theme = 'light' | 'dark';

const storageKey = 'cpnucleo-theme';

const applyTheme = (theme: Theme) => {
  document.documentElement.dataset.theme = theme;
  document.documentElement.style.colorScheme = theme;
};

export const ThemeToggle = component$(() => {
  const theme = useSignal<Theme>('dark');

  useVisibleTask$(() => {
    const stored = window.localStorage.getItem(storageKey);
    const next = stored === 'dark' || stored === 'light'
      ? stored
      : 'dark';
    theme.value = next;
    applyTheme(next);
  });

  return (
    <button
      type="button"
      class="inline-flex items-center gap-2 rounded-full border border-line bg-raised px-3 py-2 text-sm font-semibold text-ink shadow-sm transition hover:border-accent/40 hover:text-accent"
      aria-label={`Switch to ${theme.value === 'dark' ? 'light' : 'dark'} mode`}
      onClick$={() => {
        const next: Theme = theme.value === 'dark' ? 'light' : 'dark';
        theme.value = next;
        window.localStorage.setItem(storageKey, next);
        applyTheme(next);
      }}
    >
      <span aria-hidden="true">{theme.value === 'dark' ? '☾' : '☀'}</span>
      <span>{theme.value === 'dark' ? 'Dark' : 'Light'}</span>
    </button>
  );
});
