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
  vi.restoreAllMocks();
});

describe('http client token handling', () => {
  it('stores only tokens emitted by IdentityApi', () => {
    setStoredToken(tokenWithIssuer('https://identity.peris-studio.dev'));
    expect(getStoredToken()).toBeTruthy();

    setStoredToken(tokenWithIssuer('https://evil.test'));
    expect(getStoredToken()).toBeNull();
  });

  it('sends bearer tokens only when they were emitted by IdentityApi', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockImplementation(async () => new Response(JSON.stringify({ ok: true }), { status: 200 }));

    setStoredToken(tokenWithIssuer('https://evil.test'));
    await requestJson('http://example.test');
    expect(new Headers(fetchMock.mock.calls[0][1]?.headers).has('Authorization')).toBe(false);

    setStoredToken(tokenWithIssuer('https://identity.peris-studio.dev'));
    await requestJson('http://example.test');
    expect(new Headers(fetchMock.mock.calls[1][1]?.headers).get('Authorization')).toMatch(/^Bearer /);
  });
});
