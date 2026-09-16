import { login } from '~/lib/api/identity-client';
import { getPostLoginRedirectTarget } from '~/lib/auth-navigation';

export const mountLoginForm = (form: HTMLFormElement) => {
  const section = form.parentElement!;
  const error = section.querySelector<HTMLElement>('[data-login-error]')!;
  const status = section.querySelector<HTMLElement>('[data-login-status]')!;
  const button = form.querySelector<HTMLButtonElement>('[type="submit"]')!;
  form.addEventListener('submit', async event => {
    event.preventDefault();
    if (button.disabled || !form.reportValidity()) return;
    const data = new FormData(form);
    button.disabled = true; button.textContent = 'Signing in…'; error.hidden = true;
    try {
      await login(String(data.get('login') ?? ''), String(data.get('password') ?? ''));
      status.textContent = 'Login successful. Opening the dashboard…';
      window.location.assign(getPostLoginRedirectTarget(new URLSearchParams(window.location.search).get('returnUrl')));
    } catch (failure) {
      error.textContent = failure instanceof Error ? failure.message : 'Unable to sign in.'; error.hidden = false;
    } finally { button.disabled = false; button.textContent = 'Enter dashboard'; }
  });
};
