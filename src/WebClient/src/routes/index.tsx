import { component$, useSignal, useVisibleTask$ } from '@builder.io/qwik';
import { resourceMetadata } from '~/lib/api/resource-metadata';
import { webApiClient } from '~/lib/api/webapi-client';

const shortcuts = [
  ['Project overview', 'See what the app does, where the main work areas are, and where to begin.', '/projects/'],
  ['People and access', 'Review team members and the work they are connected to.', '/users/'],
  ['Data and storage', 'Browse the core records that keep projects, tasks, blockers, and calendar items connected.', '/organizations/'],
  ['Checks and status', 'Confirm the app services are reachable before you keep working.', '/api-health/'],
] as const;

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

  const featured = resourceMetadata.slice(0, 6);

  return (
    <div class="space-y-8">
      <section class="overflow-hidden rounded-[2rem] border border-line bg-surface shadow-soft">
        <div class="grid gap-6 p-6 lg:grid-cols-[1.08fr_0.92fr] lg:p-8">
          <div>
            <p class="inline-flex rounded-full border border-accent/30 bg-accent/10 px-3 py-1 text-sm font-semibold text-accent">Application hub</p>
            <h2 class="mt-5 max-w-3xl text-4xl font-semibold tracking-tight sm:text-5xl">Choose where to start in Cpnucleo.</h2>
            <p class="mt-4 max-w-2xl text-base leading-7 text-muted">Use this space to review projects, people, tasks, blockers, calendar items, and service status without digging through setup details first.</p>
            <div class="mt-7 flex flex-wrap gap-3">
              <a class="rounded-xl bg-ink px-5 py-3 text-sm font-semibold text-canvas shadow-soft transition hover:opacity-90" href="/projects/">Open projects</a>
              <a class="rounded-xl border border-line bg-raised px-5 py-3 text-sm font-semibold transition hover:border-accent/40 hover:text-accent" href="/api-health/">Check service status</a>
            </div>
          </div>
          <div class="rounded-[1.5rem] border border-line bg-raised p-5">
            <div class="flex items-center justify-between border-b border-line pb-4">
              <div>
                <p class="text-sm font-semibold">Workspace pulse</p>
                <p class="text-xs text-muted">Live counts after sign in</p>
              </div>
              <span class="rounded-full bg-success/10 px-3 py-1 text-xs font-semibold text-success">Online</span>
            </div>
            <div class="mt-4 grid gap-3">
              {featured.slice(0, 4).map((resource) => (
                <a href={resource.routePath} class="flex items-center justify-between rounded-xl border border-line bg-surface px-4 py-3 transition hover:border-accent/40">
                  <span class="text-sm font-semibold">{resource.pluralLabel}</span>
                  <span class="rounded-full bg-raised px-2.5 py-1 text-xs text-muted">{counts.value[resource.key] ?? '—'}</span>
                </a>
              ))}
            </div>
          </div>
        </div>
      </section>

      <section class="rounded-[2rem] border border-line bg-surface p-5 shadow-soft lg:p-6">
        <div class="mb-4">
          <p class="text-sm font-semibold text-accent">Start</p>
          <h3 class="mt-1 text-2xl font-semibold tracking-tight">Pick the path that matches your next step</h3>
        </div>
        <div class="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
          {shortcuts.map(([title, description, href]) => (
            <a href={href} class="group rounded-2xl border border-line bg-raised p-5 transition hover:-translate-y-0.5 hover:border-accent/40 hover:shadow-soft">
              <span class="font-semibold">{title}</span>
              <p class="mt-3 text-sm leading-6 text-muted">{description}</p>
              <span class="mt-4 inline-flex text-sm font-semibold text-accent opacity-80 transition group-hover:opacity-100">Open →</span>
            </a>
          ))}
        </div>
      </section>

      <section>
        <div class="mb-4 flex items-end justify-between gap-4">
          <div>
            <p class="text-sm font-semibold text-accent">Work areas</p>
            <h3 class="mt-1 text-2xl font-semibold tracking-tight">Manage the records that power the app</h3>
          </div>
          <span class="hidden text-sm text-muted sm:inline">Dark by default · light-ready</span>
        </div>
        <div class="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
          {resourceMetadata.map((resource) => (
            <a key={resource.key} href={resource.routePath} class="group rounded-2xl border border-line bg-surface p-5 shadow-sm transition hover:-translate-y-0.5 hover:border-accent/40 hover:shadow-soft">
              <div class="flex items-start justify-between gap-3">
                <span class="font-semibold">{resource.pluralLabel}</span>
                <span class="rounded-full bg-raised px-2.5 py-1 text-xs text-muted">{counts.value[resource.key] ?? '—'}</span>
              </div>
              <p class="mt-3 text-sm leading-6 text-muted">{resource.description}</p>
              <span class="mt-4 inline-flex text-sm font-semibold text-accent opacity-80 transition group-hover:opacity-100">Open area →</span>
            </a>
          ))}
        </div>
      </section>
    </div>
  );
});
