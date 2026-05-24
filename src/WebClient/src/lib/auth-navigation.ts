export const getLoginPath = () => '/login/';

type LocationLike = Pick<Location, 'pathname' | 'search'> & { hash?: string; origin?: string };

const canonicalStaticRoutes = new Set([
  '/login',
  '/organizations',
  '/projects',
  '/assignments',
  '/assignment-types',
  '/impediments',
  '/assignment-impediments',
  '/appointments',
  '/workflows',
  '/users',
  '/user-assignments',
  '/user-projects',
  '/api-health',
]);

export const canonicalizeStaticRoute = (pathname: string): string => {
  const normalized = pathname.replace(/\/$/, '') || '/';
  return canonicalStaticRoutes.has(normalized) ? `${normalized}/` : pathname;
};

export const getLoginRedirectTarget = ({ pathname, search, hash = '' }: LocationLike): string => {
  const loginPath = getLoginPath();
  const current = `${canonicalizeStaticRoute(pathname)}${search}${hash}`;
  const isLoginPage = pathname === '/login' || pathname === loginPath;
  const returnUrl = current && current !== '/' && !isLoginPage
    ? `?returnUrl=${encodeURIComponent(current)}`
    : '';

  return `${loginPath}${returnUrl}`;
};

export const getPostLoginRedirectTarget = (returnUrl: string | null): string => {
  if (!returnUrl || !returnUrl.startsWith('/') || returnUrl.startsWith('//')) {
    return '/';
  }

  const parsed = new URL(returnUrl, 'https://cpnucleo.local');
  return `${canonicalizeStaticRoute(parsed.pathname)}${parsed.search}${parsed.hash}`;
};
