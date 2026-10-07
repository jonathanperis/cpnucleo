let installed = false;

/**
 * `<details class="menu">` popovers: close on Escape (returning focus to the summary), on a click
 * outside, and when another menu opens. Safe to call from several components.
 */
export const installMenuBehavior = () => {
  if (installed || typeof document === 'undefined') return;
  installed = true;
  const openMenus = () => [...document.querySelectorAll<HTMLDetailsElement>('details.menu[open]')];
  document.addEventListener('click', event => {
    for (const menu of openMenus()) if (!menu.contains(event.target as Node)) menu.open = false;
  });
  document.addEventListener('keydown', event => {
    if (event.key !== 'Escape') return;
    const menu = openMenus().find(candidate => candidate.contains(document.activeElement));
    if (!menu) return;
    menu.open = false;
    menu.querySelector('summary')?.focus();
  });
  document.addEventListener('toggle', event => {
    const opened = event.target as HTMLDetailsElement;
    if (!(opened instanceof HTMLDetailsElement) || !opened.open || !opened.classList.contains('menu')) return;
    for (const menu of openMenus()) if (menu !== opened) menu.open = false;
  }, true);
};
