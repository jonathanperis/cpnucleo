// @vitest-environment jsdom
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { buildAuthorizeUrl, completeSignIn, pendingSignInStorageKey, SignInError } from './oidc';
import { getStoredToken, idTokenStorageKey, refreshTokenStorageKey } from './http-client';

const issuer = 'https://identity-cpnucleo.jonathanperis.tech';
const encode = (value: unknown) => Buffer.from(JSON.stringify(value)).toString('base64url');
const jwt = (payload: Record<string, unknown>) => `${encode({ alg: 'RS256', typ: 'JWT' })}.${encode(payload)}.signature`;
const seconds = (offset: number) => Math.floor(Date.now() / 1000) + offset;

const startSignIn = async (returnUrl = '/projects/') => {
  const url = new URL(await buildAuthorizeUrl(returnUrl));
  return { state: url.searchParams.get('state')!, nonce: url.searchParams.get('nonce')! };
};

const tokenResponse = (nonce: string, overrides: Record<string, unknown> = {}) => new Response(JSON.stringify({
  access_token: jwt({ iss: issuer, sub: 'user-1', exp: seconds(1800), 'cpnucleo:login': 'ana' }),
  refresh_token: 'refresh-1',
  id_token: jwt({ iss: issuer, aud: 'cpnucleo-webclient', nonce, sub: 'user-1', exp: seconds(300) }),
  ...overrides,
}), { status: 200 });

beforeEach(() => sessionStorage.clear());
afterEach(() => vi.restoreAllMocks());

describe('completing a sign-in', () => {
  it('redeems the code with the PKCE verifier and stores the tokens', async () => {
    const { state, nonce } = await startSignIn();
    const verifier = JSON.parse(sessionStorage.getItem(pendingSignInStorageKey)!).verifier;
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(tokenResponse(nonce));

    await expect(completeSignIn(`?code=the-code&state=${state}`)).resolves.toBe('/projects/');

    const [url, init] = fetchMock.mock.calls[0];
    expect(url).toBe('http://localhost:5200/connect/token');
    expect(Object.fromEntries(new URLSearchParams(String(init?.body)))).toEqual({
      grant_type: 'authorization_code', code: 'the-code', redirect_uri: `${window.location.origin}/signin-callback/`,
      client_id: 'cpnucleo-webclient', code_verifier: verifier,
    });
    expect(getStoredToken()).not.toBeNull();
    expect(sessionStorage.getItem(refreshTokenStorageKey)).toBe('refresh-1');
    expect(sessionStorage.getItem(idTokenStorageKey)).not.toBeNull();
    expect(sessionStorage.getItem(pendingSignInStorageKey)).toBeNull();
  });

  it.each([
    ['a different state', (state: string) => `?code=c&state=${state}-forged`],
    ['an error response', () => '?error=access_denied'],
    ['no code', (state: string) => `?state=${state}`],
  ])('rejects %s without calling the token endpoint', async (_, search) => {
    const { state } = await startSignIn();
    const fetchMock = vi.spyOn(globalThis, 'fetch');

    await expect(completeSignIn(search(state))).rejects.toBeInstanceOf(SignInError);
    expect(fetchMock).not.toHaveBeenCalled();
    expect(sessionStorage.getItem(pendingSignInStorageKey)).toBeNull();
  });

  it('rejects a replayed callback once the pending request is consumed', async () => {
    const { state, nonce } = await startSignIn();
    vi.spyOn(globalThis, 'fetch').mockResolvedValue(tokenResponse(nonce));
    await completeSignIn(`?code=c&state=${state}`);

    await expect(completeSignIn(`?code=c&state=${state}`)).rejects.toBeInstanceOf(SignInError);
  });

  it.each([
    ['nonce', (nonce: string) => ({ id_token: jwt({ iss: issuer, aud: 'cpnucleo-webclient', nonce: `${nonce}-other` }) })],
    ['issuer', (nonce: string) => ({ id_token: jwt({ iss: 'https://evil.test', aud: 'cpnucleo-webclient', nonce }) })],
    ['audience', (nonce: string) => ({ id_token: jwt({ iss: issuer, aud: 'another-client', nonce }) })],
  ])('rejects an identity token with the wrong %s and stores nothing', async (_, override) => {
    const { state, nonce } = await startSignIn();
    vi.spyOn(globalThis, 'fetch').mockResolvedValue(tokenResponse(nonce, override(nonce)));

    await expect(completeSignIn(`?code=c&state=${state}`)).rejects.toBeInstanceOf(SignInError);
    expect(getStoredToken()).toBeNull();
  });

  it('never returns into the sign-in pages', async () => {
    const { state, nonce } = await startSignIn('/signin-callback/');
    vi.spyOn(globalThis, 'fetch').mockResolvedValue(tokenResponse(nonce));

    await expect(completeSignIn(`?code=c&state=${state}`)).resolves.toBe('/');
  });
});
