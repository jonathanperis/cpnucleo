import { findResource, formFields, tableFields } from '~/lib/api/resource-metadata';
import { webApiClient } from '~/lib/api/webapi-client';
import type { ApiEntity, ResourceKey } from '~/lib/api/types';
import { buildPaginationItems, DEFAULT_PAGE_SIZE, getLastPage } from './pagination';
import { formatFormFieldValue, serializeFormValue, withSelectedRelationOption } from './crud-field-values';
import { collectMissingRelationIds, displayEntityLabel, displayFieldValue, mergeRelationRecords } from './relation-display';
import { watchListing } from './watch-list';

export const mountCrudPage = (root: HTMLElement): (() => void) => {
  const resource = findResource(root.dataset.crud as ResourceKey);
  const query = <T extends Element>(selector: string) => root.querySelector<T>(selector)!;
  const form = query<HTMLFormElement>('[data-form]');
  const alert = query<HTMLElement>('[data-error]');
  const dialog = query<HTMLDialogElement>('[data-details]');
  const lifetime = new AbortController();
  const relations: Record<string, ApiEntity[]> = {};
  const options: Record<string, ApiEntity[]> = {};
  const relationPages: Record<string, number> = {};
  const relationVersions: Record<string, number> = {};
  let listing = new AbortController();
  let items: ApiEntity[] = [];
  let selected: ApiEntity | null = null;
  let page = 1;
  let pageSize = DEFAULT_PAGE_SIZE;
  let total = 0;
  let status = 'Connecting';
  let renderedRows = '';
  let renderedPages = '';

  const fail = (error: unknown) => {
    alert.textContent = error instanceof Error ? error.message : 'Unable to complete the request.';
    alert.hidden = false;
  };
  const fieldControl = (name: string) => form.elements.namedItem(name) as HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement;
  const actionButton = (label: string, action: string, id?: string) => {
    const button = document.createElement('button');
    button.type = 'button'; button.textContent = label; button.dataset.action = action;
    if (id) button.dataset.id = id;
    button.className = 'mr-2 text-accent';
    return button;
  };

  const render = () => {
    query<HTMLElement>('[data-summary]').textContent = `${total} records · page ${page} of ${getLastPage(total, pageSize)} · ${status}`;
    const body = query<HTMLTableSectionElement>('[data-records]');
    const rowSignature = JSON.stringify(items.map(item => [item.id, ...tableFields(resource).map(field => displayFieldValue(item[field.name], field.relation, relations))]));
    if (rowSignature !== renderedRows) {
      renderedRows = rowSignature;
      body.replaceChildren(...items.map(item => {
      const row = document.createElement('tr');
      row.className = 'hover:bg-raised/70';
      for (const field of tableFields(resource)) {
        const cell = row.insertCell(); cell.className = 'max-w-xs truncate px-4 py-3';
        cell.textContent = displayFieldValue(item[field.name], field.relation, relations);
      }
      const actions = row.insertCell(); actions.className = 'whitespace-nowrap px-4 py-3 text-right';
      actions.append(actionButton('Details', 'details', item.id), actionButton('Edit', 'edit', item.id), actionButton('Delete', 'delete', item.id));
      return row;
      }));
    }
    query<HTMLElement>('[data-empty]').hidden = items.length > 0;
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
      const button = actionButton(label, 'page'); button.dataset.page = String(target); button.disabled = disabled;
      button.setAttribute('aria-label', label === 'Previous' || label === 'Next' ? `${label} Page` : `Page ${label}`);
      button.setAttribute('aria-disabled', String(disabled));
      button.className = 'mr-1 inline-flex h-8 min-w-8 items-center justify-center rounded-md px-1.5 py-2 text-sm font-normal disabled:opacity-40';
      if (Number(label) === page) { button.setAttribute('aria-current', 'page'); button.classList.add('bg-[#0969da]', 'text-white'); }
      nav.append(button);
    };
    addPage('Previous', page - 1, page === 1);
    for (const value of buildPaginationItems(page, lastPage)) {
      if (typeof value === 'number') addPage(String(value), value);
      else { const dots = document.createElement('span'); dots.textContent = '…'; nav.append(dots); }
    }
    addPage('Next', page + 1, page === lastPage);
  };

  const loadMissingLabels = async (signal: AbortSignal) => {
    const missing = collectMissingRelationIds(items, resource.fields, relations);
    await Promise.all(Object.entries(missing).map(async ([key, ids]) => {
      const found = await webApiClient.lookup(key as ResourceKey, ids ?? [], signal).catch(() => []);
      if (signal.aborted) return;
      relations[key] = mergeRelationRecords(relations[key] ?? [], found);
    }));
    if (!signal.aborted) render();
  };

  const refresh = () => {
    listing.abort(); listing = new AbortController();
    const signal = listing.signal;
    const requestedPage = page;
    const requestedSize = pageSize;
    alert.hidden = true;
    void watchListing(() => webApiClient.subscribeList(resource.key, requestedPage, requestedSize, result => {
      if (signal.aborted) return;
      total = result.totalCount ?? 0;
      const lastPage = getLastPage(total, pageSize);
      if (page > lastPage) { page = lastPage; refresh(); return; }
      items = result.items ?? []; status = 'Live'; render();
      void loadMissingLabels(signal);
    }, signal), signal, value => { status = value; render(); }).catch(error => { if (!signal.aborted) fail(error); });
  };

  const renderOptions = (key: string) => {
    for (const field of formFields(resource).filter(field => field.relation === key)) {
      const control = fieldControl(field.name) as HTMLSelectElement;
      const selectedId = control.value || String(selected?.[field.name] ?? '');
      control.replaceChildren(new Option(`Select ${field.label.toLowerCase()}`, ''),
        ...withSelectedRelationOption(options[key] ?? [], selectedId).map(item => new Option(displayEntityLabel(item), String(item.id))));
      control.value = selectedId;
    }
  };

  const loadOptions = async (container: HTMLElement, nextPage = 1) => {
    const key = container.dataset.relation as ResourceKey;
    const version = relationVersions[key] = (relationVersions[key] ?? 0) + 1;
    const message = container.querySelector<HTMLElement>('[data-relation-error]')!;
    message.textContent = 'Loading options…';
    const search = container.querySelector<HTMLInputElement>('[data-query]')!.value;
    try {
      const result = await webApiClient.list(key, nextPage, 100, lifetime.signal, search);
      if (lifetime.signal.aborted || relationVersions[key] !== version) return;
      options[key] = nextPage === 1 ? result.items ?? [] : mergeRelationRecords(options[key] ?? [], result.items ?? []);
      relations[key] = mergeRelationRecords(relations[key] ?? [], result.items ?? []);
      relationPages[key] = nextPage;
      container.querySelector<HTMLElement>('[data-action="more"]')!.hidden = nextPage * 100 >= (result.totalCount ?? 0);
      message.textContent = `${options[key].length} options loaded`;
      renderOptions(key); render();
    } catch (error) {
      if (!lifetime.signal.aborted) message.textContent = error instanceof Error ? error.message : 'Unable to load options.';
    }
  };

  const openForm = (item: ApiEntity | null) => {
    selected = item; form.reset(); form.hidden = false; alert.hidden = true;
    query<HTMLElement>('[data-form-title]').textContent = `${item ? 'Edit' : 'Create'} ${resource.label}`;
    for (const field of formFields(resource)) {
      const control = fieldControl(field.name);
      control.required = Boolean(field.required || (!item && field.requiredOnCreate));
      const value = formatFormFieldValue(item?.[field.name], field.type);
      if (field.relation && value) (control as HTMLSelectElement).add(new Option(displayFieldValue(value, field.relation, relations), value));
      control.value = value;
    }
    for (const container of root.querySelectorAll<HTMLElement>('[data-relation]')) void loadOptions(container);
    form.querySelector<HTMLElement>('input, select, textarea')?.focus();
  };

  form.addEventListener('submit', async event => {
    event.preventDefault();
    if (!form.reportValidity()) return;
    const submit = form.querySelector<HTMLButtonElement>('[type="submit"]')!;
    if (submit.disabled) return;
    submit.disabled = true; submit.textContent = 'Saving…'; alert.hidden = true;
    const payload: Record<string, unknown> = {};
    const data = new FormData(form);
    for (const field of formFields(resource)) {
      const value = String(data.get(field.name) ?? '');
      if (value === '' && ['guid', 'number', 'date', 'datetime-local', 'password'].includes(field.type)) continue;
      payload[field.name] = serializeFormValue(value, field.type);
    }
    try {
      if (resource.key === 'projects' && selected) payload.expectedVersion = selected.updatedAt ?? selected.createdAt;
      if (selected?.id) await webApiClient.update(resource.key, selected.id, payload);
      else await webApiClient.create(resource.key, payload);
      if (lifetime.signal.aborted) return;
      form.hidden = true; selected = null; refresh();
    } catch (error) { if (!lifetime.signal.aborted) fail(error); }
    finally { submit.disabled = false; submit.textContent = 'Save'; }
  }, { signal: lifetime.signal });

  root.addEventListener('click', async event => {
    const target = (event.target as Element).closest<HTMLButtonElement>('button[data-action]');
    if (!target || target.disabled) return;
    const item = items.find(item => item.id === target.dataset.id);
    switch (target.dataset.action) {
      case 'create': openForm(null); break;
      case 'edit': if (item) openForm(item); break;
      case 'cancel': form.hidden = true; selected = null; break;
      case 'refresh': refresh(); break;
      case 'page': page = Number(target.dataset.page); refresh(); break;
      case 'search': case 'more': {
        const container = target.closest<HTMLElement>('[data-relation]')!;
        await loadOptions(container, target.dataset.action === 'search' ? 1 : (relationPages[container.dataset.relation!] ?? 1) + 1);
        break;
      }
      case 'details': {
        if (!item) break;
        const fields = query<HTMLElement>('[data-detail-fields]'); fields.replaceChildren();
        for (const field of resource.fields.filter(field => field.type !== 'password')) {
          const row = document.createElement('div'); row.className = 'grid grid-cols-3 gap-3 px-4 py-3 text-sm';
          const term = document.createElement('dt'); term.textContent = field.label;
          const value = document.createElement('dd'); value.className = 'col-span-2 break-all'; value.textContent = displayFieldValue(item[field.name], field.relation, relations);
          row.append(term, value); fields.append(row);
        }
        dialog.showModal(); break;
      }
      case 'close-details': dialog.close(); break;
      case 'delete':
        if (item?.id && confirm(`Delete this ${resource.label.toLowerCase()}?`)) {
          target.disabled = true;
          try { await webApiClient.delete(resource.key, item.id); if (!lifetime.signal.aborted) refresh(); }
          catch (error) { if (!lifetime.signal.aborted) fail(error); }
          finally { target.disabled = false; }
        }
    }
  }, { signal: lifetime.signal });
  query<HTMLSelectElement>('[data-page-size]').addEventListener('change', event => {
    pageSize = Number((event.target as HTMLSelectElement).value); page = 1; refresh();
  }, { signal: lifetime.signal });
  refresh();
  return () => { lifetime.abort(); listing.abort(); };
};
