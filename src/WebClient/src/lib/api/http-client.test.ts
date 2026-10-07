import { afterEach, describe, expect, it, vi } from 'vitest';
import { ApiError, clearStoredToken, createApiError, getStoredToken, lastActivityStorageKey, parseRetryAfter, requestJson, sessionInactivityTimeoutMs, setStoredToken, setStoredTokens, tokenStorageKey } from './http-client';

const tokenWithPayload = (payload: Record<string, unknown>) => {
  const encode = (value: unknown) => Buffer.from(JSON.stringify(value)).toString('base64url');
  return `${encode({ alg: 'HS256', typ: 'JWT' })}.${encode(payload)}.signature`;
};

const tokenWithIssuer = (issuer: string, extraPayload: Record<string, unknown> = {}) =>
  tokenWithPayload({ iss: issuer, sub: 'user-1', ...extraPayload });

const storage = new Map<string, string>();
Object.defineProperty(globalThis, 'sessionStorage', {
  value: {
    getItem: (key: string) => storage.get(key) ?? null,
    setItem: (key: string, value: string) => storage.set(key, value),
    removeItem: (key: string) => storage.delete(key),
    clear: () => storage.clear(),
  },
  configurable: true,
});

afterEach(() => {
  sessionStorage.clear();
  Reflect.deleteProperty(globalThis, 'window');
  vi.restoreAllMocks();
});

describe('http client token handling', () => {
  it('does not resurrect a logged-out session when an in-flight refresh completes', async () => {
    const issuer = 'https://identity-cpnucleo.jonathanperis.tech';
    setStoredTokens({ accessToken: tokenWithIssuer(issuer, { exp: Math.floor(Date.now() / 1000) + 60 }), refreshToken: 'refresh-1' });
    let complete!: (response: Response) => void;
    const fetchMock = vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(new Response('{}', { status: 200 }))
      .mockImplementationOnce(() => new Promise<Response>(resolve => { complete = resolve; }));
    await requestJson('http://example.test/api');
    expect(fetchMock).toHaveBeenCalledTimes(2);
    clearStoredToken();
    complete(new Response(JSON.stringify({ access_token: tokenWithIssuer(issuer, { exp: Math.floor(Date.now() / 1000) + 1800 }), refresh_token: 'refresh-2' }), { status: 200 }));
    await new Promise(resolve => setImmediate(resolve));
    expect(getStoredToken()).toBeNull();
  });
  it('stores only tokens emitted by IdentityApi', () => {
    setStoredToken(tokenWithIssuer('https://identity-cpnucleo.jonathanperis.tech'));
    expect(getStoredToken()).toBeTruthy();

    setStoredToken(tokenWithIssuer('https://evil.test'));
    expect(getStoredToken()).toBeNull();
  });

  it('sends bearer tokens only when they were emitted by IdentityApi', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockImplementation(async () => new Response(JSON.stringify({ ok: true }), { status: 200 }));

    setStoredToken(tokenWithIssuer('https://evil.test'));
    await requestJson('http://example.test');
    expect(new Headers(fetchMock.mock.calls[0][1]?.headers).has('Authorization')).toBe(false);

    setStoredToken(tokenWithIssuer('https://identity-cpnucleo.jonathanperis.tech'));
    await requestJson('http://example.test');
    expect(new Headers(fetchMock.mock.calls[1][1]?.headers).get('Authorization')).toMatch(/^Bearer /);
  });

  it('omits bearer tokens when a request explicitly disables auth', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockImplementation(async () => new Response(JSON.stringify({ ok: true }), { status: 200 }));

    setStoredToken(tokenWithIssuer('https://identity-cpnucleo.jonathanperis.tech'));
    await requestJson('http://example.test/login', { method: 'POST', token: null });

    expect(new Headers(fetchMock.mock.calls[0][1]?.headers).has('Authorization')).toBe(false);
  });

  it('removes the token after 15 minutes of inactivity', () => {
    vi.spyOn(Date, 'now').mockReturnValue(Date.parse('2026-05-25T12:00:00Z'));
    setStoredToken(tokenWithIssuer('https://identity-cpnucleo.jonathanperis.tech', { exp: Math.floor(Date.now() / 1000) + 1800 }));
    expect(getStoredToken()).toBeTruthy();

    sessionStorage.setItem(lastActivityStorageKey, String(Date.now() - sessionInactivityTimeoutMs - 1));

    expect(getStoredToken()).toBeNull();
    expect(sessionStorage.getItem(tokenStorageKey)).toBeNull();
  });

  it('does not keep already expired 30-minute tokens', () => {
    vi.spyOn(Date, 'now').mockReturnValue(Date.parse('2026-05-25T12:00:00Z'));

    setStoredToken(tokenWithIssuer('https://identity-cpnucleo.jonathanperis.tech', { exp: Math.floor(Date.now() / 1000) - 1 }));

    expect(getStoredToken()).toBeNull();
  });

  it('redirects expired sessions to the canonical trailing-slash login route without leaking the current port', async () => {
    const assign = vi.fn();
    Object.defineProperty(globalThis, 'window', {
      value: {
        location: {
          pathname: '/projects',
          search: '?page=2',
          hash: '',
          origin: 'https://cpnucleo.jonathanperis.tech:5030',
          assign,
        },
      },
      configurable: true,
    });
    vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify({ title: 'Unauthorized' }), { status: 401 }));

    await expect(requestJson('http://example.test')).rejects.toMatchObject({ status: 401 });

    expect(assign).toHaveBeenCalledWith('/login/?returnUrl=%2Fprojects%2F%3Fpage%3D2');
    expect(assign.mock.calls[0][0]).not.toContain('5030');
    expect(assign.mock.calls[0][0]).not.toContain('cpnucleo.jonathanperis.tech');
  });
});

