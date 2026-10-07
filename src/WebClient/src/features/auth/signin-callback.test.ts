// @vitest-environment jsdom
import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { afterEach, beforeEach, expect, it, vi } from 'vitest';
import * as oidc from '~/lib/api/oidc';
import { mountSignInCallback } from './signin-callback';

beforeEach(() => {
  document.body.innerHTML = new DOMParser().parseFromString(readFileSync(resolve('dist/signin-callback/index.html'), 'utf8'), 'text/html').body.innerHTML;
  window.history.replaceState(null, '', '/signin-callback/?code=secret-code&state=s');
});
afterEach(() => vi.restoreAllMocks());

const section = () => document.querySelector<HTMLElement>('[data-signin-callback]')!;

it('removes the code from the address bar and opens the page the sign-in started from', async () => {
  const complete = vi.spyOn(oidc, 'completeSignIn').mockResolvedValue('/projects/');
  const replace = vi.fn();

  await mountSignInCallback(section(), { search: '?code=secret-code&state=s' }, replace);

  expect(complete).toHaveBeenCalledWith('?code=secret-code&state=s');
  expect(window.location.search).toBe('');
  expect(replace).toHaveBeenCalledWith('/projects/');
});

it('explains a failed sign-in accessibly and offers to start again', async () => {
  vi.spyOn(oidc, 'completeSignIn').mockRejectedValue(new oidc.SignInError('This sign-in is no longer valid. Sign in again.'));
  const replace = vi.fn();

  await mountSignInCallback(section(), { search: '?code=c&state=forged' }, replace);

  const error = document.querySelector<HTMLElement>('[data-callback-error]')!;
  expect(error.hidden).toBe(false);
  expect(error.getAttribute('role')).toBe('alert');
  expect(error.textContent).toBe('This sign-in is no longer valid. Sign in again.');
  expect(document.querySelector<HTMLAnchorElement>('[data-callback-retry]')!.hidden).toBe(false);
  expect(document.querySelector<HTMLAnchorElement>('[data-callback-retry]')!.getAttribute('href')).toBe('/login/');
  expect(replace).not.toHaveBeenCalled();
});
