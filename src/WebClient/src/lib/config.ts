const PUBLIC_WEB_ORIGIN = 'https://cpnucleo.jonathanperis.tech';
const PUBLIC_WEBAPI_BASE_URL = 'https://api-cpnucleo.jonathanperis.tech/api';
const PUBLIC_IDENTITY_API_BASE_URL = 'https://identity-cpnucleo.jonathanperis.tech/api';
const PUBLIC_IDENTITY_API_ISSUER = 'https://identity-cpnucleo.jonathanperis.tech';

const localhostServicePattern = /^https?:\/\/(localhost|127\.0\.0\.1|\[::1\])(?::\d+)?/i;

const browserOrigin = () => {
  if (typeof window === 'undefined') return undefined;
  return window.location.origin;
};

export const resolveBrowserServiceUrl = (candidate: string | undefined, fallback: string, origin = browserOrigin()): string => {
  if (!candidate) return fallback;
  if (origin === PUBLIC_WEB_ORIGIN && localhostServicePattern.test(candidate)) return fallback;
  return candidate;
};

export const WEBAPI_BASE_URL = resolveBrowserServiceUrl(import.meta.env.PUBLIC_WEBAPI_BASE_URL, PUBLIC_WEBAPI_BASE_URL);
export const IDENTITY_API_BASE_URL = resolveBrowserServiceUrl(import.meta.env.PUBLIC_IDENTITY_API_BASE_URL, PUBLIC_IDENTITY_API_BASE_URL);
export const IDENTITY_API_ISSUER = resolveBrowserServiceUrl(import.meta.env.PUBLIC_IDENTITY_API_ISSUER, PUBLIC_IDENTITY_API_ISSUER);

export const withoutApiSuffix = (baseUrl: string) => baseUrl.replace(/\/api\/?$/, '');
