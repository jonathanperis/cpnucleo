import { $, component$, useSignal, useStore, useVisibleTask$ } from '@builder.io/qwik';
import { formFields, tableFields } from '~/lib/api/resource-metadata';
import { webApiClient } from '~/lib/api/webapi-client';
import type { ApiEntity, ResourceKey, ResourceMetadata } from '~/lib/api/types';

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
  const total = useSignal(0);

  const load = $(async () => {
    loading.value = true;
    error.value = '';
    try {
      const result = await webApiClient.list(resource.key, page.value, 25);
      items.value = result.items ?? [];
      total.value = result.totalCount ?? items.value.length;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Unable to load records.';
    } finally {
      loading.value = false;
    }
  });

  useVisibleTask$(async ({ track }) => {
    track(() => resource.key);
    await load();
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
      await load();
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Unable to save record.';
    } finally { saving.value = false; }
  });

  const remove = $(async (item: ApiEntity) => {
    if (!item.id || !confirm(`Delete this ${resource.label.toLowerCase()}?`)) return;
    try { await webApiClient.delete(resource.key, String(item.id)); await load(); }
    catch (err) { error.value = err instanceof Error ? err.message : 'Unable to delete record.'; }
  });

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
        <div class="flex items-center justify-between border-b border-line px-4 py-3">
          <p class="text-sm text-muted">{total.value} records</p>
          <button class="rounded-md border border-line px-3 py-2 text-sm" onClick$={load}>Refresh</button>
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
