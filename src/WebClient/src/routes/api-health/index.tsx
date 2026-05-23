import { $, component$, useSignal } from '@builder.io/qwik';
import { IDENTITY_API_BASE_URL, WEBAPI_BASE_URL, withoutApiSuffix } from '~/lib/config';

type Health = { name: string; url: string; status: string; ok: boolean };

export default component$(() => {
  const checks = useSignal<Health[]>([]);
  const loading = useSignal(false);

  const run = $(async () => {
    loading.value = true;
    const targets = [
      { name: 'WebApi', url: `${withoutApiSuffix(WEBAPI_BASE_URL)}/healthz` },
      { name: 'IdentityApi', url: `${withoutApiSuffix(IDENTITY_API_BASE_URL)}/healthz` },
    ];
    checks.value = await Promise.all(targets.map(async (target) => {
      try {
        const response = await fetch(target.url, { cache: 'no-store' });
        return { ...target, status: String(response.status), ok: response.ok };
      } catch (error) {
        return { ...target, status: error instanceof Error ? error.message : 'unreachable', ok: false };
      }
    }));
    loading.value = false;
  });

  return (
    <section class="space-y-5">
      <div class="flex items-end justify-between gap-4">
        <div><p class="text-sm font-medium text-accent">System</p><h2 class="text-3xl font-semibold">API health</h2><p class="mt-2 text-sm text-muted">Checks browser reachability for the configured WebApi and IdentityApi health endpoints.</p></div>
        <button class="rounded-lg bg-ink px-4 py-2 text-sm font-semibold text-white" onClick$={run} disabled={loading.value}>{loading.value ? 'Checking…' : 'Run checks'}</button>
      </div>
      <div class="grid gap-4 md:grid-cols-2" aria-live="polite" aria-busy={loading.value ? 'true' : 'false'}>
        {checks.value.length === 0 ? <p class="text-sm text-muted">Run checks to see current API status.</p> : checks.value.map((check) => <article key={check.name} class="rounded-xl border border-line bg-surface p-5 shadow-soft"><div class="flex items-center justify-between"><h3 class="font-semibold">{check.name}</h3><span class={["rounded-full px-2 py-1 text-xs font-medium", check.ok ? "bg-success/10 text-success" : "bg-danger/10 text-danger"]}>{check.ok ? 'Healthy' : 'Unavailable'}</span></div><p class="mt-2 break-all text-sm text-muted">{check.url}</p><p class="mt-3 text-sm">Status: {check.status}</p></article>)}
      </div>
    </section>
  );
});
