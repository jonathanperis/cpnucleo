import type { ApiErrorShape } from './types';
import { IDENTITY_API_BASE_URL, IDENTITY_API_ISSUER } from '../config';
import { getLoginRedirectTarget } from '../auth-navigation';

export class ApiError extends Error implements ApiErrorShape {
  status: number;
  details?: unknown;

  constructor(status: number, message: string, details?: unknown) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.details = details;
  }
}

export const tokenStorageKey = 'cpnucleo.jwt';
export const lastActivityStorageKey = 'cpnucleo.lastActivityAt';
export const sessionInactivityTimeoutMs = 15 * 60 * 1000;
export const tokenRefreshLeadMs = 5 * 60 * 1000;
const tokenRefreshCooldownMs = 60 * 1000;

let inactivityTimer: number | undefined;
let refreshTimer: number | undefined;
let refreshInFlight: Promise<boolean> | undefined;
let lastRefreshAttemptAt = 0;

const now = () => Date.now();

const decodeJwtPayload = (token: string): { iss?: unknown; exp?: unknown } | null => {
  const payload = token.split('.')[1];
  if (!payload) return null;

  try {
    const normalized = payload.replace(/-/g, '+').replace(/_/g, '/');
    const padded = normalized.padEnd(normalized.length + ((4 - normalized.length % 4) % 4), '=');
    return JSON.parse(atob(padded)) as { iss?: unknown; exp?: unknown };
  } catch {
    return null;
  }
};

export const tokenWasIssuedByIdentityApi = (token: string, issuer = IDENTITY_API_ISSUER): boolean =>
  decodeJwtPayload(token)?.iss === issuer;

const getTokenExpiresAt = (token: string): number | null => {
  const exp = decodeJwtPayload(token)?.exp;
  return typeof exp === 'number' ? exp * 1000 : null;
};

const tokenHasExpired = (token: string, currentTime = now()) => {
  const expiresAt = getTokenExpiresAt(token);
  return expiresAt !== null && expiresAt <= currentTime;
};

const getLastActivityAt = (): number | null => {
  if (typeof sessionStorage === 'undefined') return null;
  const stored = Number(sessionStorage.getItem(lastActivityStorageKey));
  return Number.isFinite(stored) && stored > 0 ? stored : null;
};

const sessionIsInactive = (currentTime = now()) => {
  const lastActivityAt = getLastActivityAt();
  return lastActivityAt !== null && currentTime - lastActivityAt >= sessionInactivityTimeoutMs;
};

const markSessionActivity = (currentTime = now()): void => {
  if (typeof sessionStorage !== 'undefined') sessionStorage.setItem(lastActivityStorageKey, String(currentTime));
};

export const getStoredToken = (): string | null => {
  if (typeof sessionStorage === 'undefined') return null;
  const token = sessionStorage.getItem(tokenStorageKey);
  if (!token) return null;
  if (tokenWasIssuedByIdentityApi(token) && !tokenHasExpired(token) && !sessionIsInactive()) return token;
  clearStoredToken();
  return null;
};

export const setStoredToken = (token: string): void => {
  if (typeof sessionStorage === 'undefined') return;
  if (tokenWasIssuedByIdentityApi(token) && !tokenHasExpired(token)) {
    sessionStorage.setItem(tokenStorageKey, token);
    markSessionActivity();
    scheduleSessionTimers();
    return;
  }
  clearStoredToken();
};

export const clearStoredToken = (): void => {
  if (typeof sessionStorage !== 'undefined') {
    sessionStorage.removeItem(tokenStorageKey);
    sessionStorage.removeItem(lastActivityStorageKey);
  }
  if (inactivityTimer) clearTimeout(inactivityTimer);
  if (refreshTimer) clearTimeout(refreshTimer);
  inactivityTimer = undefined;
  refreshTimer = undefined;
};

const redirectToLoginForExpiredSession = () => {
  if (typeof window === 'undefined') return;
  if (window.location.pathname === '/login' || window.location.pathname === '/login/') return;
  window.location.assign(getLoginRedirectTarget(window.location));
};

const refreshStoredToken = async (baseUrl = IDENTITY_API_BASE_URL): Promise<boolean> => {
  const token = getStoredToken();
  if (!token) return false;

  if (refreshInFlight) return refreshInFlight;

  refreshInFlight = (async () => {
    let response: Response;
    try {
      response = await fetch(`${baseUrl.replace(/\/$/, '')}/refresh`, {
        method: 'POST',
        headers: {
          Accept: 'application/json',
          Authorization: `Bearer ${token}`,
        },
      });
    } catch {
      return false;
    }

    if (response.status === 401) {
      clearStoredToken();
      redirectToLoginForExpiredSession();
      return false;
    }

    if (!response.ok) return false;

    const body = await parseJson(response) as { token?: unknown } | undefined;
    if (typeof body?.token !== 'string') return false;
    setStoredToken(body.token);
    return true;
  })().finally(() => {
    refreshInFlight = undefined;
  });

  return refreshInFlight;
};

