import { describe, expect, it } from 'vitest';
import { IDENTITY_API_BASE_URL, WEBAPI_BASE_URL } from './config';

describe('public service URLs', () => {
  it('defaults to public domains for the static demo build', () => {
    expect(WEBAPI_BASE_URL).toBe('https://api-cpnucleo.jonathanperis.tech/api');
    expect(IDENTITY_API_BASE_URL).toBe('https://identity-cpnucleo.jonathanperis.tech/api');
  });

  it('does not fall back to localhost in production bundles', () => {
    expect(WEBAPI_BASE_URL).not.toContain('localhost');
    expect(IDENTITY_API_BASE_URL).not.toContain('localhost');
  });
});
