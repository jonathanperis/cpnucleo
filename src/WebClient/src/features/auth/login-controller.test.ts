// @vitest-environment jsdom
import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { afterEach, expect, it, vi } from 'vitest';
import * as identity from '~/lib/api/identity-client';
import { mountLoginForm } from './login-controller';

afterEach(() => vi.restoreAllMocks());
it('submits the generated native form once and renders authentication errors accessibly', async () => {
  document.body.innerHTML = new DOMParser().parseFromString(readFileSync(resolve('dist/login/index.html'), 'utf8'), 'text/html').body.innerHTML;
  const form = document.querySelector<HTMLFormElement>('[data-login-form]')!;
  const login = vi.spyOn(identity, 'login').mockRejectedValue(new Error('Credentials rejected'));
  mountLoginForm(form);
  expect((form.elements.namedItem('login') as HTMLInputElement).value).toBe('');
  (form.elements.namedItem('login') as HTMLInputElement).value = 'learner';
  (form.elements.namedItem('password') as HTMLInputElement).value = 'test-password';
  form.dispatchEvent(new Event('submit', { cancelable: true }));
  form.dispatchEvent(new Event('submit', { cancelable: true }));
  await vi.waitFor(() => expect(document.querySelector('[data-login-error]')?.textContent).toBe('Credentials rejected'));
  expect(login).toHaveBeenCalledExactlyOnceWith('learner', 'test-password');
  expect(form.querySelector<HTMLButtonElement>('button')!.disabled).toBe(false);
});
