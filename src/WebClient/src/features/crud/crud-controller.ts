import { filterFields, findResource, formFields, recordHref } from '~/lib/api/resource-metadata';
import { toSortColumn, webApiClient } from '~/lib/api/webapi-client';
import { ApiError, getSessionClaims, type FieldErrors, type SessionClaims } from '~/lib/api/http-client';
import type { ApiEntity, FieldMetadata, ResourceKey } from '~/lib/api/types';
import { createIcon, type IconName } from '~/lib/icons';
import { showToast } from '~/lib/ui/toast';
import { buildPaginationItems, getLastPage } from './pagination';
import { formatFormFieldValue, serializeFormValue, withSelectedRelationOption } from './crud-field-values';
import { collectMissingRelationIds, displayEntityLabel, displayFieldText, mergeRelationRecords, recordLabel } from './relation-display';
import { watchListing } from './watch-list';
import { DEFAULT_SORT_FIELD, defaultOrderFor, parseListState, serializeListState } from './list-state';
import { readHiddenColumns, visibleColumns, writeHiddenColumns } from './columns';

const RELATION_PAGE_SIZE = 100;
export const SEARCH_DEBOUNCE_MS = 300;
const HIGHLIGHT_MS = 2400;
/** Relations whose records have their own page; their cells link there. */
const linkedRelations = new Set<ResourceKey>(['projects', 'assignments']);

interface RelationSearch {
  controller: AbortController;
  /** Query the page counter belongs to; "More" always continues this query. */
  query: string;
  /** Last page successfully loaded for `query` (0 while the first page is in flight). */
  page: number;
}

interface FocusReturn {
  action?: string;
  id?: string;
}

interface ConfirmOptions {
  title?: string;
  accept?: string;
}

interface MountOptions {
  /** Where list state is read from and written to; the page location by default. */
  location?: Pick<Location, 'pathname' | 'search' | 'hash'>;
  history?: Pick<History, 'replaceState' | 'state'>;
}

