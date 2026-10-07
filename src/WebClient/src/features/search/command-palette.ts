import { getSessionClaims, type SessionClaims } from '~/lib/api/http-client';
import { recordHref, resourceMetadata } from '~/lib/api/resource-metadata';
import type { ApiEntity, ResourceMetadata } from '~/lib/api/types';
import { webApiClient } from '~/lib/api/webapi-client';
import { createIcon, resourceIcons, type IconName } from '~/lib/icons';
import { navigation } from '~/lib/navigation';
import { displayEntityLabel } from '~/features/crud/relation-display';

export const SEARCH_DEBOUNCE_MS = 250;
export const RESULTS_PER_AREA = 5;
export const MIN_QUERY_LENGTH = 2;

interface PaletteResult {
  label: string;
  detail: string;
  href: string;
  icon: IconName;
}

/** Areas worth searching by text: link records (people on tasks…) have no text of their own. */
export const searchableResources = (session: SessionClaims | null): ResourceMetadata[] =>
  resourceMetadata.filter(resource => resource.displayField !== 'id' && (!resource.access.adminRead || session?.isAdmin === true));

const pageResults = (query: string): PaletteResult[] => {
  const needle = query.trim().toLowerCase();
  return navigation.flatMap(group => group.items
    .filter(item => !needle || item.label.toLowerCase().includes(needle))
    .map(item => ({ label: item.label, detail: group.name, href: item.href, icon: item.icon })));
};

/**
 * Ctrl/⌘+K search across pages and every readable work area. Records come from each list
 * endpoint's `search` filter, so results respect the same access rules as the lists.
 */
export const mountCommandPalette = (dialog: HTMLDialogElement, session: SessionClaims | null = getSessionClaims()): (() => void) => {
  const input = dialog.querySelector<HTMLInputElement>('[data-palette-input]')!;
  const list = dialog.querySelector<HTMLElement>('[data-palette-results]')!;
  const status = dialog.querySelector<HTMLElement>('[data-palette-status]')!;
  const lifetime = new AbortController();
  let search = new AbortController();
  let timer: ReturnType<typeof setTimeout> | undefined;
  let active = 0;

  const links = () => [...list.querySelectorAll<HTMLAnchorElement>('a[data-palette-item]')];
  const highlight = (index: number) => {
    const items = links();
    if (items.length === 0) { input.removeAttribute('aria-activedescendant'); return; }
    active = (index + items.length) % items.length;
    items.forEach((item, position) => item.setAttribute('aria-selected', String(position === active)));
    input.setAttribute('aria-activedescendant', items[active].id);
    items[active].scrollIntoView?.({ block: 'nearest' });
  };

  const renderGroups = (groups: { heading: string; results: PaletteResult[] }[]) => {
    list.replaceChildren();
    let index = 0;
    for (const group of groups.filter(candidate => candidate.results.length > 0)) {
      const heading = document.createElement('li');
      heading.className = 'eyebrow px-3 pb-1 pt-3';
      heading.setAttribute('role', 'presentation');
      heading.textContent = group.heading;
      list.append(heading);
      for (const result of group.results) {
        const item = document.createElement('li');
        item.setAttribute('role', 'presentation');
        const link = document.createElement('a');
        link.href = result.href;
        link.id = `palette-option-${index++}`;
        link.dataset.paletteItem = '';
        link.setAttribute('role', 'option');
        link.className = 'palette-item';
        const text = document.createElement('span');
        text.className = 'min-w-0 flex-1 truncate';
        text.textContent = result.label;
        const detail = document.createElement('span');
        detail.className = 'flex-none text-xs text-subtle';
        detail.textContent = result.detail;
        link.append(createIcon(result.icon, 'size-4 flex-none text-subtle'), text, detail);
        item.append(link);
        list.append(item);
      }
    }
    highlight(0);
  };

  const run = async (query: string) => {
    search.abort(); search = new AbortController();
    const signal = search.signal;
    const pages = { heading: 'Pages', results: pageResults(query) };
    if (query.trim().length < MIN_QUERY_LENGTH) {
      renderGroups([pages]);
      status.textContent = query.trim() ? `Type at least ${MIN_QUERY_LENGTH} characters to search records.` : '';
      return;
    }
    renderGroups([pages]);
    status.textContent = 'Searching…';
    const areas = await Promise.all(searchableResources(session).map(async resource => {
      const result = await webApiClient.list<ApiEntity>(resource.key, 1, RESULTS_PER_AREA, signal, query).catch(() => ({ items: [] as ApiEntity[] }));
      return {
        heading: resource.pluralLabel,
        results: (result.items ?? []).map(item => ({
          label: displayEntityLabel(item), detail: resource.label, href: recordHref(resource.key, String(item.id)), icon: resourceIcons[resource.key],
        })),
      };
    }));
    if (signal.aborted || lifetime.signal.aborted) return;
    renderGroups([pages, ...areas]);
    const count = areas.reduce((sum, area) => sum + area.results.length, 0);
    status.textContent = count === 0 ? `No records match “${query.trim()}”.` : `${count} record${count === 1 ? '' : 's'} found.`;
  };

  const open = () => {
    if (dialog.open) return;
    input.value = '';
    void run('');
    dialog.showModal();
    input.focus();
  };

  input.addEventListener('input', () => {
    clearTimeout(timer);
    timer = setTimeout(() => void run(input.value), SEARCH_DEBOUNCE_MS);
  }, { signal: lifetime.signal });
  input.addEventListener('keydown', event => {
    if (event.key === 'ArrowDown') { event.preventDefault(); highlight(active + 1); }
    else if (event.key === 'ArrowUp') { event.preventDefault(); highlight(active - 1); }
    else if (event.key === 'Enter') {
      const target = links()[active];
      if (target) { event.preventDefault(); target.click(); }
    }
    // A search field's first Escape would only clear the text; close the palette instead.
    else if (event.key === 'Escape') { event.preventDefault(); dialog.close(); }
  }, { signal: lifetime.signal });
  dialog.addEventListener('click', event => { if (event.target === dialog) dialog.close(); }, { signal: lifetime.signal });
  dialog.addEventListener('close', () => { clearTimeout(timer); search.abort(); }, { signal: lifetime.signal });
  document.addEventListener('keydown', event => {
    if ((event.metaKey || event.ctrlKey) && event.key.toLowerCase() === 'k') {
      event.preventDefault();
      if (dialog.open) dialog.close(); else open();
    }
  }, { signal: lifetime.signal });
  for (const trigger of document.querySelectorAll('[data-open-palette]')) trigger.addEventListener('click', open, { signal: lifetime.signal });

  return () => { clearTimeout(timer); search.abort(); lifetime.abort(); };
};
