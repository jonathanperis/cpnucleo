export const getLoginPath = () => '/login/';

type LocationLike = Pick<Location, 'pathname' | 'search'> & { origin?: string };

export const getLoginRedirectTarget = ({ pathname, search }: LocationLike): string => {
  const loginPath = getLoginPath();
  const current = `${pathname}${search}`;
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

  return returnUrl;
};
