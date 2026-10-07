// @vitest-environment jsdom
import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { afterEach, beforeEach, expect, it, vi } from 'vitest';
import { resourceMetadata } from '~/lib/api/resource-metadata';
import { pendingSignInStorageKey } from '~/lib/api/oidc';
import { mountLoginPage, signInNoticeStorageKey, signInNotices } from './login-controller';

const authRequest = 'http://localhost:5200/connect/authorize/callback?client_id=cpnucleo-webclient&state=s';

beforeEach(() => {
  document.body.innerHTML = new DOMParser().parseFromString(readFileSync(resolve('dist/login/index.html'), 'utf8'), 'text/html').body.innerHTML;
  sessionStorage.clear();
});
afterEach(() => vi.restoreAllMocks());

const section = () => document.querySelector<HTMLElement>('[data-login]')!;
const form = () => document.querySelector<HTMLFormElement>('[data-login-form]')!;
const at = (search: string) => ({ search });

it('shows the native sign-in form for a pending authorization request, posting to IdentityApi', async () => {
  const navigate = vi.fn();
  await mountLoginPage(section(), at(`?authRequest=${encodeURIComponent(authRequest)}`), navigate);

  expect(form().hidden).toBe(false);
  expect(form().method).toBe('post');
  expect(form().action).toBe('http://localhost:5200/api/account/login');
  expect((form().elements.namedItem('authRequest') as HTMLInputElement).value).toBe(authRequest);
  expect((form().elements.namedItem('login') as HTMLInputElement).value).toBe('');
  expect((form().elements.namedItem('password') as HTMLInputElement).value).toBe('');
  expect(document.activeElement).toBe(form().elements.namedItem('login'));
  expect(navigate).not.toHaveBeenCalled();
});

it('submits once: the button is disabled while the browser posts the form', async () => {
  await mountLoginPage(section(), at(`?authRequest=${encodeURIComponent(authRequest)}`), vi.fn());
  (form().elements.namedItem('login') as HTMLInputElement).value = 'learner';
  (form().elements.namedItem('password') as HTMLInputElement).value = 'test-password';

  const first = new Event('submit', { cancelable: true });
  form().dispatchEvent(first);
  const second = new Event('submit', { cancelable: true });
  form().dispatchEvent(second);

  expect(first.defaultPrevented).toBe(false);
  expect(second.defaultPrevented).toBe(true);
  expect(form().querySelector<HTMLButtonElement>('[type="submit"]')!.textContent).toBe('Signing in…');
});

it.each([
  ['invalid', '', 'Invalid login or password.'],
  ['locked', '&retryAfter=45', 'Too many failed sign-in attempts for this login. Try again in 45 seconds.'],
])('explains a rejected sign-in (%s) and keeps the form', async (error, extra, message) => {
  await mountLoginPage(section(), at(`?authRequest=${encodeURIComponent(authRequest)}&error=${error}${extra}`), vi.fn());

  const alert = document.querySelector<HTMLElement>('[data-login-error]')!;
  expect(alert.hidden).toBe(false);
  expect(alert.textContent).toBe(message);
  expect(alert.getAttribute('role')).toBe('alert');
  expect(form().hidden).toBe(false);
});

it('starts the authorization code flow with PKCE when opened directly', async () => {
  const navigate = vi.fn();
  await mountLoginPage(section(), at('?returnUrl=%2Fprojects%2F'), navigate);

  const url = new URL(navigate.mock.calls[0][0] as string);
  expect(`${url.origin}${url.pathname}`).toBe('http://localhost:5200/connect/authorize');
  expect(url.searchParams.get('client_id')).toBe('cpnucleo-webclient');
  expect(url.searchParams.get('response_type')).toBe('code');
  expect(url.searchParams.get('scope')).toBe('openid profile cpnucleo.api offline_access');
  expect(url.searchParams.get('code_challenge_method')).toBe('S256');
  expect(url.searchParams.get('redirect_uri')).toBe(`${window.location.origin}/signin-callback/`);
  const pending = JSON.parse(sessionStorage.getItem(pendingSignInStorageKey)!);
  expect(pending.state).toBe(url.searchParams.get('state'));
  expect(pending.nonce).toBe(url.searchParams.get('nonce'));
  expect(pending.returnUrl).toBe('/projects/');
  expect(url.searchParams.get('code_challenge')).not.toBe(pending.verifier);
  expect(form().hidden).toBe(true);
});

it('waits for the user after a sign-out in another tab', async () => {
  const navigate = vi.fn();
  await mountLoginPage(section(), at('?signedOut=1'), navigate);

  expect(navigate).not.toHaveBeenCalled();
  expect(document.querySelector('[data-login-status]')?.textContent).toBe('You signed out.');
  const restart = document.querySelector<HTMLButtonElement>('[data-login-restart]')!;
  expect(restart.hidden).toBe(false);
  restart.click();
  await vi.waitFor(() => expect(navigate).toHaveBeenCalledWith(expect.stringContaining('/connect/authorize?')));
});

it('explains a rejected authorization request from its error id', async () => {
  const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(
    JSON.stringify({ error: 'invalid_request', errorDescription: 'code challenge required' }), { status: 200 }));

  await mountLoginPage(section(), at('?errorId=abc'), vi.fn());

  expect(String(fetchMock.mock.calls[0][0])).toBe('http://localhost:5200/api/account/error?errorId=abc');
  expect(document.querySelector('[data-login-error]')?.textContent).toBe('The sign-in request was rejected: code challenge required');
  expect(document.querySelector<HTMLButtonElement>('[data-login-restart]')!.hidden).toBe(false);
});

it('derives the advertised number of work areas from resource metadata', () => {
  expect(document.querySelector('[data-work-area-count]')?.textContent).toBe(String(resourceMetadata.length));
  expect(resourceMetadata).toHaveLength(11);
});

it('explains once why sign-in is needed again after a password change', async () => {
  sessionStorage.setItem(signInNoticeStorageKey, 'credentials-changed');
  await mountLoginPage(section(), at(`?authRequest=${encodeURIComponent(authRequest)}`), vi.fn());
  expect(document.querySelector('[data-login-status]')?.textContent).toBe(signInNotices['credentials-changed']);
  expect(sessionStorage.getItem(signInNoticeStorageKey)).toBeNull();
});

it('never displays stored text that is not a known notice code', async () => {
  sessionStorage.setItem(signInNoticeStorageKey, '<b>Injected</b>');
  await mountLoginPage(section(), at(`?authRequest=${encodeURIComponent(authRequest)}`), vi.fn());
  expect(document.querySelector('[data-login-status]')?.textContent).toBe('');
});
