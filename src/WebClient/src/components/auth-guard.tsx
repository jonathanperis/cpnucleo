import { component$, Slot, useSignal, useVisibleTask$ } from '@builder.io/qwik';
import { clearStoredToken, getStoredToken, setupSessionActivityTracking } from '~/lib/api/http-client';
import { getLoginPath, getLoginRedirectTarget } from '~/lib/auth-navigation';

const loginPath = getLoginPath();

export const AuthGuard = component$(() => {
  const checked = useSignal(false);
  const authenticated = useSignal(false);

  useVisibleTask$(() => {
    const token = getStoredToken();
    authenticated.value = Boolean(token);
    checked.value = true;
    if (!token) {
      window.location.replace(getLoginRedirectTarget(window.location));
      return;
    }

    return setupSessionActivityTracking();
  });

  if (!checked.value) {
    return (
      <section class="rounded-2xl border border-line bg-surface p-6 shadow-soft" aria-live="polite">
        <p class="text-sm font-medium text-accent">Secure session</p>
        <h2 class="mt-2 text-2xl font-semibold">Checking your session…</h2>
        <p class="mt-2 text-sm text-muted">Please sign in before opening the Cpnucleo workspace.</p>
      </section>
    );
  }

  if (!authenticated.value) {
    return (
      <section class="rounded-2xl border border-line bg-surface p-6 shadow-soft" aria-live="polite">
        <p class="text-sm font-medium text-accent">Secure session</p>
        <h2 class="mt-2 text-2xl font-semibold">Redirecting to sign in…</h2>
        <p class="mt-2 text-sm text-muted">Sign in to continue.</p>
      </section>
    );
  }

  return <Slot />;
});

export const AuthStatus = component$(() => {
  const checked = useSignal(false);
  const authenticated = useSignal(false);

  useVisibleTask$(() => {
    authenticated.value = Boolean(getStoredToken());
    checked.value = true;
  });

  if (!checked.value) return <span class="rounded-full border border-line bg-raised px-3 py-1 text-muted">Checking session</span>;

  return authenticated.value ? (
    <button
      class="rounded-md border border-line px-3 py-2"
      type="button"
      onClick$={() => {
        clearStoredToken();
        window.location.assign(loginPath);
      }}
    >
      Logout
    </button>
  ) : (
    <a class="rounded-md bg-accent px-3 py-2 font-medium text-white" href={loginPath}>Login</a>
  );
});
