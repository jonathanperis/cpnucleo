import { beforeEach, describe, expect, it } from 'vitest';

const publicEnv = import.meta.env as Record<string, string | undefined>;

const clearPublicServiceEnv = () => {
  delete publicEnv.PUBLIC_WEBAPI_BASE_URL;
  delete publicEnv.PUBLIC_IDENTITY_API_BASE_URL;
  delete publicEnv.PUBLIC_IDENTITY_API_ISSUER;
};

const importConfigWithPublicEnvCleared = async () => {
  clearPublicServiceEnv();
  return import(`./config?defaults=${Date.now()}-${Math.random()}`);
};

describe('public service URLs', () => {
  beforeEach(() => {
    clearPublicServiceEnv();
  });

  it('defaults to public domains for the static demo build', async () => {
    const { WEBAPI_BASE_URL, IDENTITY_API_BASE_URL } = await importConfigWithPublicEnvCleared();

    expect(WEBAPI_BASE_URL).toBe('https://api-cpnucleo.jonathanperis.tech/api');
    expect(IDENTITY_API_BASE_URL).toBe('https://identity-cpnucleo.jonathanperis.tech/api');
  });

  it('does not fall back to localhost in production bundles', async () => {
    const { WEBAPI_BASE_URL, IDENTITY_API_BASE_URL } = await importConfigWithPublicEnvCleared();

    expect(WEBAPI_BASE_URL).not.toContain('localhost');
    expect(IDENTITY_API_BASE_URL).not.toContain('localhost');
  });

  it('replaces localhost service URLs when the public app is running in the browser', async () => {
    const { resolveBrowserServiceUrl } = await importConfigWithPublicEnvCleared();

    expect(resolveBrowserServiceUrl('http://localhost:5200/api', 'https://identity-cpnucleo.jonathanperis.tech/api', 'https://cpnucleo.jonathanperis.tech'))
      .toBe('https://identity-cpnucleo.jonathanperis.tech/api');
    expect(resolveBrowserServiceUrl('http://localhost:5100/api', 'https://api-cpnucleo.jonathanperis.tech/api', 'http://localhost:5400'))
      .toBe('http://localhost:5100/api');
  });
});
