import { $, component$, useSignal, useStore, useVisibleTask$ } from '@builder.io/qwik';
import { formFields, tableFields } from '~/lib/api/resource-metadata';
import { webApiClient } from '~/lib/api/webapi-client';
import type { ApiEntity, ResourceKey, ResourceMetadata } from '~/lib/api/types';
import { buildPaginationItems, DEFAULT_PAGE_SIZE, getLastPage } from './pagination';

const formatValue = (value: unknown): string => {
  if (value === null || value === undefined || value === '') return '—';
  if (typeof value === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(value)) return new Date(value).toLocaleString();
  return String(value);
};

const inputType = (type: string) => type === 'guid' ? 'text' : type;

export const CrudPage = component$<{ resource: ResourceMetadata }>(({ resource }) => {
  const items = useSignal<ApiEntity[]>([]);
  const relations = useStore<Record<string, ApiEntity[]>>({});
  const loading = useSignal(true);
  const saving = useSignal(false);
  const error = useSignal('');
  const mode = useSignal<'list' | 'create' | 'edit'>('list');
  const selected = useSignal<ApiEntity | null>(null);
  const details = useSignal<ApiEntity | null>(null);
  const page = useSignal(1);
  const pageSize = useSignal(DEFAULT_PAGE_SIZE);
  const total = useSignal(0);
  const refreshKey = useSignal(0);

  const refresh = $(() => { refreshKey.value += 1; });

  useVisibleTask$(({ track, cleanup }) => {
    track(() => resource.key);
    track(() => page.value);
    track(() => pageSize.value);
    track(() => refreshKey.value);

    const controller = new AbortController();
    const requestedPage = page.value;
    const requestedPageSize = pageSize.value;
    loading.value = true;
    error.value = '';

    void webApiClient.subscribeList(resource.key, requestedPage, requestedPageSize, (result) => {
      if (controller.signal.aborted || page.value !== requestedPage || pageSize.value !== requestedPageSize) return;

      const nextTotal = result.totalCount ?? result.items?.length ?? 0;
      const lastPage = getLastPage(nextTotal, requestedPageSize);
      if (requestedPage > lastPage) {
        page.value = lastPage;
        return;
      }

      items.value = result.items ?? [];
      total.value = nextTotal;
      loading.value = false;
    }, controller.signal).catch((err) => {
      if (controller.signal.aborted) return;
      error.value = err instanceof Error ? err.message : 'Unable to load records.';
      loading.value = false;
    });

    cleanup(() => controller.abort());
  });

  useVisibleTask$(async ({ track }) => {
    track(() => resource.key);
    const relationKeys = [...new Set(formFields(resource).map((field) => field.relation).filter(Boolean))] as ResourceKey[];
    await Promise.all(relationKeys.map(async (key) => {
      try { relations[key] = (await webApiClient.list(key, 1, 100)).items ?? []; } catch { relations[key] = []; }
    }));
  });

  const startCreate = $(() => { selected.value = null; mode.value = 'create'; });
  const startEdit = $((item: ApiEntity) => { selected.value = item; mode.value = 'edit'; });
  const cancelForm = $(() => { selected.value = null; mode.value = 'list'; });

  const submit = $(async (event: SubmitEvent) => {
    event.preventDefault();
    const form = event.currentTarget as HTMLFormElement;
    const values = Object.fromEntries(new FormData(form).entries());
    const payload: Record<string, unknown> = {};
    for (const field of formFields(resource)) {
      const value = values[field.name];
      if (value === undefined || value === '') continue;
      payload[field.name] = field.type === 'number' ? Number(value) : String(value);
    }
    saving.value = true;
    error.value = '';
    try {
      if (mode.value === 'edit' && selected.value?.id) await webApiClient.update(resource.key, String(selected.value.id), payload);
      else await webApiClient.create(resource.key, payload);
      mode.value = 'list';
      selected.value = null;
      refreshKey.value += 1;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Unable to save record.';
    } finally { saving.value = false; }
  });

  const remove = $(async (item: ApiEntity) => {
    if (!item.id || !confirm(`Delete this ${resource.label.toLowerCase()}?`)) return;
    try { await webApiClient.delete(resource.key, String(item.id)); refreshKey.value += 1; }
    catch (err) { error.value = err instanceof Error ? err.message : 'Unable to delete record.'; }
  });

  const previousPage = $(() => { if (page.value > 1) page.value -= 1; });
  const nextPage = $(() => { if (page.value < getLastPage(total.value, pageSize.value)) page.value += 1; });
  const goToPage = $((nextPageNumber: number) => { page.value = Math.min(Math.max(1, nextPageNumber), getLastPage(total.value, pageSize.value)); });
  const changePageSize = $((_event: Event, currentTarget: HTMLSelectElement) => {
    pageSize.value = Number(currentTarget.value);
    page.value = 1;
  });

  const lastPage = getLastPage(total.value, pageSize.value);
  const displayPage = Math.min(Math.max(1, page.value), lastPage);
  const paginationItems = buildPaginationItems(displayPage, lastPage);
  const isFirstPage = displayPage <= 1;
  const isLastPage = displayPage >= lastPage;

  return (
    <section class="space-y-6">
      <div class="flex flex-col justify-between gap-4 sm:flex-row sm:items-end">
        <div>
          <p class="text-sm font-medium text-accent">{resource.pluralLabel}</p>
          <h2 class="mt-1 text-3xl font-semibold tracking-tight">Manage {resource.pluralLabel.toLowerCase()}</h2>
          <p class="mt-2 max-w-2xl text-sm text-muted">{resource.description}</p>
        </div>
        <button class="rounded-lg bg-accent px-4 py-2 text-sm font-semibold text-canvas shadow-soft ring-1 ring-accent/30 transition hover:bg-accent-hover hover:text-canvas disabled:opacity-50" onClick$={startCreate}>New {resource.label}</button>
      </div>

      {error.value && <div class="rounded-lg border border-danger/25 bg-danger/5 px-4 py-3 text-sm text-danger">{error.value}</div>}

      {mode.value !== 'list' && (
        <form preventdefault:submit onSubmit$={submit} class="rounded-xl border border-line bg-surface p-5 shadow-soft">
          <div class="mb-5 flex items-center justify-between">
            <h3 class="text-lg font-semibold">{mode.value === 'edit' ? 'Edit' : 'Create'} {resource.label}</h3>
            <button type="button" class="rounded-md border border-line px-3 py-2 text-sm" onClick$={cancelForm}>Cancel</button>
          </div>
          <div class="grid gap-4 md:grid-cols-2">
            {formFields(resource).map((field) => {
              const value = selected.value?.[field.name];
              const relation = field.relation ? relations[field.relation] ?? [] : [];
              return (
                <label key={field.name} class={field.type === 'textarea' ? 'md:col-span-2' : ''}>
                  <span class="mb-1 block text-sm font-medium">{field.label}{field.required ? ' *' : ''}</span>
                  {field.relation ? (
                    <select name={field.name} required={field.required} class="w-full rounded-lg border border-line bg-raised px-3 py-2 text-sm">
                      <option value="">{`Select ${field.label.toLowerCase()}`}</option>
                      {relation.map((option) => <option key={String(option.id)} value={String(option.id)} selected={String(option.id) === String(value ?? '')}>{formatValue(option.name ?? option.description ?? option.login ?? option.id)}</option>)}
                    </select>
                  ) : field.type === 'textarea' ? (
                    <textarea name={field.name} required={field.required} rows={3} class="w-full rounded-lg border border-line bg-raised px-3 py-2 text-sm" value={String(value ?? '')} />
                  ) : (
                    <input name={field.name} required={field.required} type={inputType(field.type)} step={field.type === 'number' ? '0.25' : undefined} class="w-full rounded-lg border border-line bg-raised px-3 py-2 text-sm" value={String(value ?? '')} />
                  )}
                </label>
              );
            })}
          </div>
          <div class="mt-5 flex justify-end gap-2">
            <button type="button" class="rounded-lg border border-line px-4 py-2 text-sm font-medium" onClick$={cancelForm}>Cancel</button>
            <button type="submit" disabled={saving.value} class="rounded-lg bg-accent px-4 py-2 text-sm font-semibold text-canvas shadow-sm ring-1 ring-accent/30 transition hover:bg-accent-hover disabled:opacity-50">{saving.value ? 'Saving…' : 'Save'}</button>
          </div>
        </form>
      )}

      <div class="overflow-hidden rounded-xl border border-line bg-surface shadow-soft">
        <div class="flex flex-col gap-3 border-b border-line px-4 py-3 lg:flex-row lg:items-center lg:justify-between">
          <p key={`page-summary-${displayPage}-${lastPage}-${total.value}`} class="text-sm text-muted" aria-live="polite">{total.value} records · page {displayPage} of {lastPage}</p>
          <div class="flex flex-wrap items-center gap-3">
            <label class="flex items-center gap-2 text-sm text-muted">
              Rows
              <select class="rounded-md border border-line bg-raised px-2 py-2 text-sm text-ink" value={pageSize.value} onChange$={changePageSize}>
                {[10, 25, 50, 100].map((size) => <option key={String(size)} value={String(size)}>{String(size)}</option>)}
              </select>
            </label>
            <nav class="flex items-center" aria-label="Pagination" data-current-page={displayPage}>
              <button
                type="button"
                class="mr-1 inline-flex h-8 min-w-8 items-center justify-center gap-1 rounded-md px-1.5 py-2 text-sm font-normal text-muted transition hover:bg-raised hover:text-ink disabled:cursor-not-allowed disabled:text-muted/70 disabled:hover:bg-transparent disabled:hover:text-muted/70"
                onClick$={previousPage}
                disabled={isFirstPage}
                aria-disabled={isFirstPage ? 'true' : undefined}
                aria-label="Previous Page"
              >
                <span aria-hidden="true" class="text-base leading-none">‹</span>
                Previous
              </button>
              <div class="flex items-center" role="group" aria-label={`${resource.label} pages`}>
                {paginationItems.map((paginationItem) => (
                  typeof paginationItem === 'number' ? (
                    <button
                      key={`${paginationItem}-${displayPage === paginationItem ? 'active' : 'idle'}`}
                      type="button"
                      class={`mr-1 inline-flex h-8 min-w-8 items-center justify-center rounded-md px-1.5 py-2 text-sm font-normal transition ${displayPage === paginationItem ? 'bg-[#0969da] text-white' : 'text-ink hover:bg-raised hover:text-[#0969da]'}`}
                      aria-current={displayPage === paginationItem ? 'page' : undefined}
                      aria-label={`Page ${paginationItem}`}
                      onClick$={() => goToPage(paginationItem)}
                    >
                      {paginationItem}
                    </button>
                  ) : (
                    <span key={paginationItem} class="mr-1 inline-flex h-8 min-w-8 items-center justify-center rounded-md px-1.5 py-2 text-sm font-normal text-muted" aria-hidden="true">…</span>
                  )
                ))}
              </div>
              <button
                type="button"
                class="inline-flex h-8 min-w-8 items-center justify-center gap-1 rounded-md px-1.5 py-2 text-sm font-normal text-[#0969da] transition hover:bg-raised hover:text-[#0969da] disabled:cursor-not-allowed disabled:text-muted/70 disabled:hover:bg-transparent disabled:hover:text-muted/70"
                onClick$={nextPage}
                disabled={isLastPage}
                aria-disabled={isLastPage ? 'true' : undefined}
                aria-label="Next Page"
              >
                Next
                <span aria-hidden="true" class="text-base leading-none">›</span>
              </button>
            </nav>
            <button class="rounded-md border border-line px-3 py-2 text-sm" onClick$={refresh}>Refresh</button>
          </div>
        </div>
        {loading.value ? <p class="p-6 text-sm text-muted">Loading {resource.pluralLabel.toLowerCase()}…</p> : items.value.length === 0 ? (
          <div class="p-8 text-center"><h3 class="font-semibold">No {resource.pluralLabel.toLowerCase()} yet</h3><p class="mt-1 text-sm text-muted">Create the first record to start using this resource.</p></div>
        ) : (
          <div class="overflow-x-auto">
            <table class="min-w-full divide-y divide-line text-sm">
              <thead class="bg-raised text-left text-xs font-semibold uppercase tracking-wide text-muted"><tr>{tableFields(resource).map((field) => <th key={field.name} class="px-4 py-3">{field.label}</th>)}<th class="px-4 py-3 text-right">Actions</th></tr></thead>
              <tbody class="divide-y divide-line">
                {items.value.map((item) => <tr key={String(item.id)} class="hover:bg-raised/70">{tableFields(resource).map((field) => <td key={field.name} class="max-w-xs truncate px-4 py-3">{formatValue(item[field.name])}</td>)}<td class="whitespace-nowrap px-4 py-3 text-right"><button class="mr-2 text-accent" onClick$={() => (details.value = item)}>Details</button><button class="mr-2 text-accent" onClick$={() => startEdit(item)}>Edit</button><button class="text-danger" onClick$={() => remove(item)}>Delete</button></td></tr>)}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {details.value && (
        <div
          class="fixed inset-0 z-50 flex justify-end bg-black/30"
          onClick$={() => (details.value = null)}
          onKeyDown$={(event) => { if (event.key === 'Escape') details.value = null; }}
        >
          <aside
            role="dialog"
            aria-modal="true"
            aria-labelledby={`${resource.key}-details-title`}
            tabIndex={-1}
            class="h-full w-full max-w-xl overflow-y-auto bg-surface p-6 shadow-soft"
            onClick$={(event) => event.stopPropagation()}
            onKeyDown$={(event) => {
              if (event.key !== 'Tab') return;
              const focusable = Array.from((event.currentTarget as HTMLElement).querySelectorAll<HTMLElement>('button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])')).filter((element) => !element.hasAttribute('disabled'));
              const first = focusable[0];
              const last = focusable[focusable.length - 1];
              if (!first || !last) return;
              if (event.shiftKey && document.activeElement === first) { event.preventDefault(); last.focus(); }
              else if (!event.shiftKey && document.activeElement === last) { event.preventDefault(); first.focus(); }
            }}
          >
            <div class="mb-4 flex items-center justify-between">
              <h3 id={`${resource.key}-details-title`} class="text-lg font-semibold">{resource.label} details</h3>
              <button class="rounded-md border border-line px-3 py-2" onClick$={() => (details.value = null)}>Close</button>
            </div>
            <dl class="divide-y divide-line rounded-lg border border-line">
              {resource.fields.map((field) => <div key={field.name} class="grid grid-cols-3 gap-3 px-4 py-3 text-sm"><dt class="font-medium text-muted">{field.label}</dt><dd class="col-span-2 break-all">{formatValue(details.value?.[field.name])}</dd></div>)}
            </dl>
          </aside>
        </div>
      )}
    </section>
  );
});
