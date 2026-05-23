import { component$, Slot, useSignal, useVisibleTask$ } from '@builder.io/qwik';
import { Link, useLocation } from '@builder.io/qwik-city';
import { clearStoredToken, getStoredToken } from '~/lib/api/http-client';

const groups = [
  { name: 'Work', items: [
    ['Dashboard', '/'], ['Organizations', '/organizations'], ['Projects', '/projects'], ['Assignments', '/assignments'], ['Workflows', '/workflows'], ['Appointments', '/appointments'],
  ] },
  { name: 'People', items: [['Users', '/users'], ['User assignments', '/user-assignments'], ['User projects', '/user-projects']] },
  { name: 'Configuration', items: [['Types', '/settings/types'], ['Relations', '/settings/relations'], ['Assignment types', '/assignment-types'], ['Impediments', '/impediments'], ['Assignment impediments', '/assignment-impediments']] },
  { name: 'System', items: [['API health', '/api-health'], ['Login', '/login']] },
] as const;

export const AppShell = component$(() => {
  const loc = useLocation();
  const mobileOpen = useSignal(false);
  const authed = useSignal(false);
  useVisibleTask$(() => { authed.value = Boolean(getStoredToken()); });

  return (
    <div class="min-h-screen bg-canvas text-ink">
      <aside class="fixed inset-y-0 left-0 z-40 hidden w-72 border-r border-line bg-surface px-5 py-6 lg:block">
        <Brand />
        <Navigation path={loc.url.pathname} />
      </aside>
      <div class="lg:pl-72">
        <header class="sticky top-0 z-30 border-b border-line bg-surface/95 backdrop-blur">
          <div class="flex h-16 items-center justify-between px-4 sm:px-6 lg:px-8">
            <button
              class="rounded-md border border-line px-3 py-2 text-sm lg:hidden"
              type="button"
              aria-expanded={mobileOpen.value}
              aria-controls="mobile-nav-drawer"
              onClick$={() => (mobileOpen.value = true)}
            >
              Menu
            </button>
            <div>
              <p class="text-sm font-medium text-muted">CPnucleo WebClient</p>
              <h1 class="text-lg font-semibold">Project operations</h1>
            </div>
            <div class="flex items-center gap-2 text-sm">
              <span class="rounded-full border border-line bg-raised px-3 py-1 text-muted">Local-ready</span>
              {authed.value ? (
                <button class="rounded-md border border-line px-3 py-2" onClick$={() => { clearStoredToken(); authed.value = false; }}>Logout</button>
              ) : <Link class="rounded-md bg-accent px-3 py-2 font-medium text-white" href="/login">Login</Link>}
            </div>
          </div>
        </header>
        {mobileOpen.value && (
          <div class="fixed inset-0 z-50 bg-black/30 lg:hidden" onClick$={() => (mobileOpen.value = false)}>
            <div
              id="mobile-nav-drawer"
              role="dialog"
              aria-modal="true"
              aria-label="Navigation menu"
              class="h-full w-80 bg-surface p-5 shadow-soft"
              onClick$={(event) => event.stopPropagation()}
            >
              <div class="flex items-center justify-between">
                <Brand />
                <button class="rounded-md border border-line px-3 py-2" onClick$={() => (mobileOpen.value = false)}>Close</button>
              </div>
              <Navigation path={loc.url.pathname} />
            </div>
          </div>
        )}
        <main class="px-4 py-8 sm:px-6 lg:px-8"><Slot /></main>
      </div>
    </div>
  );
});

export const Brand = component$(() => (
  <Link href="/" class="mb-8 flex items-center gap-3">
    <span class="flex h-10 w-10 items-center justify-center rounded-xl bg-ink text-sm font-bold text-white">CP</span>
    <span><span class="block text-base font-semibold">CPnucleo</span><span class="block text-xs text-muted">Project management</span></span>
  </Link>
));

export const Navigation = component$<{ path: string }>(({ path }) => {
  const normalizedPath = path !== '/' && path.endsWith('/') ? path.slice(0, -1) : path;

  return (
    <nav class="mt-8 space-y-6">
      {groups.map((group) => (
        <section key={group.name}>
          <h2 class="px-3 text-xs font-semibold uppercase tracking-wide text-muted">{group.name}</h2>
          <div class="mt-2 space-y-1">
            {group.items.map(([label, href]) => {
              const active = href === '/' ? normalizedPath === '/' : normalizedPath === href || normalizedPath.startsWith(`${href}/`);
              return <Link key={href} href={href} class={["block rounded-lg px-3 py-2 text-sm font-medium transition", active ? "bg-ink text-white shadow-sm" : "text-muted hover:bg-raised hover:text-ink"]}>{label}</Link>;
            })}
          </div>
        </section>
      ))}
    </nav>
  );
});
