import { afterEach, describe, expect, it, vi } from 'vitest';
import { getStoredToken, requestJson, setStoredToken } from './http-client';

const tokenWithIssuer = (issuer: string) => {
  const encode = (value: unknown) => Buffer.from(JSON.stringify(value)).toString('base64url');
  return `${encode({ alg: 'HS256', typ: 'JWT' })}.${encode({ iss: issuer, sub: 'user-1' })}.signature`;
};

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