const json = (status: number, body: unknown, headers: Record<string, string> = {}) =>
  new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json', ...headers } });

describe('unified error envelope', () => {
  it('prefers field errors and general errors over the generic summary', () => {
    const error = createApiError(400, {
      statusCode: 400,
      message: 'One or more errors occurred.',
      errors: { Name: ['Name is required.'], endDate: ['End date must be after start date.'], generalErrors: ['The project is archived.'] },
    });
    expect(error.fieldErrors).toEqual({ name: ['Name is required.'], endDate: ['End date must be after start date.'] });
    expect(error.generalErrors).toEqual(['The project is archived.']);
    expect(error.message).toBe('The project is archived. Name is required. End date must be after start date.');
  });

  it('falls back to the server message, then to a status default', () => {
    expect(createApiError(404, { statusCode: 404, message: 'Organization not found.' }).message).toBe('Organization not found.');
    expect(createApiError(500, { statusCode: 500, message: 'An unexpected error occurred.' }).message).toBe('An unexpected error occurred.');
    expect(createApiError(404, undefined).message).toBe('The requested record was not found.');
    expect(createApiError(418, undefined).message).toBe('Request failed with status 418.');
  });

  it('shows the permission message on 403 without ending the session', async () => {
    setStoredToken(tokenWithIssuer('https://identity-cpnucleo.jonathanperis.tech', { exp: Math.floor(Date.now() / 1000) + 1800 }));
    vi.spyOn(globalThis, 'fetch').mockResolvedValue(json(403, { statusCode: 403, message: 'Administrator access is required.' }));
    await expect(requestJson('http://example.test/api/organization', { method: 'POST', body: '{}' }))
      .rejects.toMatchObject({ status: 403, message: 'Administrator access is required.' });
    expect(getStoredToken()).not.toBeNull();
  });

  it('surfaces the server message for conflicts, including active related records', async () => {
    vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(json(409, { statusCode: 409, message: 'The project changed. Reload before saving your changes.' }))
      .mockResolvedValueOnce(json(409, { statusCode: 409, message: 'The organization still has active projects.' }));
    await expect(requestJson('http://example.test/api/project', { method: 'PATCH', body: '{}' }))
      .rejects.toMatchObject({ status: 409, message: 'The project changed. Reload before saving your changes.' });
    await expect(requestJson('http://example.test/api/organization', { method: 'DELETE', body: '{}' }))
      .rejects.toMatchObject({ status: 409, message: 'The organization still has active projects.' });
  });

  it('includes the Retry-After delay in rate-limit messages', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue(json(429, { statusCode: 429, message: 'Too many failed sign-in attempts.' }, { 'Retry-After': '90' }));
    const error = await requestJson('http://example.test/api/login', { method: 'POST', token: null, body: '{}' }).catch(failure => failure as ApiError);
    expect(error).toBeInstanceOf(ApiError);
    expect(error).toMatchObject({ status: 429, retryAfterSeconds: 90, message: 'Too many failed sign-in attempts. Try again in 2 minutes.' });
    expect(createApiError(429, undefined, new Headers({ 'Retry-After': '1' })).message).toBe('Too many requests. Please wait and try again. Try again in 1 second.');
  });

  it('parses Retry-After as seconds or an HTTP date', () => {
    const now = Date.parse('2026-10-06T12:00:00Z');
    expect(parseRetryAfter('30', now)).toBe(30);
    expect(parseRetryAfter('Tue, 06 Oct 2026 12:00:45 GMT', now)).toBe(45);
    expect(parseRetryAfter('soon', now)).toBeUndefined();
    expect(parseRetryAfter(null, now)).toBeUndefined();
  });
});
