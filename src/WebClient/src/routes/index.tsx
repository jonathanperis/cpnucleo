import { component$, useSignal, useVisibleTask$ } from '@builder.io/qwik';
import { resourceMetadata } from '~/lib/api/resource-metadata';
import { webApiClient } from '~/lib/api/webapi-client';

export default component$(() => {
  const counts = useSignal<Record<string, number>>({});
  useVisibleTask$(async () => {
    const next: Record<string, number> = {};
    await Promise.all(resourceMetadata.slice(0, 6).map(async (resource) => {
      try { next[resource.key] = (await webApiClient.list(resource.key, 1, 5)).totalCount ?? 0; }
      catch { next[resource.key] = 0; }
    }));
    counts.value = next;
  });

  return (
    <div class="space-y-8">
      <section class="rounded-2xl border border-line bg-surface p-6 shadow-soft">
        <p class="text-sm font-medium text-accent">Astro + Qwik WebClient</p>
        <h2 class="mt-2 text-3xl font-semibold tracking-tight">Run CPnucleo operations from one clean workspace.</h2>
        <p class="mt-3 max-w-3xl text-sm leading-6 text-muted">Manage organizations, projects, assignments, workflows, people, appointments, and join records through the WebApi CRUD surface.</p>
        <div class="mt-5 flex flex-wrap gap-2">
          <a class="rounded-lg bg-ink px-4 py-2 text-sm font-semibold text-white" href="/projects">Open projects</a>
          <a class="rounded-lg border border-line px-4 py-2 text-sm font-semibold" href="/api-health">Check API health</a>
        </div>
      </section>
      <section>
        <h3 class="mb-3 text-lg font-semibold">Resource overview</h3>
        <div class="grid gap-3 md:grid-cols-2 xl:grid-cols-3">
          {resourceMetadata.map((resource) => <a key={resource.key} href={resource.routePath} class="rounded-xl border border-line bg-surface p-4 shadow-sm hover:shadow-soft"><div class="flex items-center justify-between"><span class="font-semibold">{resource.pluralLabel}</span><span class="rounded-full bg-raised px-2 py-1 text-xs text-muted">{counts.value[resource.key] ?? '—'}</span></div><p class="mt-2 text-sm text-muted">{resource.description}</p></a>)}
        </div>
      </section>
    </div>
  );
});
