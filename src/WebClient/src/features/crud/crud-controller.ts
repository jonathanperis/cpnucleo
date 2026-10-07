import { findResource, formFields, tableFields } from '~/lib/api/resource-metadata';
import { webApiClient } from '~/lib/api/webapi-client';
import { ApiError, getSessionClaims, type FieldErrors, type SessionClaims } from '~/lib/api/http-client';
import type { ApiEntity, FieldMetadata, ResourceKey } from '~/lib/api/types';
import { buildPaginationItems, DEFAULT_PAGE_SIZE, getLastPage } from './pagination';
import { formatFormFieldValue, serializeFormValue, withSelectedRelationOption } from './crud-field-values';
import { collectMissingRelationIds, displayEntityLabel, displayFieldText, mergeRelationRecords, recordLabel } from './relation-display';
import { watchListing } from './watch-list';
import { createIcon, type IconName } from '~/lib/icons';

const RELATION_PAGE_SIZE = 100;

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

export const mountCrudPage = (root: HTMLElement, session: SessionClaims | null = getSessionClaims()): (() => void) => {
  const resource = findResource(root.dataset.crud as ResourceKey);
  const query = <T extends Element>(selector: string) => root.querySelector<T>(selector)!;
  const form = query<HTMLFormElement>('[data-form]');
  const alert = query<HTMLElement>('[data-error]');
  const dialog = query<HTMLDialogElement>('[data-details]');
  const confirmDialog = query<HTMLDialogElement>('[data-confirm]');
  const createButton = query<HTMLButtonElement>('[data-action="create"]');
  const heading = query<HTMLElement>('[data-heading]');
  const permissionNote = query<HTMLElement>('[data-permission-note]');
  const lifetime = new AbortController();
  const relations: Partial<Record<ResourceKey, ApiEntity[]>> = {};
  const options: Partial<Record<ResourceKey, ApiEntity[]>> = {};
  const relationSearches: Partial<Record<ResourceKey, RelationSearch>> = {};
  let listing = new AbortController();
  let items: ApiEntity[] = [];
  let selected: ApiEntity | null = null;
  let page = 1;
  let pageSize = DEFAULT_PAGE_SIZE;
  let total = 0;
  let status = 'Connecting';
  let renderedRows = '';
  let renderedPages = '';
  let renderedSummary = '';
  let focusReturn: FocusReturn | null = null;
  let restoreFocusAfterRowsChange = false;

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
  const fieldControl = (name: string) => form.elements.namedItem(name) as HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement;
  const fieldError = (name: string) => root.querySelector<HTMLElement>(`[data-field-error="${name}"]`);

  const fail = (error: unknown) => {
    alert.textContent = error instanceof Error ? error.message : 'Unable to complete the request.';
    alert.hidden = false;
  };

  const setDescribedBy = (control: Element, id: string, present: boolean) => {
    const ids = new Set((control.getAttribute('aria-describedby') ?? '').split(/\s+/).filter(Boolean));
    if (present) ids.add(id); else ids.delete(id);
    if (ids.size > 0) control.setAttribute('aria-describedby', [...ids].join(' '));
    else control.removeAttribute('aria-describedby');
  };

  const clearFieldErrors = () => {
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
    fail(error);
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
  const confirmAction = (message: string) => new Promise<boolean>(resolve => {
    query<HTMLElement>('[data-confirm-message]').textContent = message;
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
    return index === 0 ? 'max-w-xs truncate font-medium text-ink' : 'max-w-xs truncate text-muted';
  };

  const render = () => {
    const summary = `${total} records · page ${page} of ${getLastPage(total, pageSize)} · ${status}`;
    if (summary !== renderedSummary) {
      renderedSummary = summary;
      query<HTMLElement>('[data-summary]').textContent = summary;
      query<HTMLElement>('[data-live-dot]').className = `status-dot ${statusTone[status] ?? 'text-subtle'}`;
    }
    const body = query<HTMLTableSectionElement>('[data-records]');
    const rowSignature = JSON.stringify([canWrite, items.map(item => [item.id, ...tableFields(resource).map(field => displayFieldText(field, item[field.name], relations))])]);
    if (rowSignature !== renderedRows) {
      renderedRows = rowSignature;
      const active = document.activeElement as HTMLElement | null;
      const focusedRowAction: FocusReturn | null = active && body.contains(active) && active.dataset.action
        ? { action: active.dataset.action, id: active.dataset.id }
        : null;
      body.replaceChildren(...items.map(item => {
        const row = document.createElement('tr');
        tableFields(resource).forEach((field, index) => {
          const cell = row.insertCell();
          const text = displayFieldText(field, item[field.name], relations);
          cell.className = cellClass(field, index);
          cell.textContent = text;
          if (text.length > 32) cell.title = text;
        });
        const label = recordLabel(resource, item, relations);
        const id = String(item.id);
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
    query<HTMLElement>('[data-empty-state]').hidden = items.length > 0;
    query<HTMLElement>('[data-empty]').textContent = `No ${resource.pluralLabel.toLowerCase()} on this page.`;
    query<HTMLTableElement>('[data-table]').hidden = items.length === 0;
    const nav = query<HTMLElement>('[data-pagination]');
    nav.dataset.currentPage = String(page);
    const lastPage = getLastPage(total, pageSize);
    const pageSignature = `${page}:${lastPage}`;
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
      if (Number(label) === page) button.setAttribute('aria-current', 'page');
      nav.append(button);
    };
    addPage('Previous', page - 1, page === 1);
    for (const value of buildPaginationItems(page, lastPage)) {
      if (typeof value === 'number') addPage(String(value), value);
      else { const dots = document.createElement('span'); dots.textContent = '…'; dots.className = 'px-1 text-subtle'; dots.setAttribute('aria-hidden', 'true'); nav.append(dots); }
    }
    addPage('Next', page + 1, page === lastPage);
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
    const missing = collectMissingRelationIds(items, resource.fields, relations);
    if (restrictUserPicker) delete missing.users;
    await Promise.all(Object.entries(missing).map(async ([key, ids]) => {
      const found = await webApiClient.lookup(key as ResourceKey, ids ?? [], signal).catch(() => []);
      if (signal.aborted) return;
      relations[key as ResourceKey] = mergeRelationRecords(relations[key as ResourceKey] ?? [], found);
    }));
    if (signal.aborted) return;
    render();
    if (!form.hidden) relationKeys().forEach(renderOptions);
  };

  const refresh = () => {
    listing.abort(); listing = new AbortController();
    const signal = listing.signal;
    const requestedPage = page;
    const requestedSize = pageSize;
    alert.hidden = true;
    void watchListing(onLiveSnapshot => webApiClient.subscribeList(resource.key, requestedPage, requestedSize, (result, info) => {
      if (signal.aborted) return;
      if (info.live) onLiveSnapshot();
      total = result.totalCount ?? 0;
      const lastPage = getLastPage(total, pageSize);
      if (page > lastPage) { page = lastPage; refresh(); return; }
      items = result.items ?? []; status = info.live ? 'Live' : 'Updated'; render();
      void loadMissingLabels(signal);
    }, signal), signal, value => { status = value; render(); }).catch(error => { if (!signal.aborted) fail(error); });
  };

  const abortRelationSearches = () => {
    for (const search of Object.values(relationSearches)) search?.controller.abort();
  };

  const loadOptions = async (container: HTMLElement, mode: 'search' | 'more' = 'search') => {
    const key = container.dataset.relation as ResourceKey;
    const statusElement = container.querySelector<HTMLElement>('[data-relation-status]')!;
    const errorElement = container.querySelector<HTMLElement>('[data-relation-error]')!;
    const more = container.querySelector<HTMLButtonElement>('[data-action="more"]')!;
    errorElement.textContent = '';

    if (key === 'users' && restrictUserPicker) {
      options.users = currentUser ? [currentUser] : [];
      more.hidden = true;
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
      const result = await webApiClient.list(key, nextPage, RELATION_PAGE_SIZE, search.controller.signal, search.query);
      if (search.controller.signal.aborted || lifetime.signal.aborted || relationSearches[key] !== search) return;
      search.page = nextPage;
      const loaded = result.items ?? [];
      options[key] = nextPage === 1 ? loaded : mergeRelationRecords(options[key] ?? [], loaded);
      relations[key] = mergeRelationRecords(relations[key] ?? [], loaded);
      const totalOptions = result.totalCount ?? options[key]!.length;
      more.hidden = nextPage * RELATION_PAGE_SIZE >= totalOptions;
      statusElement.textContent = `${options[key]!.length} of ${totalOptions} options loaded${search.query ? ` for “${search.query}”` : ''}.`;
      renderOptions(key); render();
    } catch (error) {
      if (search.controller.signal.aborted || lifetime.signal.aborted || relationSearches[key] !== search) return;
      statusElement.textContent = '';
      errorElement.textContent = error instanceof Error ? error.message : 'Unable to load options.';
    }
  };

  const configureRelationPickers = () => {
    for (const container of root.querySelectorAll<HTMLElement>('[data-relation]')) {
      const restricted = container.dataset.relation === 'users' && restrictUserPicker;
      container.querySelector<HTMLElement>('[data-relation-search]')!.hidden = restricted;
      const note = container.querySelector<HTMLElement>('[data-relation-note]')!;
      note.hidden = !restricted;
      note.textContent = restricted ? 'Only your own account is listed: the team member directory requires administrator access.' : '';
    }
  };

  const defaultValue = (field: FieldMetadata, item: ApiEntity | null) => {
    if (item) return formatFormFieldValue(item[field.name], field.type);
    // New calendar items belong to the signed-in user; non-admins can only pick themselves.
    if (field.relation === 'users' && currentUser && (resource.key === 'appointments' || restrictUserPicker)) return String(currentUser.id);
    return '';
  };

  const openForm = (item: ApiEntity | null, trigger?: HTMLButtonElement) => {
    abortRelationSearches();
    selected = item; form.reset(); clearFieldErrors(); form.hidden = false; alert.hidden = true;
    focusReturn = trigger ? { action: trigger.dataset.action, id: trigger.dataset.id } : null;
    restoreFocusAfterRowsChange = false;
    query<HTMLElement>('[data-form-title]').textContent = `${item ? 'Edit' : 'Create'} ${resource.label}`;
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
    for (const container of root.querySelectorAll<HTMLElement>('[data-relation]')) void loadOptions(container);
    form.querySelector<HTMLElement>('input, select, textarea')?.focus();
  };

  const closeForm = () => {
    abortRelationSearches();
    form.hidden = true; selected = null; clearFieldErrors();
    restoreFocus();
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
    fail(new Error(message));
    return false;
  };

  form.addEventListener('submit', async event => {
    event.preventDefault();
    clearFieldErrors();
    alert.hidden = true;
    if (!form.reportValidity()) return;
    const submit = form.querySelector<HTMLButtonElement>('[type="submit"]')!;
    if (submit.disabled) return;
    const data = new FormData(form);
    if (!validateDateRange(data)) return;
    submit.disabled = true; submit.textContent = 'Saving…';
    const payload: Record<string, unknown> = {};
    for (const field of fields) {
      const value = String(data.get(field.name) ?? '');
      if (value === '' && ['guid', 'number', 'date', 'datetime-local', 'password'].includes(field.type)) continue;
      payload[field.name] = serializeFormValue(value, field.type);
    }
    try {
      if (resource.key === 'projects' && selected) payload.expectedVersion = selected.updatedAt ?? selected.createdAt;
      if (selected?.id) await webApiClient.update(resource.key, selected.id, payload);
      else await webApiClient.create(resource.key, payload);
      if (lifetime.signal.aborted) return;
      closeForm(); refresh();
    } catch (error) {
      if (lifetime.signal.aborted) return;
      if (resource.key === 'projects' && selected?.id && error instanceof ApiError && error.status === 409) {
        // Keep the user's edits but adopt the stored version, so the conflict is visible once and an
        // intentional second save replaces the other change instead of failing forever.
        try {
          const latest = await webApiClient.get<ApiEntity>(resource.key, String(selected.id), lifetime.signal);
          selected = { ...selected, updatedAt: latest.updatedAt, createdAt: latest.createdAt };
          failForm(new ApiError(409, `${error.message} The latest version was loaded: your edits are kept, and saving again replaces the other change.`));
          return;
        } catch { /* fall back to the original conflict message */ }
      }
      failForm(error);
    }
    finally { submit.disabled = false; submit.textContent = 'Save'; }
  }, { signal: lifetime.signal });

  root.addEventListener('click', async event => {
    const target = (event.target as Element).closest<HTMLButtonElement>('button[data-action]');
    if (!target || target.disabled) return;
    const item = items.find(item => item.id === target.dataset.id);
    switch (target.dataset.action) {
      case 'create': if (canWrite) openForm(null, target); break;
      case 'edit': if (item && canWrite) openForm(item, target); break;
      case 'cancel': closeForm(); break;
      case 'refresh': refresh(); break;
      case 'page': page = Number(target.dataset.page); refresh(); break;
      case 'search': case 'more': {
        const container = target.closest<HTMLElement>('[data-relation]')!;
        await loadOptions(container, target.dataset.action as 'search' | 'more');
        break;
      }
      case 'details': {
        if (!item) break;
        const detailFields = query<HTMLElement>('[data-detail-fields]'); detailFields.replaceChildren();
        for (const field of resource.fields.filter(field => field.type !== 'password')) {
          const row = document.createElement('div'); row.className = 'grid gap-1 py-3 text-sm sm:grid-cols-3 sm:gap-4';
          const term = document.createElement('dt'); term.className = 'text-subtle'; term.textContent = field.label;
          const value = document.createElement('dd');
          value.className = field.type === 'guid' && !field.relation ? 'break-all font-mono text-[0.8125rem] text-muted sm:col-span-2' : 'break-words sm:col-span-2';
          value.textContent = displayFieldText(field, item[field.name], relations);
          row.append(term, value); detailFields.append(row);
        }
        dialog.showModal(); break;
      }
      case 'close-details': dialog.close(); break;
      case 'delete': {
        if (!item?.id || !canWrite) break;
        const label = recordLabel(resource, item, relations);
        const confirmed = await confirmAction(`“${label}” will be removed from ${resource.pluralLabel.toLowerCase()}. It stays in the database as a soft-deleted record.`);
        // Not every browser returns focus to the trigger when a modal dialog closes, and a live
        // snapshot may have replaced the row meanwhile: focus its current Delete button, or the
        // stable fallback when the row is gone.
        focusReturn = { action: 'delete', id: target.dataset.id };
        restoreFocus();
        if (!confirmed) break;
        target.disabled = true;
        alert.hidden = true;
        try {
          await webApiClient.delete(resource.key, item.id);
          if (lifetime.signal.aborted) return;
          // The row disappears on the next snapshot; move focus to a stable control.
          focusReturn = null;
          restoreFocusAfterRowsChange = true;
          refresh();
        } catch (error) { if (!lifetime.signal.aborted) fail(error); }
        finally { target.disabled = false; }
      }
    }
  }, { signal: lifetime.signal });
  // A click on the backdrop (the dialog element itself, outside its content) dismisses it.
  for (const modal of [dialog, confirmDialog]) modal.addEventListener('click', event => { if (event.target === modal) modal.close(); }, { signal: lifetime.signal });
  query<HTMLButtonElement>('[data-confirm-accept]').addEventListener('click', () => confirmDialog.close('confirm'), { signal: lifetime.signal });
  query<HTMLButtonElement>('[data-confirm-cancel]').addEventListener('click', () => confirmDialog.close('cancel'), { signal: lifetime.signal });
  query<HTMLSelectElement>('[data-page-size]').addEventListener('change', event => {
    pageSize = Number((event.target as HTMLSelectElement).value); page = 1; refresh();
  }, { signal: lifetime.signal });

  configureRelationPickers();
  if (!canWrite) {
    createButton.hidden = true;
    permissionNote.textContent = `Only administrators can create, edit or delete ${resource.pluralLabel.toLowerCase()}. You can still browse them.`;
    permissionNote.hidden = false;
  }
  if (!canRead) {
    permissionNote.textContent = `${resource.pluralLabel} are managed by administrators. Your account does not have administrator access, so this list is not available.`;
    permissionNote.hidden = false;
    query<HTMLElement>('[data-listing]').hidden = true;
    return () => { lifetime.abort(); listing.abort(); };
  }
  refresh();
  return () => { lifetime.abort(); listing.abort(); abortRelationSearches(); };
};
