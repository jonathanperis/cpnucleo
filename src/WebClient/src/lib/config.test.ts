import { beforeEach, describe, expect, it, vi } from 'vitest';

const publicEnv = import.meta.env as Record<string, string | undefined>;

const clearPublicServiceEnv = () => {
  delete publicEnv.PUBLIC_WEBAPI_BASE_URL;
  delete publicEnv.PUBLIC_IDENTITY_API_BASE_URL;
  delete publicEnv.PUBLIC_IDENTITY_API_ISSUER;
};

const importConfigWithPublicEnvCleared = async () => {
  clearPublicServiceEnv();
  vi.resetModules();
  return import('./config');
};

describe('public service URLs', () => {
  beforeEach(() => {
    clearPublicServiceEnv();
  });

  it('defaults to isolated local services when build configuration is absent', async () => {
    const { WEBAPI_BASE_URL, IDENTITY_API_BASE_URL } = await importConfigWithPublicEnvCleared();

    expect(WEBAPI_BASE_URL).toBe('http://localhost:5100/api');
    expect(IDENTITY_API_BASE_URL).toBe('http://localhost:5200/api');
  });

  it('uses explicitly configured public URLs for production builds', async () => {
    const { resolveBrowserServiceUrl } = await importConfigWithPublicEnvCleared();
    expect(resolveBrowserServiceUrl('https://api.example.test/api', 'http://localhost:5100/api')).toBe('https://api.example.test/api');
  });

  it('replaces localhost service URLs when the public app is running in the browser', async () => {
    const { resolveBrowserServiceUrl } = await importConfigWithPublicEnvCleared();

    expect(resolveBrowserServiceUrl('http://localhost:5200/api', 'https://identity-cpnucleo.jonathanperis.tech/api', 'https://cpnucleo.jonathanperis.tech'))
      .toBe('https://identity-cpnucleo.jonathanperis.tech/api');
    expect(resolveBrowserServiceUrl('http://localhost:5100/api', 'https://api-cpnucleo.jonathanperis.tech/api', 'http://localhost:5400'))
      .toBe('http://localhost:5100/api');
  });
});
