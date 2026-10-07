import { IDENTITY_API_BASE_URL, IDENTITY_API_ISSUER } from '../config';
import { getPostLoginRedirectTarget } from '../auth-navigation';
import { decodeJwtPayload, identityOrigin, oidcClientId, setStoredTokens } from './http-client';

export { identityOrigin, oidcClientId };

/**
 * The WebClient as an OpenID Connect public client of IdentityApi: authorization code flow with
 * PKCE (S256), state and nonce. The pending request lives in this tab's sessionStorage until the
 * callback page redeems the code; tokens never appear in URLs.
 */
export const oidcScopes = 'openid profile cpnucleo.api offline_access';
export const pendingSignInStorageKey = 'cpnucleo.oidc.pending';
const pendingSignInLifetimeMs = 10 * 60 * 1000;

interface PendingSignIn {
  state: string;
  nonce: string;
  verifier: string;
  returnUrl: string;
  createdAt: number;
}

export class SignInError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'SignInError';
  }
}

export const signInCallbackUrl = (origin = window.location.origin) => `${origin}/signin-callback/`;
export const postLogoutRedirectUrl = (origin = window.location.origin) => `${origin}/login/`;
export const accountLoginUrl = (baseUrl = IDENTITY_API_BASE_URL) => `${identityOrigin(baseUrl)}/api/account/login`;
export const tokenEndpoint = (baseUrl = IDENTITY_API_BASE_URL) => `${identityOrigin(baseUrl)}/connect/token`;
export const revocationEndpoint = (baseUrl = IDENTITY_API_BASE_URL) => `${identityOrigin(baseUrl)}/connect/revocation`;

const base64Url = (bytes: Uint8Array) =>
  btoa(String.fromCharCode(...bytes)).replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');

const randomValue = (byteLength = 32) => base64Url(crypto.getRandomValues(new Uint8Array(byteLength)));

export const createPkce = async () => {
  const verifier = randomValue(32);
  const digest = await crypto.subtle.digest('SHA-256', new TextEncoder().encode(verifier));
  return { verifier, challenge: base64Url(new Uint8Array(digest)) };
};

/** Starts a sign-in: remembers PKCE, state and nonce in this tab and returns the authorize URL. */
export const buildAuthorizeUrl = async (returnUrl: string | null, baseUrl = IDENTITY_API_BASE_URL): Promise<string> => {
  const pkce = await createPkce();
  const pending: PendingSignIn = {
    state: randomValue(16),
    nonce: randomValue(16),
    verifier: pkce.verifier,
    returnUrl: getPostLoginRedirectTarget(returnUrl),
    createdAt: Date.now(),
  };
  sessionStorage.setItem(pendingSignInStorageKey, JSON.stringify(pending));
  const parameters = new URLSearchParams({
    client_id: oidcClientId,
    response_type: 'code',
    scope: oidcScopes,
    redirect_uri: signInCallbackUrl(),
    state: pending.state,
    nonce: pending.nonce,
    code_challenge: pkce.challenge,
    code_challenge_method: 'S256',
  });
  return `${identityOrigin(baseUrl)}/connect/authorize?${parameters}`;
};

const readPendingSignIn = (): PendingSignIn | null => {
  const raw = sessionStorage.getItem(pendingSignInStorageKey);
  sessionStorage.removeItem(pendingSignInStorageKey);
  if (!raw) return null;
  try {
    const value = JSON.parse(raw) as Partial<PendingSignIn>;
    return typeof value.state === 'string' && typeof value.nonce === 'string' && typeof value.verifier === 'string'
      && typeof value.returnUrl === 'string' && typeof value.createdAt === 'number' ? value as PendingSignIn : null;
  } catch {
    return null;
  }
};

const audienceIncludes = (audience: unknown, clientId: string) =>
  audience === clientId || (Array.isArray(audience) && audience.includes(clientId));

/**
 * Completes the sign-in on the callback page: checks state, redeems the code with the PKCE
 * verifier and checks the identity token's issuer, audience and nonce. Returns the page to open.
 */
export const completeSignIn = async (search: string, baseUrl = IDENTITY_API_BASE_URL): Promise<string> => {
  const parameters = new URLSearchParams(search);
  const pending = readPendingSignIn();
  if (parameters.get('error')) throw new SignInError('The sign-in was not completed. Sign in again.');
  if (!pending || pending.state !== parameters.get('state') || Date.now() - pending.createdAt > pendingSignInLifetimeMs) {
    throw new SignInError('This sign-in is no longer valid. Sign in again.');
  }
  const code = parameters.get('code');
  if (!code) throw new SignInError('The sign-in response is incomplete. Sign in again.');

  let response: Response;
  try {
    response = await fetch(tokenEndpoint(baseUrl), {
      method: 'POST',
      headers: { Accept: 'application/json', 'Content-Type': 'application/x-www-form-urlencoded' },
      body: new URLSearchParams({
        grant_type: 'authorization_code',
        code,
        redirect_uri: signInCallbackUrl(),
        client_id: oidcClientId,
        code_verifier: pending.verifier,
      }),
    });
  } catch {
    throw new SignInError('Network error. Check your connection and sign in again.');
  }
  const body = await response.json().catch(() => undefined) as Record<string, unknown> | undefined;
  const accessToken = body?.access_token;
  const refreshToken = body?.refresh_token;
  const idToken = body?.id_token;
  if (!response.ok || typeof accessToken !== 'string' || typeof refreshToken !== 'string' || typeof idToken !== 'string') {
    throw new SignInError('The sign-in could not be completed. Sign in again.');
  }

  const identity = decodeJwtPayload(idToken);
  if (identity?.iss !== IDENTITY_API_ISSUER || !audienceIncludes(identity.aud, oidcClientId) || identity.nonce !== pending.nonce) {
    throw new SignInError('The sign-in response could not be verified. Sign in again.');
  }
  if (!setStoredTokens({ accessToken, refreshToken, idToken })) {
    throw new SignInError('The sign-in response could not be verified. Sign in again.');
  }
  return pending.returnUrl;
};

/** Explains a rejected authorization request (the identity server redirects with an error id). */
export const describeSignInError = async (errorId: string, baseUrl = IDENTITY_API_BASE_URL): Promise<string> => {
  const fallback = 'The sign-in request was rejected. Start again.';
  try {
    const response = await fetch(`${identityOrigin(baseUrl)}/api/account/error?errorId=${encodeURIComponent(errorId)}`, { headers: { Accept: 'application/json' } });
    if (!response.ok) return fallback;
    const body = await response.json() as { errorDescription?: unknown };
    return typeof body.errorDescription === 'string' && body.errorDescription.trim()
      ? `The sign-in request was rejected: ${body.errorDescription.trim()}`
      : fallback;
  } catch {
    return fallback;
  }
};