const shouldRefreshToken = (token: string, currentTime = now()) => {
  const expiresAt = getTokenExpiresAt(token);
  return expiresAt !== null && expiresAt - currentTime <= tokenRefreshLeadMs;
};

const refreshTokenIfNeeded = () => {
  const currentTime = now();
  const token = getStoredToken();
  if (!token || sessionIsInactive(currentTime) || !shouldRefreshToken(token, currentTime)) return;
  if (currentTime - lastRefreshAttemptAt < tokenRefreshCooldownMs) return;
  lastRefreshAttemptAt = currentTime;
  void refreshStoredToken();
};

const scheduleSessionTimers = () => {
  if (typeof window === 'undefined') return;

  if (inactivityTimer) clearTimeout(inactivityTimer);
  if (refreshTimer) clearTimeout(refreshTimer);

  const token = getStoredToken();
  if (!token) return;

  const currentTime = now();
  const lastActivityAt = getLastActivityAt() ?? currentTime;
  const inactivityRemaining = Math.max(lastActivityAt + sessionInactivityTimeoutMs - currentTime, 0);
  inactivityTimer = window.setTimeout(() => {
    clearStoredToken();
    redirectToLoginForExpiredSession();
  }, inactivityRemaining);

  const expiresAt = getTokenExpiresAt(token);
  if (expiresAt === null) return;
  const refreshIn = Math.max(expiresAt - tokenRefreshLeadMs - currentTime, 0);
  refreshTimer = window.setTimeout(refreshTokenIfNeeded, refreshIn);
};

export const setupSessionActivityTracking = (): (() => void) => {
  if (typeof window === 'undefined') return () => undefined;

  const activityEvents = ['click', 'keydown', 'mousemove', 'scroll', 'touchstart'] as const;
  const onActivity = () => {
    if (!getStoredToken()) return;
    markSessionActivity();
    scheduleSessionTimers();
    refreshTokenIfNeeded();
  };

  if (getStoredToken() && getLastActivityAt() === null) markSessionActivity();
  scheduleSessionTimers();
  activityEvents.forEach(event => window.addEventListener(event, onActivity, { passive: true }));

  return () => {
    activityEvents.forEach(event => window.removeEventListener(event, onActivity));
    if (inactivityTimer) clearTimeout(inactivityTimer);
    if (refreshTimer) clearTimeout(refreshTimer);
  };
};

const errorMessage = (status: number, body: unknown): string => {
  if (body && typeof body === 'object') {
    const candidate = body as { message?: unknown; title?: unknown; detail?: unknown; errors?: unknown };
    if (typeof candidate.message === 'string') return candidate.message;
    if (typeof candidate.title === 'string') return candidate.title;
    if (typeof candidate.detail === 'string') return candidate.detail;
    if (candidate.errors) return 'Validation failed. Review the highlighted fields.';
  }
  const defaults: Record<number, string> = {
    400: 'The request is invalid.',
    401: 'Your session is missing or expired.',
    404: 'The requested record was not found.',
    409: 'The record could not be changed because it conflicts with current data.',
    429: 'Too many requests. Please wait and try again.',
    500: 'The server returned an unexpected error.',
  };
  return defaults[status] || `Request failed with status ${status}.`;
};

const parseJson = async (response: Response): Promise<unknown> => {
  const text = await response.text();
  if (!text) return undefined;
  try {
    return JSON.parse(text);
  } catch {
    return text;
  }
};

export interface HttpOptions extends RequestInit {
  token?: string | null;
}

export const requestJson = async <T>(url: string, options: HttpOptions = {}): Promise<T> => {
  const token = options.token === undefined ? getStoredToken() : options.token;
  if (token) markSessionActivity();
  const headers = new Headers(options.headers);
  if (!headers.has('Accept')) headers.set('Accept', 'application/json');
  if (options.body && !headers.has('Content-Type')) headers.set('Content-Type', 'application/json');
  if (token) headers.set('Authorization', `Bearer ${token}`);

  let response: Response;
  try {
    response = await fetch(url, { ...options, headers });
  } catch (error) {
    throw new ApiError(0, 'Network error. Please check your connection and try again.', error);
  }
  const body = await parseJson(response);
  if (!response.ok) {
    if (response.status === 401) {
      clearStoredToken();
      redirectToLoginForExpiredSession();
    }
    throw new ApiError(response.status, errorMessage(response.status, body), body);
  }
  refreshTokenIfNeeded();
  return body as T;
};
