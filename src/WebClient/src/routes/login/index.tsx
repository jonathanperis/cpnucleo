import { component$, useSignal } from '@builder.io/qwik';
import { login } from '~/lib/api/identity-client';
import { getPostLoginRedirectTarget } from '~/lib/auth-navigation';

export default component$(() => {
  const loading = useSignal(false);
  const error = useSignal('');
  const success = useSignal(false);

  return (
    <section class="w-full max-w-md rounded-[2rem] border border-line bg-surface/95 p-6 shadow-soft backdrop-blur sm:p-8">
      <p class="text-sm font-semibold text-accent">IdentityApi login</p>
      <h2 class="mt-2 text-3xl font-semibold tracking-tight">Welcome back</h2>
      <p class="mt-3 text-sm leading-6 text-muted">Enter your CPnucleo credentials to open the dashboard workspace.</p>
      {error.value && <div role="alert" aria-live="assertive" aria-atomic="true" class="mt-5 rounded-xl border border-danger/25 bg-danger/5 px-4 py-3 text-sm text-danger">{error.value}</div>}
      {success.value && <div role="status" aria-live="polite" aria-atomic="true" class="mt-5 rounded-xl border border-success/25 bg-success/5 px-4 py-3 text-sm text-success">Login successful. Opening the dashboard…</div>}
      <form class="mt-6 space-y-5" preventdefault:submit onSubmit$={async (event) => {
        const form = event.currentTarget as HTMLFormElement;
        const data = new FormData(form);
        const loginName = String(data.get('login') ?? '');
        const password = String(data.get('password') ?? '');
        if (!loginName || !password) { error.value = 'Enter both login and password.'; return; }
        loading.value = true; error.value = ''; success.value = false;
        try {
          await login(loginName, password);
          success.value = true;
          form.reset();
          const params = new URLSearchParams(window.location.search);
          const returnUrl = params.get('returnUrl');
          window.location.assign(getPostLoginRedirectTarget(returnUrl));
        }
        catch (err) { error.value = err instanceof Error ? err.message : 'Unable to log in.'; }
        finally { loading.value = false; }
      }}>
        <label class="block">
          <span class="mb-2 block text-sm font-semibold">Login</span>
          <input name="login" autocomplete="username" class="w-full rounded-xl border border-line bg-raised px-4 py-3 text-ink shadow-sm transition placeholder:text-muted focus:border-accent" placeholder="your.login" />
        </label>
        <label class="block">
          <span class="mb-2 block text-sm font-semibold">Password</span>
          <input name="password" type="password" autocomplete="current-password" class="w-full rounded-xl border border-line bg-raised px-4 py-3 text-ink shadow-sm transition placeholder:text-muted focus:border-accent" placeholder="••••••••" />
        </label>
        <button disabled={loading.value} class="w-full rounded-xl bg-ink px-4 py-3 font-semibold text-canvas shadow-soft transition hover:opacity-90 disabled:cursor-not-allowed disabled:opacity-50">{loading.value ? 'Signing in…' : 'Enter dashboard'}</button>
      </form>
      <p class="mt-5 text-center text-sm text-muted">You will be redirected to your requested dashboard page after login.</p>
    </section>
  );
});
