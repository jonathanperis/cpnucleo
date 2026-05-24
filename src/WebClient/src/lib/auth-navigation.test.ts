import { describe, expect, it } from 'vitest';
import { getLoginPath, getLoginRedirectTarget, getPostLoginRedirectTarget } from './auth-navigation';

describe('auth navigation', () => {
  it('redirects protected pages to the trailing-slash login route without leaking the internal container port', () => {
    const target = getLoginRedirectTarget({ pathname: '/', search: '', origin: 'https://cpnucleo.jonathanperis.tech:5030' });

    expect(target).toBe('/login/');
    expect(target).not.toContain('5030');
    expect(target).not.toContain('cpnucleo.jonathanperis.tech');
  });

  it('keeps protected return urls relative', () => {
    expect(getLoginRedirectTarget({ pathname: '/projects', search: '?page=2', origin: 'https://cpnucleo.jonathanperis.tech:5030' }))
      .toBe('/login/?returnUrl=%2Fprojects%3Fpage%3D2');
  });

  it('rejects absolute and protocol-relative post-login return urls', () => {
    expect(getPostLoginRedirectTarget('https://evil.test/projects')).toBe('/');
    expect(getPostLoginRedirectTarget('//evil.test/projects')).toBe('/');
    expect(getPostLoginRedirectTarget('/projects')).toBe('/projects');
  });

  it('uses the canonical standalone login path', () => {
    expect(getLoginPath()).toBe('/login/');
  });
});