export const mountCrudPage = (root: HTMLElement, session: SessionClaims | null = getSessionClaims(), { location = window.location, history = window.history }: MountOptions = {}): (() => void) => {
  const resource = findResource(root.dataset.crud as ResourceKey);
  const query = <T extends Element>(selector: string) => root.querySelector<T>(selector)!;
  const form = query<HTMLFormElement>('[data-form]');
  const formDialog = query<HTMLDialogElement>('[data-form-dialog]');
  const alert = query<HTMLElement>('[data-error]');
  const formAlert = query<HTMLElement>('[data-form-error]');
  const dialog = query<HTMLDialogElement>('[data-details]');
  const confirmDialog = query<HTMLDialogElement>('[data-confirm]');
  const createButton = query<HTMLButtonElement>('[data-action="create"]');
  const heading = query<HTMLElement>('[data-heading]');
  const permissionNote = query<HTMLElement>('[data-permission-note]');
  const searchInput = query<HTMLInputElement>('[data-list-search]');
  const selectAll = query<HTMLInputElement>('[data-select-all]');
  const lifetime = new AbortController();
  const relations: Partial<Record<ResourceKey, ApiEntity[]>> = {};
  /** Relation ids already looked up: still missing afterwards means deleted or not visible. */
  const attempted: Partial<Record<ResourceKey, Set<string>>> = {};
  const options: Partial<Record<ResourceKey, ApiEntity[]>> = {};
  const relationSearches: Partial<Record<ResourceKey, RelationSearch>> = {};
  const relationTimers = new Map<HTMLElement, ReturnType<typeof setTimeout>>();
  const { state, intent } = parseListState(resource, location.search);
  const hiddenColumns = readHiddenColumns(resource);
  const selectedIds = new Set<string>();
  const highlightIds = new Set<string>();
  let listing = new AbortController();
  let items: ApiEntity[] = [];
  let selected: ApiEntity | null = null;
  let total = 0;
  let status = 'Connecting';
  let renderedRows = '';
  let renderedPages = '';
  let renderedSummary = '';
  let focusReturn: FocusReturn | null = null;
  let restoreFocusAfterRowsChange = false;
  let initialFormValues = '';
  let searchTimer: ReturnType<typeof setTimeout> | undefined;
  let intentHandled = false;
  let loaded = false;

  // Authorization gating. The APIs enforce these rules; the UI only avoids offering actions that
  // would be rejected and explains why.
  const isAdmin = session?.isAdmin === true;
  const canRead = !resource.access.adminRead || isAdmin;
  const canWrite = !resource.access.adminWrite || isAdmin;
  // Non-admins cannot list /api/users, so person pickers offer only the signed-in user.
  const restrictUserPicker = !isAdmin;
  const currentUser: ApiEntity | null = session ? { id: session.sub, name: session.login, login: session.login } : null;
  if (restrictUserPicker && currentUser) relations.users = [currentUser];

  const fields = formFields(resource);
  const plural = resource.pluralLabel.toLowerCase();
  const singular = resource.label.toLowerCase();
  const fieldControl = (name: string) => form.elements.namedItem(name) as HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement;
  const fieldError = (name: string) => root.querySelector<HTMLElement>(`[data-field-error="${name}"]`);
  const countLabel = (count: number) => `${count} ${count === 1 ? singular : plural}`;

  const messageOf = (error: unknown, fallback = 'Unable to complete the request.') => (error instanceof Error ? error.message : fallback);
  const fail = (error: unknown) => {
    alert.textContent = messageOf(error);
    alert.hidden = false;
  };

  const syncUrl = () => {
    history.replaceState(history.state, '', `${location.pathname}${serializeListState(state)}${location.hash}`);
  };

  const setDescribedBy = (control: Element, id: string, present: boolean) => {
    const ids = new Set((control.getAttribute('aria-describedby') ?? '').split(/\s+/).filter(Boolean));
    if (present) ids.add(id); else ids.delete(id);
    if (ids.size > 0) control.setAttribute('aria-describedby', [...ids].join(' '));
    else control.removeAttribute('aria-describedby');
  };

  const clearFieldErrors = () => {
    formAlert.hidden = true;
    for (const field of fields) {
      const control = fieldControl(field.name);
      const message = fieldError(field.name);
      control.removeAttribute('aria-invalid');
      if (!message) continue;
      message.textContent = '';
      message.hidden = true;
      setDescribedBy(control, message.id, false);
    }
  };

  /** Marks matching controls invalid with an associated description; returns unmatched messages. */
  const markFieldErrors = (errors: FieldErrors): string[] => {
    const unmatched: string[] = [];
    let first: HTMLElement | undefined;
    for (const [key, messages] of Object.entries(errors)) {
      const field = fields.find(candidate => candidate.name.toLowerCase() === key.toLowerCase());
      const message = field && fieldError(field.name);
      if (!field || !message) { unmatched.push(...messages); continue; }
      const control = fieldControl(field.name);
      control.setAttribute('aria-invalid', 'true');
      message.textContent = messages.join(' ');
      message.hidden = false;
      setDescribedBy(control, message.id, true);
      first ??= control;
    }
    first?.focus();
    return unmatched;
  };

  const failForm = (error: unknown) => {
    if (error instanceof ApiError && Object.keys(error.fieldErrors).length > 0) markFieldErrors(error.fieldErrors);
    formAlert.textContent = messageOf(error);
    formAlert.hidden = false;
  };

  const hiddenWithinRoot = (element: HTMLElement) => {
    for (let current: HTMLElement | null = element; current && current !== root; current = current.parentElement) {
      if (current.hidden) return true;
    }
    return false;
  };
  const focusFallback = () => (!hiddenWithinRoot(createButton) ? createButton : heading);

  const restoreFocus = () => {
    const target = focusReturn?.action
      ? [...root.querySelectorAll<HTMLButtonElement>('button[data-action]')]
        .find(button => button.dataset.action === focusReturn!.action && (focusReturn!.id === undefined || button.dataset.id === focusReturn!.id))
      : undefined;
    (target && !hiddenWithinRoot(target) ? target : focusFallback()).focus();
  };

  const actionButton = (icon: IconName, title: string, action: string, id: string, accessibleName: string, tone = '') => {
    const button = document.createElement('button');
    button.type = 'button'; button.dataset.action = action; button.dataset.id = id; button.title = title;
    button.setAttribute('aria-label', accessibleName);
    button.className = `btn btn-ghost btn-icon ${tone}`;
    button.append(createIcon(icon));
    return button;
  };

  /** Resolves true only when the user confirms in the styled dialog (Escape and Cancel decline). */
  const confirmAction = (message: string, { title = `Delete ${singular}?`, accept = 'Delete' }: ConfirmOptions = {}) => new Promise<boolean>(resolve => {
    query<HTMLElement>('[data-confirm-title]').textContent = title;
    query<HTMLElement>('[data-confirm-message]').textContent = message;
    query<HTMLElement>('[data-confirm-accept]').textContent = accept;
    confirmDialog.returnValue = '';
    confirmDialog.addEventListener('close', () => resolve(confirmDialog.returnValue === 'confirm'), { once: true });
    confirmDialog.showModal();
    query<HTMLButtonElement>('[data-confirm-cancel]').focus();
  });

  const statusTone: Record<string, string> = { Live: 'text-success', Updated: 'text-accent', Connecting: 'text-warning', Reconnecting: 'text-warning' };

  const cellClass = (field: FieldMetadata, index: number) => {
    if (field.name === 'createdAt') return 'whitespace-nowrap tabular-nums text-subtle';
    if (field.type === 'number') return 'text-right tabular-nums text-muted';
    if (field.type === 'date' || field.type === 'datetime-local') return 'whitespace-nowrap tabular-nums text-muted';
    return index === 0 ? 'max-w-xs truncate font-medium text-ink' : 'max-w-56 truncate text-muted';
  };

  /** What a cell shows: resolved text, a link for records with their own page, or a placeholder. */
  const cellContent = (field: FieldMetadata, value: unknown): { text: string; href?: string; state: 'ready' | 'pending' | 'missing' } => {
    if (!field.relation || value === null || value === undefined || value === '') return { text: displayFieldText(field, value, relations), state: 'ready' };
    const id = String(value);
    const record = relations[field.relation]?.find(entity => String(entity.id ?? '') === id);
    if (record) return { text: displayEntityLabel(record), href: linkedRelations.has(field.relation) ? recordHref(field.relation, id) : undefined, state: 'ready' };
    return attempted[field.relation]?.has(id) ? { text: 'Unavailable', state: 'missing' } : { text: '', state: 'pending' };
  };

  const filtersActive = () => Boolean(state.search) || Object.keys(state.filters).length > 0 || state.ids.length > 0;

  const renderSortHeaders = () => {
    const shown = new Set(visibleColumns(resource, hiddenColumns).map(field => field.name));
    for (const button of root.querySelectorAll<HTMLButtonElement>('[data-sort]')) {
      const header = button.closest('th')!;
      const active = button.dataset.sort === state.sort;
      const sort = active ? (state.order === 'asc' ? 'ascending' : 'descending') : 'none';
      header.hidden = !shown.has(button.dataset.sort!);
      if (header.getAttribute('aria-sort') === sort && button.querySelector('svg')) continue;
      header.setAttribute('aria-sort', sort);
      button.querySelector<HTMLElement>('[data-sort-icon]')!.replaceChildren(
        createIcon(active ? (state.order === 'asc' ? 'sortAsc' : 'sortDesc') : 'sort', `size-3.5 ${active ? 'text-accent-text' : 'sort-idle'}`));
    }
    query<HTMLElement>('[data-select-col]').hidden = !canWrite;
  };

  const renderSelection = () => {
    const pageIds = items.map(item => String(item.id));
    const onPage = pageIds.filter(id => selectedIds.has(id)).length;
    selectAll.checked = pageIds.length > 0 && onPage === pageIds.length;
    selectAll.indeterminate = onPage > 0 && onPage < pageIds.length;
    query<HTMLElement>('[data-selection-bar]').hidden = selectedIds.size === 0;
    query<HTMLElement>('[data-selection-count]').textContent = selectedIds.size === 0 ? '' : `${countLabel(selectedIds.size)} selected`;
    for (const box of root.querySelectorAll<HTMLInputElement>('[data-select]')) box.checked = selectedIds.has(box.dataset.select!);
  };

  const renderChips = () => {
    const chips = query<HTMLElement>('[data-filter-chips]');
    const entries: { key: string; label: string }[] = [];
    for (const field of filterFields(resource)) {
      const value = state.filters[field.name];
      if (!value) continue;
      const content = cellContent(field, value);
      entries.push({ key: field.name, label: `${field.label}: ${content.state === 'ready' ? content.text : content.state === 'missing' ? 'Unavailable' : '…'}` });
    }
    if (state.ids.length > 0) entries.push({ key: 'ids', label: `${countLabel(state.ids.length)} linked` });
    const signature = JSON.stringify(entries);
    if (chips.dataset.signature === signature) return;
    chips.dataset.signature = signature;
    chips.replaceChildren(...entries.map(entry => {
      const item = document.createElement('li');
      item.className = 'chip';
      const text = document.createElement('span');
      text.className = 'truncate';
      text.textContent = entry.label;
      const remove = document.createElement('button');
      remove.type = 'button';
      remove.dataset.action = 'remove-filter';
      remove.dataset.filter = entry.key;
      remove.className = 'chip-remove';
      remove.setAttribute('aria-label', `Remove filter ${entry.label}`);
      remove.append(createIcon('close', 'size-3'));
      item.append(createIcon('filter', 'size-3 flex-none text-subtle'), text, remove);
      return item;
    }));
  };

  const render = () => {
    const lastPage = getLastPage(total, state.pageSize);
    const summary = `${total} ${total === 1 ? 'record' : 'records'} · page ${state.page} of ${lastPage} · ${status}`;
    if (summary !== renderedSummary) {
      renderedSummary = summary;
      query<HTMLElement>('[data-summary]').textContent = summary;
      query<HTMLElement>('[data-live-dot]').className = `status-dot ${statusTone[status] ?? 'text-subtle'}`;
    }
    renderSortHeaders();
    renderChips();
    const columns = visibleColumns(resource, hiddenColumns);
    const body = query<HTMLTableSectionElement>('[data-records]');
    const rowSignature = JSON.stringify([canWrite, [...highlightIds], columns.map(field => field.name),
      items.map(item => [item.id, ...columns.map(field => cellContent(field, item[field.name]))])]);
    if (rowSignature !== renderedRows) {
      renderedRows = rowSignature;
      const active = document.activeElement as HTMLElement | null;
      const focusedRowAction: FocusReturn | null = active && body.contains(active) && active.dataset.action
        ? { action: active.dataset.action, id: active.dataset.id }
        : null;
      body.replaceChildren(...items.map(item => {
        const row = document.createElement('tr');
        const id = String(item.id);
        const label = recordLabel(resource, item, relations);
        if (highlightIds.has(id)) row.className = 'row-highlight';
        if (canWrite) {
          const cell = row.insertCell(); cell.className = 'col-select';
          const box = document.createElement('input');
          box.type = 'checkbox'; box.className = 'checkbox'; box.dataset.select = id; box.checked = selectedIds.has(id);
          box.setAttribute('aria-label', `Select ${label}`);
          cell.append(box);
        }
        columns.forEach((field, index) => {
          const cell = row.insertCell();
          const content = cellContent(field, item[field.name]);
          cell.className = cellClass(field, index);
          cell.dataset.label = field.label;
          if (content.state === 'pending') {
            const placeholder = document.createElement('span');
            placeholder.className = 'skeleton inline-block h-3.5 w-24 align-middle';
            placeholder.setAttribute('aria-hidden', 'true');
            const loading = document.createElement('span');
            loading.className = 'sr-only'; loading.textContent = 'Loading';
            cell.append(placeholder, loading);
          } else if (content.href) {
            const link = document.createElement('a');
            link.href = content.href; link.className = 'table-link'; link.textContent = content.text;
            cell.append(link);
          } else {
            cell.textContent = content.text;
            if (content.state === 'missing') { cell.title = `Deleted or not visible to you (${String(item[field.name])})`; cell.classList.add('italic'); }
          }
          if (content.text.length > 32) cell.title = content.text;
        });
        const actions = row.insertCell(); actions.className = 'col-actions whitespace-nowrap py-1.5 text-right';
        const group = document.createElement('div'); group.className = 'inline-flex items-center gap-0.5';
        group.append(actionButton('eye', 'Details', 'details', id, `Details for ${label}`));
        if (canWrite) group.append(actionButton('pencil', 'Edit', 'edit', id, `Edit ${label}`), actionButton('trash', 'Delete', 'delete', id, `Delete ${label}`, 'btn-danger'));
        actions.append(group);
        return row;
      }));
      // Live snapshots replace the rows; keep keyboard focus on the equivalent row button, or on a
      // stable control when that row is gone (for example after deleting it).
      if (focusedRowAction) {
        focusReturn = focusedRowAction;
        restoreFocusAfterRowsChange = false;
        restoreFocus();
      } else if (restoreFocusAfterRowsChange) {
        restoreFocusAfterRowsChange = false;
        if (document.activeElement === document.body || document.activeElement === null) restoreFocus();
      }
    }
    renderSelection();
    const filtered = filtersActive();
    query<HTMLElement>('[data-empty-state]').hidden = items.length > 0;
    query<HTMLElement>('[data-empty]').textContent = !loaded
      ? 'Loading records…'
      : filtered ? `No ${plural} match the current search and filters.` : `No ${plural} yet.${canWrite ? ` Use “New ${singular}” to add the first one.` : ''}`;
    query<HTMLElement>('[data-action="reset-view"]').hidden = !filtered;
    query<HTMLTableElement>('[data-table]').hidden = items.length === 0;
    const nav = query<HTMLElement>('[data-pagination]');
    nav.dataset.currentPage = String(state.page);
    const pageSignature = `${state.page}:${lastPage}`;
    if (pageSignature === renderedPages) return;
    renderedPages = pageSignature;
    nav.replaceChildren();
    const addPage = (label: string, target: number, disabled = false) => {
      const button = document.createElement('button');
      button.type = 'button'; button.dataset.action = 'page'; button.dataset.page = String(target); button.disabled = disabled;
      button.className = 'page-button';
      const step = label === 'Previous' || label === 'Next';
      if (step) button.append(createIcon(label === 'Previous' ? 'chevronLeft' : 'chevronRight'));
      else button.textContent = label;
      button.setAttribute('aria-label', step ? `${label} Page` : `Page ${label}`);
      button.setAttribute('aria-disabled', String(disabled));
      if (Number(label) === state.page) button.setAttribute('aria-current', 'page');
      nav.append(button);
    };
    addPage('Previous', state.page - 1, state.page === 1);
    for (const value of buildPaginationItems(state.page, lastPage)) {
      if (typeof value === 'number') addPage(String(value), value);
      else { const dots = document.createElement('span'); dots.textContent = '…'; dots.className = 'px-1 text-subtle'; dots.setAttribute('aria-hidden', 'true'); nav.append(dots); }
    }
    addPage('Next', state.page + 1, state.page === lastPage);
  };

  const relationKeys = () => [...new Set(fields.map(field => field.relation).filter((key): key is ResourceKey => Boolean(key)))];

  const renderOptions = (key: ResourceKey) => {
    for (const field of fields.filter(candidate => candidate.relation === key)) {
      const control = fieldControl(field.name) as HTMLSelectElement;
      // The current control value is authoritative: it holds the prefilled value until the user
      // changes it, and an explicit clear ("Select …") must stay cleared after later searches.
      const selectedId = control.value;
      const choices = withSelectedRelationOption(options[key] ?? [], selectedId, relations[key] ?? []);
      control.replaceChildren(new Option(`Select ${field.label.toLowerCase()}`, ''),
        ...choices.map(item => new Option(displayEntityLabel(item), String(item.id))));
      control.value = selectedId;
    }
  };

  const loadMissingLabels = async (signal: AbortSignal) => {
    const pending = [...items, { ...state.filters }];
    const missing = collectMissingRelationIds(pending, resource.fields, relations);
    if (restrictUserPicker) delete missing.users;
    await Promise.all(Object.entries(missing).map(async ([key, ids]) => {
      const found = await webApiClient.lookup(key as ResourceKey, ids ?? [], signal).catch(() => null);
      if (signal.aborted || found === null) return;
      relations[key as ResourceKey] = mergeRelationRecords(relations[key as ResourceKey] ?? [], found);
      const tried = attempted[key as ResourceKey] ??= new Set();
      (ids ?? []).forEach(id => tried.add(id));
    }));
    if (signal.aborted) return;
    render();
    if (formDialog.open) relationKeys().forEach(renderOptions);
  };

  const handleIntent = async () => {
    if (intentHandled) return;
    intentHandled = true;
    if (intent.create && canWrite) openForm(null);
    else if (intent.editId && canWrite) {
      const item = items.find(candidate => candidate.id === intent.editId)
        ?? await webApiClient.get<ApiEntity>(resource.key, intent.editId, lifetime.signal).catch(() => null);
      if (lifetime.signal.aborted) return;
      if (item?.id) openForm(item);
      else fail(new Error(`That ${singular} was not found. It may have been deleted, or you may not have access to it.`));
    }
    if (intent.create || intent.editId) syncUrl();
  };

  const refresh = () => {
    listing.abort(); listing = new AbortController();
    const signal = listing.signal;
    const requestedPage = state.page;
    const requestedSize = state.pageSize;
    alert.hidden = true;
    void watchListing(onLiveSnapshot => webApiClient.subscribeList(resource.key, requestedPage, requestedSize, (result, info) => {
      if (signal.aborted) return;
      if (info.live) onLiveSnapshot();
      total = result.totalCount ?? 0;
      const lastPage = getLastPage(total, state.pageSize);
      if (state.page > lastPage) { state.page = lastPage; syncUrl(); refresh(); return; }
      items = result.items ?? []; status = info.live ? 'Live' : 'Updated'; loaded = true;
      const present = new Set(items.map(item => String(item.id)));
      for (const id of selectedIds) if (!present.has(id)) selectedIds.delete(id);
      render();
      void loadMissingLabels(signal);
      void handleIntent();
    }, signal, {
      search: state.search,
      ids: state.ids,
      sort: { column: toSortColumn(state.sort), order: state.order === 'asc' ? 'ASC' : 'DESC' },
      filters: state.filters,
    }), signal, value => { status = value; render(); }).catch(error => { if (!signal.aborted) fail(error); });
  };

  /** Applies a change to what the list shows: back to page 1, selection cleared, URL updated. */
  const changeView = (change: () => void) => {
    change();
    state.page = 1;
    selectedIds.clear();
    syncUrl();
    renderedPages = '';
    refresh();
  };

  const abortRelationSearches = () => {
    for (const search of Object.values(relationSearches)) search?.controller.abort();
    for (const timer of relationTimers.values()) clearTimeout(timer);
    relationTimers.clear();
  };

  /** Pickers list options alphabetically when the related record has a name or description. */
  const optionSort = (key: ResourceKey) => {
    const field = findResource(key).displayField;
    return field === 'id' ? undefined : { column: toSortColumn(field), order: 'ASC' as const };
  };

  const loadOptions = async (container: HTMLElement, mode: 'search' | 'more' = 'search') => {
    const key = container.dataset.relation as ResourceKey;
    const searchRow = container.querySelector<HTMLElement>('[data-relation-search]')!;
    const statusElement = container.querySelector<HTMLElement>('[data-relation-status]')!;
    const errorElement = container.querySelector<HTMLElement>('[data-relation-error]')!;
    const more = container.querySelector<HTMLButtonElement>('[data-action="more"]')!;
    errorElement.textContent = '';

    if (key === 'users' && restrictUserPicker) {
      options.users = currentUser ? [currentUser] : [];
      more.hidden = true;
      searchRow.hidden = true;
      statusElement.textContent = '';
      renderOptions(key);
      return;
    }

    const previous = relationSearches[key];
    const continuing = mode === 'more' && previous !== undefined;
    const search: RelationSearch = {
      controller: new AbortController(),
      query: continuing ? previous.query : container.querySelector<HTMLInputElement>('[data-query]')!.value.trim(),
      page: continuing ? previous.page : 0,
    };
    previous?.controller.abort();
    relationSearches[key] = search;
    const nextPage = search.page + 1;
    statusElement.textContent = 'Loading options…';
    try {
      const result = await webApiClient.list(key, nextPage, RELATION_PAGE_SIZE, search.controller.signal, search.query, { sort: optionSort(key) });
      if (search.controller.signal.aborted || lifetime.signal.aborted || relationSearches[key] !== search) return;
      search.page = nextPage;
      const loaded = result.items ?? [];
      options[key] = nextPage === 1 ? loaded : mergeRelationRecords(options[key] ?? [], loaded);
      relations[key] = mergeRelationRecords(relations[key] ?? [], loaded);
      const totalOptions = result.totalCount ?? options[key]!.length;
      const partial = nextPage * RELATION_PAGE_SIZE < totalOptions;
      more.hidden = !partial;
      // The filter only appears when the full list is longer than one page, or a filter is active.
      searchRow.hidden = totalOptions <= RELATION_PAGE_SIZE && !search.query;
      statusElement.textContent = search.query
        ? `${options[key]!.length} of ${totalOptions} options match “${search.query}”.`
        : partial ? `${options[key]!.length} of ${totalOptions} options loaded. Filter to find others.` : '';
      renderOptions(key); render();
    } catch (error) {
      if (search.controller.signal.aborted || lifetime.signal.aborted || relationSearches[key] !== search) return;
      statusElement.textContent = '';
      errorElement.textContent = messageOf(error, 'Unable to load options.');
    }
  };

  const configureRelationPickers = () => {
    for (const container of root.querySelectorAll<HTMLElement>('[data-relation]')) {
      const restricted = container.dataset.relation === 'users' && restrictUserPicker;
      const note = container.querySelector<HTMLElement>('[data-relation-note]')!;
      note.hidden = !restricted;
      note.textContent = restricted ? 'Only your own account is listed: the team member directory requires administrator access.' : '';
      const input = container.querySelector<HTMLInputElement>('[data-query]')!;
      input.addEventListener('input', () => {
        clearTimeout(relationTimers.get(container));
        relationTimers.set(container, setTimeout(() => void loadOptions(container), SEARCH_DEBOUNCE_MS));
      }, { signal: lifetime.signal });
      // Enter filters instead of submitting the surrounding form.
      input.addEventListener('keydown', event => {
        if (event.key !== 'Enter') return;
        event.preventDefault();
        clearTimeout(relationTimers.get(container));
        void loadOptions(container);
      }, { signal: lifetime.signal });
    }
  };

  const defaultValue = (field: FieldMetadata, item: ApiEntity | null) => {
    if (item) return formatFormFieldValue(item[field.name], field.type);
    // A filtered list ("tasks of this project") prefills the same relation for new records.
    const filtered = state.filters[field.name as keyof typeof state.filters];
    if (filtered) return filtered;
    // New calendar items belong to the signed-in user; non-admins can only pick themselves.
    if (field.relation === 'users' && currentUser && (resource.key === 'appointments' || restrictUserPicker)) return String(currentUser.id);
    return '';
  };

  const formSnapshot = () => JSON.stringify(fields.map(field => fieldControl(field.name).value));
  const formIsDirty = () => formDialog.open && formSnapshot() !== initialFormValues;

  const fillForm = (item: ApiEntity | null) => {
    form.reset(); clearFieldErrors();
    for (const field of fields) {
      const control = fieldControl(field.name);
      control.required = Boolean(field.required || (!item && field.requiredOnCreate));
      const value = defaultValue(field, item);
      if (field.relation) {
        const select = control as HTMLSelectElement;
        const choices = withSelectedRelationOption(options[field.relation] ?? [], value, relations[field.relation] ?? []);
        select.replaceChildren(new Option(`Select ${field.label.toLowerCase()}`, ''), ...choices.map(choice => new Option(displayEntityLabel(choice), String(choice.id))));
      }
      control.value = value;
    }
    initialFormValues = formSnapshot();
  };

  const openForm = (item: ApiEntity | null, trigger?: HTMLButtonElement) => {
    abortRelationSearches();
    selected = item; alert.hidden = true;
    focusReturn = trigger ? { action: trigger.dataset.action, id: trigger.dataset.id } : null;
    restoreFocusAfterRowsChange = false;
    query<HTMLElement>('[data-form-title]').textContent = item ? `Edit ${recordLabel(resource, item, relations)}` : `New ${singular}`;
    query<HTMLElement>('[data-submit="another"]').hidden = Boolean(item);
    fillForm(item);
    if (!formDialog.open) formDialog.showModal();
    for (const container of root.querySelectorAll<HTMLElement>('[data-relation]')) {
      container.querySelector<HTMLInputElement>('[data-query]')!.value = '';
      void loadOptions(container);
    }
    form.querySelector<HTMLElement>('input:not([type="search"]), select, textarea')?.focus();
  };

  const closeForm = () => {
    abortRelationSearches();
    if (formDialog.open) formDialog.close();
    selected = null; clearFieldErrors();
    restoreFocus();
  };

  /** Closes the form, asking first when it holds unsaved changes. */
  const requestCloseForm = async () => {
    if (formIsDirty() && !await confirmAction('Your changes to this form will be lost.', { title: 'Discard unsaved changes?', accept: 'Discard' })) {
      form.querySelector<HTMLElement>('input:not([type="search"]), select, textarea')?.focus();
      return;
    }
    closeForm();
  };

  const validateDateRange = (data: FormData): boolean => {
    if (!resource.dateRange) return true;
    const { start, end } = resource.dateRange;
    const startValue = String(data.get(start) ?? '');
    const endValue = String(data.get(end) ?? '');
    if (!startValue || !endValue || endValue >= startValue) return true;
    const startLabel = fields.find(field => field.name === start)?.label.toLowerCase() ?? start;
    const endLabel = fields.find(field => field.name === end)?.label ?? end;
    const message = `${endLabel} must be on or after the ${startLabel}.`;
    markFieldErrors({ [end]: [message] });
    failForm(new Error(message));
    return false;
  };

  const highlight = (id: string) => {
    highlightIds.add(id);
    setTimeout(() => { highlightIds.delete(id); if (!lifetime.signal.aborted) render(); }, HIGHLIGHT_MS);
  };

  const restore = async (ids: string[], label: string) => {
    try {
      await webApiClient.restore(resource.key, ids);
      if (lifetime.signal.aborted) return;
      ids.forEach(highlight);
      showToast(`${label} restored.`);
      refresh();
    } catch (error) {
      if (!lifetime.signal.aborted) showToast(`Could not restore: ${messageOf(error)}`, { tone: 'danger' });
    }
  };

  const removeRecords = async (ids: string[], label: string) => {
    alert.hidden = true;
    await webApiClient.delete(resource.key, ids);
    if (lifetime.signal.aborted) return;
    ids.forEach(id => selectedIds.delete(id));
    showToast(`${label} deleted.`, { action: { label: 'Undo', run: () => restore(ids, label) } });
    refresh();
  };

  form.addEventListener('submit', async event => {
    event.preventDefault();
    clearFieldErrors();
    alert.hidden = true;
    if (!form.reportValidity()) return;
    const submitter = (event as SubmitEvent).submitter as HTMLButtonElement | null;
    const addAnother = submitter?.dataset.submit === 'another' && !selected;
    const buttons = [...form.querySelectorAll<HTMLButtonElement>('[type="submit"]')];
    if (buttons.some(button => button.disabled)) return;
    const data = new FormData(form);
    if (!validateDateRange(data)) return;
    const pressed = submitter ?? buttons.at(-1)!;
    const pressedText = pressed.textContent;
    buttons.forEach(button => { button.disabled = true; });
    pressed.textContent = 'Saving…';
    const payload: Record<string, unknown> = {};
    for (const field of fields) {
      const value = String(data.get(field.name) ?? '');
      if (value === '' && ['guid', 'number', 'date', 'datetime-local', 'password'].includes(field.type)) continue;
      payload[field.name] = serializeFormValue(value, field.type);
    }
    const editing = selected;
    try {
      if (resource.key === 'projects' && editing) payload.expectedVersion = editing.updatedAt ?? editing.createdAt;
      let savedId: string;
      if (editing?.id) {
        await webApiClient.update(resource.key, editing.id, payload);
        savedId = editing.id;
      } else {
        savedId = crypto.randomUUID();
        await webApiClient.create(resource.key, { id: savedId, ...payload });
      }
      if (lifetime.signal.aborted) return;
      highlight(savedId);
      const name = String(payload[resource.displayField] ?? '').trim();
      const link = !editing && linkedRelations.has(resource.key) ? { label: 'Open', href: recordHref(resource.key, savedId) } : undefined;
      showToast(editing ? `Changes to ${name ? `“${name}”` : `the ${singular}`} saved.` : `${resource.label} ${name ? `“${name}” ` : ''}created.`, { link });
      // New records sort first by default: show page 1 so the new row is visible.
      if (!editing && state.sort === DEFAULT_SORT_FIELD && state.order === 'desc' && state.page !== 1) { state.page = 1; syncUrl(); }
      if (addAnother) {
        fillForm(null);
        form.querySelector<HTMLElement>('input:not([type="search"]), select, textarea')?.focus();
      } else closeForm();
      refresh();
    } catch (error) {
      if (lifetime.signal.aborted) return;
      if (resource.key === 'projects' && editing?.id && error instanceof ApiError && error.status === 409) {
        // Keep the user's edits but adopt the stored version, so the conflict is visible once and an
        // intentional second save replaces the other change instead of failing forever.
        try {
          const latest = await webApiClient.get<ApiEntity>(resource.key, String(editing.id), lifetime.signal);
          selected = { ...editing, updatedAt: latest.updatedAt, createdAt: latest.createdAt };
          failForm(new ApiError(409, `${error.message} The latest version was loaded: your edits are kept, and saving again replaces the other change.`));
          return;
        } catch { /* fall back to the original conflict message */ }
      }
      failForm(error);
    }
    finally { buttons.forEach(button => { button.disabled = false; }); pressed.textContent = pressedText; }
  }, { signal: lifetime.signal });

  const showDetails = (item: ApiEntity) => {
    const detailFields = query<HTMLElement>('[data-detail-fields]'); detailFields.replaceChildren();
    for (const field of resource.fields.filter(field => field.type !== 'password')) {
      const row = document.createElement('div'); row.className = 'grid gap-1 py-3 text-sm sm:grid-cols-3 sm:gap-4';
      const term = document.createElement('dt'); term.className = 'text-subtle'; term.textContent = field.label;
      const value = document.createElement('dd');
      value.className = field.type === 'guid' && !field.relation ? 'break-all font-mono text-[0.8125rem] text-muted sm:col-span-2' : 'break-words sm:col-span-2';
      const content = cellContent(field, item[field.name]);
      if (content.href) {
        const link = document.createElement('a'); link.href = content.href; link.className = 'table-link'; link.textContent = content.text;
        value.append(link);
      } else value.textContent = content.state === 'pending' ? displayFieldText(field, item[field.name], relations) : content.text;
      row.append(term, value); detailFields.append(row);
    }
    const actions = query<HTMLElement>('[data-detail-actions]');
    actions.replaceChildren();
    if (linkedRelations.has(resource.key)) {
      const open = document.createElement('a');
      open.href = recordHref(resource.key, String(item.id)); open.className = 'btn btn-secondary'; open.textContent = `Open ${singular} page`;
      actions.append(open);
    }
    if (canWrite) {
      const edit = document.createElement('button');
      edit.type = 'button'; edit.dataset.action = 'edit-from-details'; edit.dataset.id = String(item.id); edit.className = 'btn btn-primary';
      edit.append(createIcon('pencil'), document.createTextNode('Edit'));
      actions.append(edit);
    }
    actions.hidden = actions.childElementCount === 0;
    dialog.showModal();
  };

  root.addEventListener('click', async event => {
    const target = (event.target as Element).closest<HTMLButtonElement>('button[data-action]');
    if (!target || target.disabled) return;
    const item = items.find(item => item.id === target.dataset.id);
    switch (target.dataset.action) {
      case 'create': if (canWrite) openForm(null, target); break;
      case 'edit': if (item && canWrite) openForm(item, target); break;
      case 'edit-from-details': if (item && canWrite) { dialog.close(); openForm(item); focusReturn = { action: 'edit', id: target.dataset.id }; } break;
      case 'cancel': await requestCloseForm(); break;
      case 'refresh': refresh(); break;
      case 'page': state.page = Number(target.dataset.page); selectedIds.clear(); syncUrl(); refresh(); break;
      case 'more': {
        const container = target.closest<HTMLElement>('[data-relation]')!;
        await loadOptions(container, 'more');
        break;
      }
      case 'remove-filter': {
        const key = target.dataset.filter!;
        changeView(() => { if (key === 'ids') state.ids = []; else delete state.filters[key as keyof typeof state.filters]; });
        searchInput.focus();
        break;
      }
      case 'reset-view':
        changeView(() => { state.search = ''; state.filters = {}; state.ids = []; searchInput.value = ''; });
        searchInput.focus();
        break;
      case 'clear-selection': selectedIds.clear(); renderSelection(); selectAll.focus(); break;
      case 'delete-selected': {
        const ids = [...selectedIds];
        if (ids.length === 0 || !canWrite) break;
        const confirmed = await confirmAction(`${countLabel(ids.length)} will be removed together. They stay in the database as soft-deleted records, and you can undo right after.`, { title: `Delete ${countLabel(ids.length)}?` });
        if (!confirmed) { target.focus(); break; }
        target.disabled = true;
        try {
          await removeRecords(ids, countLabel(ids.length));
          restoreFocusAfterRowsChange = true;
          focusReturn = null;
        } catch (error) { if (!lifetime.signal.aborted) fail(error); }
        finally { target.disabled = false; }
        break;
      }
      case 'details': if (item) showDetails(item); break;
      case 'close-details': dialog.close(); break;
      case 'delete': {
        if (!item?.id || !canWrite) break;
        const label = recordLabel(resource, item, relations);
        const confirmed = await confirmAction(`“${label}” will be removed from ${plural}. It stays in the database as a soft-deleted record, and you can undo right after.`);
        // Not every browser returns focus to the trigger when a modal dialog closes, and a live
        // snapshot may have replaced the row meanwhile: focus its current Delete button, or the
        // stable fallback when the row is gone.
        focusReturn = { action: 'delete', id: target.dataset.id };
        restoreFocus();
        if (!confirmed) break;
        target.disabled = true;
        try {
          await removeRecords([item.id], `“${label}”`);
          // The row disappears on the next snapshot; move focus to a stable control.
          focusReturn = null;
          restoreFocusAfterRowsChange = true;
        } catch (error) { if (!lifetime.signal.aborted) fail(error); }
        finally { target.disabled = false; }
      }
    }
  }, { signal: lifetime.signal });

  root.addEventListener('change', event => {
    const target = event.target as HTMLInputElement;
    if (target.matches('[data-select]')) {
      if (target.checked) selectedIds.add(target.dataset.select!); else selectedIds.delete(target.dataset.select!);
      renderSelection();
    } else if (target === selectAll) {
      for (const item of items) if (selectAll.checked) selectedIds.add(String(item.id)); else selectedIds.delete(String(item.id));
      renderSelection();
    } else if (target.matches('[data-column-toggle]')) {
      const name = target.dataset.columnToggle!;
      if (target.checked) hiddenColumns.delete(name); else hiddenColumns.add(name);
      writeHiddenColumns(resource, hiddenColumns);
      render();
    }
  }, { signal: lifetime.signal });

  for (const button of root.querySelectorAll<HTMLButtonElement>('[data-sort]')) {
    button.addEventListener('click', () => {
      const field = button.dataset.sort!;
      changeView(() => {
        state.order = state.sort === field ? (state.order === 'asc' ? 'desc' : 'asc') : defaultOrderFor(field);
        state.sort = field;
      });
    }, { signal: lifetime.signal });
  }

  searchInput.value = state.search;
  searchInput.addEventListener('input', () => {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(() => {
      const value = searchInput.value.trim();
      if (value !== state.search) changeView(() => { state.search = value; });
    }, SEARCH_DEBOUNCE_MS);
  }, { signal: lifetime.signal });
  // "/" jumps to the list search, unless the person is typing or a dialog is open.
  document.addEventListener('keydown', event => {
    if (event.key !== '/' || event.ctrlKey || event.metaKey || event.altKey) return;
    const active = document.activeElement as HTMLElement | null;
    if (active?.closest('input, textarea, select, [contenteditable="true"]') || document.querySelector('dialog[open]')) return;
    event.preventDefault();
    searchInput.focus();
  }, { signal: lifetime.signal });

  for (const box of root.querySelectorAll<HTMLInputElement>('[data-column-toggle]')) box.checked = !hiddenColumns.has(box.dataset.columnToggle!) || box.disabled;

  // A click on the backdrop (the dialog element itself, outside its content) dismisses it; the
  // form asks first when it holds unsaved changes.
  for (const modal of [dialog, confirmDialog]) modal.addEventListener('click', event => { if (event.target === modal) modal.close(); }, { signal: lifetime.signal });
  formDialog.addEventListener('click', event => { if (event.target === formDialog) void requestCloseForm(); }, { signal: lifetime.signal });
  formDialog.addEventListener('cancel', event => { event.preventDefault(); void requestCloseForm(); }, { signal: lifetime.signal });
  query<HTMLButtonElement>('[data-confirm-accept]').addEventListener('click', () => confirmDialog.close('confirm'), { signal: lifetime.signal });
  query<HTMLButtonElement>('[data-confirm-cancel]').addEventListener('click', () => confirmDialog.close('cancel'), { signal: lifetime.signal });
  const pageSize = query<HTMLSelectElement>('[data-page-size]');
  pageSize.value = String(state.pageSize);
  pageSize.addEventListener('change', event => {
    changeView(() => { state.pageSize = Number((event.target as HTMLSelectElement).value); });
  }, { signal: lifetime.signal });
  window.addEventListener('beforeunload', event => { if (formIsDirty()) event.preventDefault(); }, { signal: lifetime.signal });

  configureRelationPickers();
  if (!canWrite) {
    createButton.hidden = true;
    permissionNote.textContent = `Only administrators can create, edit or delete ${plural}. You can still browse them.`;
    permissionNote.hidden = false;
  }
  if (!canRead) {
    permissionNote.textContent = `${resource.pluralLabel} are managed by administrators. Your account does not have administrator access, so this list is not available.`;
    permissionNote.hidden = false;
    query<HTMLElement>('[data-listing]').hidden = true;
    return () => { lifetime.abort(); listing.abort(); };
  }
  render();
  refresh();
  return () => { lifetime.abort(); listing.abort(); abortRelationSearches(); clearTimeout(searchTimer); };
};
