import { afterEach, describe, expect, it, vi } from 'vitest';
import { getStoredToken, lastActivityStorageKey, requestJson, sessionInactivityTimeoutMs, setStoredToken, tokenStorageKey } from './http-client';

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
