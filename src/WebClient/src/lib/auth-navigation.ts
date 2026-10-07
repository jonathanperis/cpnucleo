export const getLoginPath = () => '/login/';

type LocationLike = Pick<Location, 'pathname' | 'search'> & { hash?: string; origin?: string };

const canonicalStaticRoutes = new Set([
  '/login',
  '/signin-callback',
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
  '/board',
  '/calendar',
  '/account',
  '/projects/view',
  '/assignments/view',
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
  const path = canonicalizeStaticRoute(parsed.pathname);
  // Never return into the sign-in pages themselves.
  if (path === '/login/' || path === '/signin-callback/') return '/';
  return `${path}${parsed.search}${parsed.hash}`;
};
