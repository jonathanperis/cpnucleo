import type { ApiErrorShape } from './types';
import { IDENTITY_API_ISSUER } from '../config';

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

const decodeJwtPayload = (token: string): { iss?: unknown } | null => {
  const payload = token.split('.')[1];
  if (!payload) return null;

  try {
    const normalized = payload.replace(/-/g, '+').replace(/_/g, '/');
    const padded = normalized.padEnd(normalized.length + ((4 - normalized.length % 4) % 4), '=');
    return JSON.parse(atob(padded)) as { iss?: unknown };
  } catch {
    return null;
  }
};

export const tokenWasIssuedByIdentityApi = (token: string, issuer = IDENTITY_API_ISSUER): boolean =>
  decodeJwtPayload(token)?.iss === issuer;

export const getStoredToken = (): string | null => {
  if (typeof sessionStorage === 'undefined') return null;
  const token = sessionStorage.getItem(tokenStorageKey);
  if (!token) return null;
  if (tokenWasIssuedByIdentityApi(token)) return token;
  sessionStorage.removeItem(tokenStorageKey);
  return null;
};

export const setStoredToken = (token: string): void => {
  if (typeof sessionStorage === 'undefined') return;
  if (tokenWasIssuedByIdentityApi(token)) {
    sessionStorage.setItem(tokenStorageKey, token);
    return;
  }
  sessionStorage.removeItem(tokenStorageKey);
};

export const clearStoredToken = (): void => {
  if (typeof sessionStorage !== 'undefined') sessionStorage.removeItem(tokenStorageKey);
};

const redirectToLoginForExpiredSession = () => {
  if (typeof window === 'undefined') return;
  if (window.location.pathname === '/login') return;
  const returnUrl = `${window.location.pathname}${window.location.search}${window.location.hash}`;
  window.location.assign(`/login?returnUrl=${encodeURIComponent(returnUrl)}`);
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
  const token = options.token ?? getStoredToken();
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
  return body as T;
};
