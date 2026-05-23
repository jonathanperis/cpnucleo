import { component$, useSignal } from '@builder.io/qwik';
import { Link } from '@builder.io/qwik-city';
import { login } from '~/lib/api/identity-client';

export default component$(() => {
  const loading = useSignal(false);
  const error = useSignal('');
  const success = useSignal(false);

  return (
    <section class="mx-auto max-w-md rounded-2xl border border-line bg-surface p-6 shadow-soft">
      <p class="text-sm font-medium text-accent">IdentityApi</p>
      <h2 class="mt-1 text-2xl font-semibold">Sign in</h2>
      <p class="mt-2 text-sm text-muted">Session tokens are stored in sessionStorage and sent as bearer tokens to WebApi requests.</p>
      {error.value && <div class="mt-4 rounded-lg border border-danger/25 bg-danger/5 px-3 py-2 text-sm text-danger">{error.value}</div>}
      {success.value && <div class="mt-4 rounded-lg border border-success/25 bg-success/5 px-3 py-2 text-sm text-success">Login successful. Continue to the dashboard.</div>}
      <form class="mt-5 space-y-4" preventdefault:submit onSubmit$={async (event) => {
        const form = event.currentTarget as HTMLFormElement;
        const data = new FormData(form);
        const loginName = String(data.get('login') ?? '');
        const password = String(data.get('password') ?? '');
        if (!loginName || !password) { error.value = 'Enter both login and password.'; return; }
        loading.value = true; error.value = ''; success.value = false;
        try { await login(loginName, password); success.value = true; form.reset(); }
        catch (err) { error.value = err instanceof Error ? err.message : 'Unable to log in.'; }
        finally { loading.value = false; }
      }}>
        <label><span class="mb-1 block text-sm font-medium">Login</span><input name="login" autocomplete="username" class="w-full rounded-lg border border-line bg-raised px-3 py-2" /></label>
        <label><span class="mb-1 block text-sm font-medium">Password</span><input name="password" type="password" autocomplete="current-password" class="w-full rounded-lg border border-line bg-raised px-3 py-2" /></label>
        <button disabled={loading.value} class="w-full rounded-lg bg-ink px-4 py-2 font-semibold text-white disabled:opacity-50">{loading.value ? 'Signing in…' : 'Sign in'}</button>
      </form>
      <Link class="mt-4 block text-center text-sm text-accent" href="/">Back to dashboard</Link>
    </section>
  );
});
