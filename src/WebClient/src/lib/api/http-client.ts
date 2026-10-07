import type { ApiErrorShape } from './types';
import { IDENTITY_API_BASE_URL, IDENTITY_API_ISSUER, withoutApiSuffix } from '../config';
import { getLoginRedirectTarget } from '../auth-navigation';

export type FieldErrors = Record<string, string[]>;

interface ApiErrorDetails {
  fieldErrors?: FieldErrors;
  generalErrors?: string[];
  retryAfterSeconds?: number;
}

export class ApiError extends Error implements ApiErrorShape {
  status: number;
  details?: unknown;
  /** Field messages keyed by camelCase request property name. */
  fieldErrors: FieldErrors;
  /** Messages that do not belong to a single field (`errors.generalErrors`). */
  generalErrors: string[];
  /** Parsed `Retry-After` for 429 responses. */
  retryAfterSeconds?: number;

  constructor(status: number, message: string, details?: unknown, { fieldErrors = {}, generalErrors = [], retryAfterSeconds }: ApiErrorDetails = {}) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.details = details;
    this.fieldErrors = fieldErrors;
    this.generalErrors = generalErrors;
    this.retryAfterSeconds = retryAfterSeconds;
  }
}

/** The WebClient's OpenID Connect client id at IdentityApi. */
export const oidcClientId = 'cpnucleo-webclient';
export const tokenStorageKey = 'cpnucleo.jwt';
export const refreshTokenStorageKey = 'cpnucleo.refresh';
export const idTokenStorageKey = 'cpnucleo.idToken';
/** The identity token of a session that expired in this tab, kept to end the identity session. */
export const expiredSessionStorageKey = 'cpnucleo.expiredSession';
export const lastActivityStorageKey = 'cpnucleo.lastActivityAt';
export const logoutStorageKey = 'cpnucleo.logout';
export const sessionChannelName = 'cpnucleo.session';
export const sessionInactivityTimeoutMs = 15 * 60 * 1000;
export const tokenRefreshLeadMs = 5 * 60 * 1000;
const tokenRefreshCooldownMs = 60 * 1000;
const activityThrottleMs = 250;

const storedValue = (key: string): string | null => (typeof sessionStorage === 'undefined' ? null : sessionStorage.getItem(key));

let inactivityTimer: ReturnType<typeof setTimeout> | undefined;
let refreshTimer: ReturnType<typeof setTimeout> | undefined;
let refreshInFlight: Promise<boolean> | undefined;
let lastRefreshAttemptAt = 0;

const now = () => Date.now();

type JwtPayload = Record<string, unknown>;

