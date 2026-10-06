// @vitest-environment jsdom
import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { afterEach, beforeEach, expect, it, vi } from 'vitest';
import * as identity from '~/lib/api/identity-client';
import { resourceMetadata } from '~/lib/api/resource-metadata';
import { mountLoginForm } from './login-controller';

beforeEach(() => {
  document.body.innerHTML = new DOMParser().parseFromString(readFileSync(resolve('dist/login/index.html'), 'utf8'), 'text/html').body.innerHTML;
});
afterEach(() => vi.restoreAllMocks());

const fillAndSubmit = (form: HTMLFormElement, times = 1) => {
  (form.elements.namedItem('login') as HTMLInputElement).value = 'learner';
  (form.elements.namedItem('password') as HTMLInputElement).value = 'test-password';
  for (let index = 0; index < times; index += 1) form.dispatchEvent(new Event('submit', { cancelable: true }));
};

it('submits the generated native form once and renders authentication errors accessibly', async () => {
  const form = document.querySelector<HTMLFormElement>('[data-login-form]')!;
  const login = vi.spyOn(identity, 'login').mockRejectedValue(new Error('Credentials rejected'));
  mountLoginForm(form);
  expect((form.elements.namedItem('login') as HTMLInputElement).value).toBe('');
  fillAndSubmit(form, 2);
  await vi.waitFor(() => expect(document.querySelector('[data-login-error]')?.textContent).toBe('Credentials rejected'));
  expect(login).toHaveBeenCalledExactlyOnceWith('learner', 'test-password');
  expect(form.querySelector<HTMLButtonElement>('button')!.disabled).toBe(false);
});

it('explains a sign-in lockout with the server retry time', async () => {
  const form = document.querySelector<HTMLFormElement>('[data-login-form]')!;
  vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(
    JSON.stringify({ statusCode: 429, message: 'Too many failed sign-in attempts for this login.' }),
    { status: 429, headers: { 'Content-Type': 'application/json', 'Retry-After': '45' } },
  ));
  mountLoginForm(form);
  fillAndSubmit(form);
  await vi.waitFor(() => expect(document.querySelector('[data-login-error]')?.textContent)
    .toBe('Too many failed sign-in attempts for this login. Try again in 45 seconds.'));
  expect(document.querySelector<HTMLElement>('[data-login-error]')!.hidden).toBe(false);
});

it('derives the advertised number of work areas from resource metadata', () => {
  expect(document.querySelector('[data-work-area-count]')?.textContent).toBe(String(resourceMetadata.length));
  expect(resourceMetadata).toHaveLength(11);
});
