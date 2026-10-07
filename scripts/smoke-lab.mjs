import assert from 'node:assert/strict';
import { createHash, randomBytes, randomUUID } from 'node:crypto';

// Lab smoke over real HTTP: the WebClient's OpenID Connect sign-in (authorization code + PKCE)
// against IdentityApi, then authenticated REST calls, refresh token rotation and sign-out.
const api = new URL(process.env.LAB_API_URL ?? 'http://localhost:5100/api/');
const identityApi = new URL(process.env.LAB_IDENTITY_URL ?? 'http://localhost:5200/api/');
const webOrigin = process.env.LAB_WEB_ORIGIN ?? 'http://localhost:5400';
for (const url of [api, identityApi, new URL(webOrigin)]) {
  assert(['localhost', '127.0.0.1', '[::1]'].includes(url.hostname), 'Lab smoke accepts loopback URLs only.');
}
const identity = new URL('/', identityApi);
const clientId = 'cpnucleo-webclient';
const redirectUri = `${webOrigin}/signin-callback/`;

// A minimal browser: one cookie jar for the identity host, redirects handled step by step.
const cookies = new Map();
const browse = async (url, init = {}) => {
  const response = await fetch(url, {
    ...init,
    redirect: 'manual',
    headers: { ...init.headers, Cookie: [...cookies].map(([name, value]) => `${name}=${value}`).join('; ') },
  });
  for (const cookie of response.headers.getSetCookie()) {
    const [pair] = cookie.split(';');
    const [name, ...value] = pair.split('=');
    if (/expires=Thu, 01 Jan 1970/i.test(cookie)) cookies.delete(name.trim());
    else cookies.set(name.trim(), value.join('='));
  }
  return response;
};
const location = response => new URL(response.headers.get('location'), identity);
const tokenRequest = async form => browse(new URL('/connect/token', identity), {
  method: 'POST',
  headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
  body: new URLSearchParams({ client_id: clientId, ...form }),
});

// 1. Authorization request with PKCE: without a session the server sends the browser to the sign-in page.
const verifier = randomBytes(32).toString('base64url');
const authorize = new URL('/connect/authorize', identity);
authorize.search = new URLSearchParams({
  client_id: clientId, response_type: 'code', scope: 'openid profile cpnucleo.api offline_access',
  redirect_uri: redirectUri, state: randomUUID(), nonce: randomUUID(),
  code_challenge: createHash('sha256').update(verifier).digest('base64url'), code_challenge_method: 'S256',
}).toString();
// IdentityApi's login step picks the sign-in page of the WebClient origin that started the request.
const loginStep = location(await browse(authorize));
assert.equal(`${loginStep.origin}${loginStep.pathname}`, new URL('/api/account/login-page', identity).href, 'Authorize must go through the IdentityApi sign-in step.');
const signInPage = location(await browse(loginStep));
assert.equal(`${signInPage.origin}${signInPage.pathname}`, `${webOrigin}/login/`, 'Authorize must send the browser to the WebClient sign-in page.');

// 2. The sign-in form post, as the WebClient page sends it.
const signedIn = await browse(new URL('/api/account/login', identity), {
  method: 'POST',
  headers: { 'Content-Type': 'application/x-www-form-urlencoded', Origin: webOrigin },
  body: new URLSearchParams({
    login: 'demo@cpnucleo.local',
    password: process.env.LAB_PASSWORD ?? 'LocalLearning@123',
    authRequest: signInPage.searchParams.get('authRequest'),
  }),
});
assert.equal(signedIn.status, 302, 'Local sign-in must succeed after explicit lab seeding.');
assert.equal(location(signedIn).searchParams.get('error'), null, 'Local sign-in must succeed after explicit lab seeding.');

// 3. Back to the authorization request, which issues the code to the WebClient callback.
const callback = location(await browse(location(signedIn)));
assert.equal(`${callback.origin}${callback.pathname}`, redirectUri, 'The code must reach the registered redirect URI.');
const issued = await tokenRequest({
  grant_type: 'authorization_code', code: callback.searchParams.get('code'), redirect_uri: redirectUri, code_verifier: verifier,
});
assert.equal(issued.status, 200, 'Code redemption failed.');
const tokens = await issued.json();
assert.equal(typeof tokens.access_token, 'string');

const call = (path, method = 'GET', body) => fetch(new URL(path, api), {
  method,
  headers: { Authorization: `Bearer ${tokens.access_token}`, ...(body ? { 'Content-Type': 'application/json' } : {}) },
  redirect: 'error', ...(body ? { body: JSON.stringify(body) } : {}),
});
const organizationResponse = await call('organizations?pageSize=1');
assert.equal(organizationResponse.status, 200, 'Authenticated organization listing failed.');
const organizations = await organizationResponse.json();
const organizationId = organizations.result.data[0]?.id;
assert(organizationId, 'The tiny seed must contain an organization.');
let id;
try {
  const created = await call('project', 'POST', { name: `Smoke-${randomUUID()}`, organizationId });
  assert.equal(created.status, 200, 'Project creation failed.');
  id = (await created.json()).project.id;
  const original = (await (await call(`project?id=${id}`)).json()).project;
  const change = { id, organizationId, name: 'Smoke updated', expectedVersion: original.updatedAt ?? original.createdAt };
  assert.equal((await call('project', 'PATCH', change)).status, 200, 'Versioned edit failed.');
  assert.equal((await call('project', 'PATCH', change)).status, 409, 'Stale edit must conflict.');
  assert.equal((await call('users?pageSize=1')).status, 200, 'Local administrator must manage users.');

  const refreshed = await tokenRequest({ grant_type: 'refresh_token', refresh_token: tokens.refresh_token });
  assert.equal(refreshed.status, 200, 'Active session refresh failed.');
  const rotated = await refreshed.json();
  assert.notEqual(rotated.refresh_token, tokens.refresh_token, 'Refresh tokens must be one-time use.');
  Object.assign(tokens, rotated);
} finally {
  if (id) {
    assert.equal((await call('project', 'DELETE', { ids: [id] })).status, 200, 'Smoke-owned project cleanup failed.');
    assert.equal((await call(`project?id=${id}`)).status, 404, 'Removed project remains visible.');
  }
}

// 4. Sign out through the end-session endpoint: the session's refresh token stops working.
const endSession = new URL('/connect/endsession', identity);
endSession.search = new URLSearchParams({ id_token_hint: tokens.id_token, post_logout_redirect_uri: `${webOrigin}/login/` }).toString();
const logout = await browse(location(await browse(endSession)));
assert.equal(location(logout).href, `${webOrigin}/login/`, 'Sign-out must return to the WebClient sign-in page.');
const afterSignOut = await tokenRequest({ grant_type: 'refresh_token', refresh_token: tokens.refresh_token });
assert.equal(afterSignOut.status, 400, 'Signing out must revoke the session\'s refresh tokens.');
console.log('Local HTTP smoke passed: OIDC sign-in (code + PKCE), admin access, create/read/update, conflict, refresh rotation and sign-out.');