export const decodeJwtPayload = (token: string): JwtPayload | null => {
  const payload = token.split('.')[1];
  if (!payload) return null;

  try {
    const normalized = payload.replace(/-/g, '+').replace(/_/g, '/');
    const padded = normalized.padEnd(normalized.length + ((4 - normalized.length % 4) % 4), '=');
    const decoded = JSON.parse(atob(padded)) as unknown;
    return decoded && typeof decoded === 'object' ? decoded as JwtPayload : null;
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

export const getLastActivityAt = (): number | null => {
  if (typeof sessionStorage === 'undefined') return null;
  const stored = Number(sessionStorage.getItem(lastActivityStorageKey));
  return Number.isFinite(stored) && stored > 0 ? stored : null;
};

const sessionIsInactive = (currentTime = now()) => {
  const lastActivityAt = getLastActivityAt();
  return lastActivityAt !== null && currentTime - lastActivityAt >= sessionInactivityTimeoutMs;
};

/** Records real user activity. API calls, refreshes and stream reconnects must not call this. */
const markSessionActivity = (currentTime = now()): void => {
  if (typeof sessionStorage !== 'undefined') sessionStorage.setItem(lastActivityStorageKey, String(currentTime));
};

export const getStoredToken = (): string | null => {
  if (typeof sessionStorage === 'undefined') return null;
  const token = sessionStorage.getItem(tokenStorageKey);
  if (!token) return null;
  if (tokenWasIssuedByIdentityApi(token) && !tokenHasExpired(token) && !sessionIsInactive()) return token;
  expireStoredSession();
  return null;
};

/** Clears an expired session but remembers its identity token, so the identity session can be ended too. */
const expireStoredSession = () => {
  const idToken = sessionStorage.getItem(idTokenStorageKey);
  clearStoredToken();
  if (idToken) sessionStorage.setItem(expiredSessionStorageKey, idToken);
};

/** True when this tab's session expired (inactivity, lifetime) and the identity session still has to end. */
export const hasExpiredSession = (): boolean => storedValue(expiredSessionStorageKey) !== null;

const storeToken = (token: string, markActivity: boolean): boolean => {
  if (typeof sessionStorage === 'undefined') return false;
  if (tokenWasIssuedByIdentityApi(token) && !tokenHasExpired(token)) {
    sessionStorage.setItem(tokenStorageKey, token);
    if (markActivity || getLastActivityAt() === null) markSessionActivity();
    scheduleSessionTimers();
    return true;
  }
  clearStoredToken();
  return false;
};

/** Stores a token obtained by an explicit sign-in, which counts as user activity. */
export const setStoredToken = (token: string): void => {
  storeToken(token, true);
};

export interface SignInTokens {
  accessToken: string;
  /** One-time refresh token: each refresh returns its successor. */
  refreshToken: string;
  /** Identity token, kept only as the end-session hint. */
  idToken?: string;
}

/** Stores the tokens of a completed sign-in (counts as user activity). False when the access token is unusable. */
export const setStoredTokens = ({ accessToken, refreshToken, idToken }: SignInTokens): boolean => {
  if (typeof sessionStorage === 'undefined') return false;
  sessionStorage.removeItem(expiredSessionStorageKey);
  sessionStorage.setItem(refreshTokenStorageKey, refreshToken);
  if (idToken) sessionStorage.setItem(idTokenStorageKey, idToken);
  return storeToken(accessToken, true);
};


export const clearStoredToken = (): void => {
  if (typeof sessionStorage !== 'undefined') {
    sessionStorage.removeItem(tokenStorageKey);
    sessionStorage.removeItem(refreshTokenStorageKey);
    sessionStorage.removeItem(idTokenStorageKey);
    sessionStorage.removeItem(lastActivityStorageKey);
  }
  if (inactivityTimer) clearTimeout(inactivityTimer);
  if (refreshTimer) clearTimeout(refreshTimer);
  inactivityTimer = undefined;
  refreshTimer = undefined;
};

export interface SessionClaims {
  /** Raw JWT `sub`: the signed-in user's id. */
  sub: string;
  /** `cpnucleo:login`, falling back to `sub`. */
  login: string;
  /** `cpnucleo:admin` == "true". UI gating only: the APIs enforce authorization. */
  isAdmin: boolean;
}

export const getSessionClaims = (token: string | null = getStoredToken()): SessionClaims | null => {
  if (!token) return null;
  const payload = decodeJwtPayload(token);
  const sub = typeof payload?.sub === 'string' ? payload.sub : '';
  if (!sub) return null;
  const login = typeof payload?.['cpnucleo:login'] === 'string' && payload['cpnucleo:login'] ? payload['cpnucleo:login'] : sub;
  const admin = payload?.['cpnucleo:admin'];
  return { sub, login, isAdmin: admin === true || (typeof admin === 'string' && admin.toLowerCase() === 'true') };
};

export const identityOrigin = (baseUrl = IDENTITY_API_BASE_URL) => withoutApiSuffix(baseUrl).replace(/\/$/, '');

/**
 * The identity server's end-session URL. Ending the identity session matters: otherwise the
 * identity cookie would sign the browser straight back in. After sign-out the server returns to
 * the sign-in page, which receives the page to reopen as `state`.
 */
export const buildEndSessionUrl = (idToken: string | null, returnPath: string | null, baseUrl = IDENTITY_API_BASE_URL): string => {
  const parameters = new URLSearchParams();
  if (idToken) {
    parameters.set('id_token_hint', idToken);
    parameters.set('post_logout_redirect_uri', `${window.location.origin}/login/`);
    if (returnPath) parameters.set('state', returnPath);
  }
  const query = parameters.toString();
  return `${identityOrigin(baseUrl)}/connect/endsession${query ? `?${query}` : ''}`;
};

const isLoginPage = () => window.location.pathname === '/login' || window.location.pathname === '/login/';

/** Expired or revoked session: ends the identity session too, then signs in again for this page. */
export const redirectToLoginForExpiredSession = () => {
  if (typeof window === 'undefined') return;
  const idToken = storedValue(idTokenStorageKey) ?? storedValue(expiredSessionStorageKey);
  clearStoredToken();
  sessionStorage.removeItem(expiredSessionStorageKey);
  if (isLoginPage()) return;
  const loginTarget = getLoginRedirectTarget(window.location);
  // Without an identity token this tab never held an identity session: sign in again directly.
  if (!idToken) {
    window.location.assign(loginTarget);
    return;
  }
  const returnPath = new URLSearchParams(loginTarget.split('?')[1] ?? '').get('returnUrl');
  window.location.assign(buildEndSessionUrl(idToken, returnPath));
};

/** Another tab signed out (and ended the identity session): show the signed-out page here. */
const showSignedOut = () => {
  if (typeof window === 'undefined' || isLoginPage()) return;
  window.location.assign('/login/?signedOut=1');
};

const expireSession = () => {
  redirectToLoginForExpiredSession();
};

// One channel object per tab: BroadcastChannel never delivers a message back to the object that
// posted it, so the tab that logs out does not process its own logout notification.
let sessionChannel: BroadcastChannel | null | undefined;
const getSessionChannel = (): BroadcastChannel | null => {
  if (sessionChannel !== undefined) return sessionChannel;
  sessionChannel = typeof BroadcastChannel === 'undefined' ? null : new BroadcastChannel(sessionChannelName);
  return sessionChannel;
};

const broadcastLogout = () => {
  const channel = getSessionChannel();
  if (channel) {
    channel.postMessage({ type: 'logout' });
    return;
  }
  try {
    // `storage` events fire in the other tabs of this origin, never in the writing tab.
    localStorage.setItem(logoutStorageKey, String(now()));
    localStorage.removeItem(logoutStorageKey);
  } catch {
    // Storage can be unavailable (privacy mode); the local logout still applies.
  }
};

/** Clears this tab and asks every other tab of the origin to sign out too. */
export const logout = (): void => {
  clearStoredToken();
  broadcastLogout();
};

/**
 * Explicit sign-out: revokes the refresh token, signs out every tab of this origin and ends the
 * identity session (which revokes the session's other tokens at the API hosts).
 */
export const signOut = async (baseUrl = IDENTITY_API_BASE_URL): Promise<void> => {
  const refreshToken = storedValue(refreshTokenStorageKey);
  const idToken = storedValue(idTokenStorageKey);
  logout();
  if (refreshToken) {
    await fetch(`${identityOrigin(baseUrl)}/connect/revocation`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
      body: new URLSearchParams({ token: refreshToken, token_type_hint: 'refresh_token', client_id: oidcClientId }),
      keepalive: true,
    }).catch(() => undefined);
  }
  window.location.assign(buildEndSessionUrl(idToken, null, baseUrl));
};

export const subscribeToCrossTabLogout = (onLogout: () => void): (() => void) => {
  if (typeof window === 'undefined') return () => undefined;
  const handle = () => {
    clearStoredToken();
    onLogout();
  };
  const channel = getSessionChannel();
  if (channel) {
    const onMessage = (event: MessageEvent) => {
      if ((event.data as { type?: unknown } | null)?.type === 'logout') handle();
    };
    channel.addEventListener('message', onMessage);
    return () => channel.removeEventListener('message', onMessage);
  }
  const onStorage = (event: StorageEvent) => {
    if (event.key === logoutStorageKey && event.newValue) handle();
  };
  window.addEventListener('storage', onStorage);
  return () => window.removeEventListener('storage', onStorage);
};

/** Exchanges the one-time refresh token at the token endpoint (refresh_token grant). */
const refreshStoredToken = async (baseUrl = IDENTITY_API_BASE_URL): Promise<boolean> => {
  const token = getStoredToken();
  const refreshToken = storedValue(refreshTokenStorageKey);
  if (!token || !refreshToken) return false;

  if (refreshInFlight) return refreshInFlight;

  refreshInFlight = (async () => {
    let response: Response;
    try {
      response = await fetch(`${identityOrigin(baseUrl)}/connect/token`, {
        method: 'POST',
        headers: { Accept: 'application/json', 'Content-Type': 'application/x-www-form-urlencoded' },
        body: new URLSearchParams({ grant_type: 'refresh_token', refresh_token: refreshToken, client_id: oidcClientId }),
      });
    } catch {
      return false;
    }

    if (sessionStorage.getItem(tokenStorageKey) !== token) return false;
    if (response.status === 400 || response.status === 401) {
      // invalid_grant: the eight-hour session ended, the account is inactive, the password or login
      // changed, the session was signed out or the token was replayed. All end the session.
      redirectToLoginForExpiredSession();
      return false;
    }

    if (!response.ok) return false;

    const body = await parseJson(response) as { access_token?: unknown; refresh_token?: unknown; id_token?: unknown } | undefined;
    if (typeof body?.access_token !== 'string' || typeof body.refresh_token !== 'string') return false;
    if (getStoredToken() !== token) return false;
    sessionStorage.setItem(refreshTokenStorageKey, body.refresh_token);
    if (typeof body.id_token === 'string') sessionStorage.setItem(idTokenStorageKey, body.id_token);
    // A background refresh keeps the existing last-activity time.
    return storeToken(body.access_token, false);
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

const onInactivityTimeout = () => {
  // Re-check the recorded user activity: the timer may be stale (throttled background tab,
  // activity recorded after it was scheduled), and only real user input extends the session.
  const lastActivityAt = getLastActivityAt();
  if (lastActivityAt !== null && now() - lastActivityAt < sessionInactivityTimeoutMs && getStoredToken()) {
    scheduleSessionTimers();
    return;
  }
  expireSession();
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
  inactivityTimer = setTimeout(onInactivityTimeout, inactivityRemaining);

  const expiresAt = getTokenExpiresAt(token);
  if (expiresAt === null) return;
  const refreshIn = Math.max(expiresAt - tokenRefreshLeadMs - currentTime, 0);
  refreshTimer = setTimeout(refreshTokenIfNeeded, refreshIn);
};

const throttleLeading = (callback: () => void, waitMs: number): (() => void) => {
  let lastCalledAt = 0;
  return () => {
    const currentTime = now();
    if (currentTime - lastCalledAt < waitMs) return;
    lastCalledAt = currentTime;
    callback();
  };
};

export const setupSessionActivityTracking = (): (() => void) => {
  if (typeof window === 'undefined') return () => undefined;

  const highFrequencyActivityEvents = ['mousemove', 'scroll'] as const;
  const lowFrequencyActivityEvents = ['click', 'keydown', 'touchstart'] as const;
  const onActivity = () => {
    if (!getStoredToken()) return;
    markSessionActivity();
    scheduleSessionTimers();
    refreshTokenIfNeeded();
  };
  const onHighFrequencyActivity = throttleLeading(onActivity, activityThrottleMs);

  if (getStoredToken() && getLastActivityAt() === null) markSessionActivity();
  scheduleSessionTimers();
  highFrequencyActivityEvents.forEach(event => window.addEventListener(event, onHighFrequencyActivity, { passive: true }));
  lowFrequencyActivityEvents.forEach(event => window.addEventListener(event, onActivity, { passive: true }));
  const stopCrossTabLogout = subscribeToCrossTabLogout(showSignedOut);

  return () => {
    highFrequencyActivityEvents.forEach(event => window.removeEventListener(event, onHighFrequencyActivity));
    lowFrequencyActivityEvents.forEach(event => window.removeEventListener(event, onActivity));
    stopCrossTabLogout();
    if (inactivityTimer) clearTimeout(inactivityTimer);
    if (refreshTimer) clearTimeout(refreshTimer);
  };
};

const defaultErrorMessages: Record<number, string> = {
  400: 'The request is invalid.',
  401: 'Your session is missing or expired.',
  403: 'You do not have permission to perform this action.',
  404: 'The requested record was not found.',
  409: 'The record could not be changed because it conflicts with current data.',
  429: 'Too many requests. Please wait and try again.',
  500: 'The server returned an unexpected error.',
};

const generalErrorKeys = new Set(['generalErrors', 'GeneralErrors', '']);

const nonEmptyStrings = (value: unknown): string[] =>
  (Array.isArray(value) ? value : [value]).filter((item): item is string => typeof item === 'string' && item.trim() !== '');

const toFieldKey = (key: string) => key.charAt(0).toLowerCase() + key.slice(1);

/** Parses `Retry-After` as delay-seconds or an HTTP date. */
export const parseRetryAfter = (value: string | null | undefined, currentTime = now()): number | undefined => {
  if (!value) return undefined;
  const trimmed = value.trim();
  if (/^\d+$/.test(trimmed)) return Number(trimmed);
  const date = Date.parse(trimmed);
  return Number.isNaN(date) ? undefined : Math.max(0, Math.ceil((date - currentTime) / 1000));
};

const formatRetryAfter = (seconds: number) => {
  if (seconds < 60) return `${seconds} second${seconds === 1 ? '' : 's'}`;
  const minutes = Math.ceil(seconds / 60);
  return `${minutes} minute${minutes === 1 ? '' : 's'}`;
};

/**
 * Builds an ApiError from the unified envelope `{ statusCode, message, errors? }`.
 * Field messages and `generalErrors` win over the generic summary `message`.
 */
export const createApiError = (status: number, body: unknown, headers?: Headers | null): ApiError => {
  const record = body && typeof body === 'object' ? body as { message?: unknown; title?: unknown; detail?: unknown; errors?: unknown } : undefined;
  const fieldErrors: FieldErrors = {};
  const generalErrors: string[] = [];

  if (Array.isArray(record?.errors)) {
    generalErrors.push(...nonEmptyStrings(record.errors));
  } else if (record?.errors && typeof record.errors === 'object') {
    for (const [key, value] of Object.entries(record.errors)) {
      const messages = nonEmptyStrings(value);
      if (messages.length === 0) continue;
      if (generalErrorKeys.has(key)) generalErrors.push(...messages);
      else (fieldErrors[toFieldKey(key)] ??= []).push(...messages);
    }
  }

  const summary = [record?.message, record?.title, record?.detail].find((value): value is string => typeof value === 'string' && value.trim() !== '');
  const detailed = [...generalErrors, ...Object.values(fieldErrors).flat()].join(' ');
  let message = detailed || summary || defaultErrorMessages[status] || `Request failed with status ${status}.`;

  const retryAfterSeconds = status === 429 ? parseRetryAfter(headers?.get('Retry-After')) : undefined;
  if (retryAfterSeconds !== undefined) message = `${message.replace(/\s*$/, '')} Try again in ${formatRetryAfter(retryAfterSeconds)}.`;

  return new ApiError(status, message, body, { fieldErrors, generalErrors, retryAfterSeconds });
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

/**
 * Converts a non-2xx response into an ApiError. A 401 ends the session (clear + redirect to
 * login); 403/409/429 never sign the user out.
 */
export const readErrorResponse = async (response: Response): Promise<ApiError> => {
  const body = await parseJson(response).catch(() => undefined);
  if (response.status === 401) expireSession();
  return createApiError(response.status, body, response.headers);
};

export interface HttpOptions extends RequestInit {
  token?: string | null;
}

export const requestJson = async <T>(url: string, options: HttpOptions = {}): Promise<T> => {
  const token = options.token === undefined ? getStoredToken() : options.token;
  const headers = new Headers(options.headers);
  if (!headers.has('Accept')) headers.set('Accept', 'application/json');
  if (options.body && !headers.has('Content-Type')) headers.set('Content-Type', 'application/json');
  if (token) headers.set('Authorization', `Bearer ${token}`);

  let response: Response;
  try {
    response = await fetch(url, { ...options, headers });
  } catch (error) {
    if (options.signal?.aborted) throw error;
    throw new ApiError(0, 'Network error. Please check your connection and try again.', error);
  }
  if (!response.ok) throw await readErrorResponse(response);
  const body = await parseJson(response);
  if (token) refreshTokenIfNeeded();
  return body as T;
};
