import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';

type HttpClientModule = typeof import('./http-client');

const issuer = 'https://identity-cpnucleo.jonathanperis.tech';
const encode = (value: unknown) => Buffer.from(JSON.stringify(value)).toString('base64url');
const token = (payload: Record<string, unknown>) => `${encode({ alg: 'HS256', typ: 'JWT' })}.${encode({ iss: issuer, sub: 'user-1', ...payload })}.signature`;
const secondsFromNow = (seconds: number) => Math.floor(Date.now() / 1000) + seconds;

const createStorage = () => {
  const values = new Map<string, string>();
  return {
    getItem: vi.fn((key: string) => values.get(key) ?? null),
    setItem: vi.fn((key: string, value: string) => { values.set(key, value); }),
    removeItem: vi.fn((key: string) => { values.delete(key); }),
    clear: () => values.clear(),
  };
};

/** Minimal window: an EventTarget with a location whose assign() we can observe. */
const createWindow = (pathname = '/projects/') => Object.assign(new EventTarget(), {
  location: { pathname, search: '', hash: '', origin: 'http://localhost:5030', assign: vi.fn() },
});

let session: ReturnType<typeof createStorage>;
let local: ReturnType<typeof createStorage>;
let win: ReturnType<typeof createWindow>;
const originalBroadcastChannel = globalThis.BroadcastChannel;
// Each test's "tabs" must not outlive it: a leftover channel would answer the next test's tabs.
const openChannels: BroadcastChannel[] = [];

/** Each import is an isolated "tab" (own timers, own BroadcastChannel object). */
const loadTab = async (): Promise<HttpClientModule> => {
  vi.resetModules();
  return import('./http-client');
};

beforeEach(() => {
  session = createStorage();
  local = createStorage();
  win = createWindow();
  Object.defineProperty(globalThis, 'sessionStorage', { value: session, configurable: true });
  Object.defineProperty(globalThis, 'localStorage', { value: local, configurable: true });
  Object.defineProperty(globalThis, 'window', { value: win, configurable: true });
  Object.defineProperty(globalThis, 'BroadcastChannel', {
    value: class extends originalBroadcastChannel {
      constructor(name: string) { super(name); openChannels.push(this); }
    },
    configurable: true,
    writable: true,
  });
});

afterEach(() => {
  openChannels.splice(0).forEach(channel => channel.close());
  vi.useRealTimers();
  vi.restoreAllMocks();
  Reflect.deleteProperty(globalThis, 'window');
  Object.defineProperty(globalThis, 'BroadcastChannel', { value: originalBroadcastChannel, configurable: true, writable: true });
});

describe('session inactivity', () => {
  it('only real user input extends the session; the timer re-checks activity before logging out', async () => {
    vi.useFakeTimers({ now: Date.parse('2026-10-06T12:00:00Z') });
    const http = await loadTab();
    http.setStoredToken(token({ exp: secondsFromNow(8 * 60 * 60) }));
    const stop = http.setupSessionActivityTracking();

    await vi.advanceTimersByTimeAsync(10 * 60 * 1000);
    win.dispatchEvent(new Event('keydown'));
    await vi.advanceTimersByTimeAsync(10 * 60 * 1000);
    expect(http.getStoredToken()).not.toBeNull();
    expect(win.location.assign).not.toHaveBeenCalled();

    await vi.advanceTimersByTimeAsync(5 * 60 * 1000 + 1);
    expect(session.getItem(http.tokenStorageKey)).toBeNull();
    expect(win.location.assign).toHaveBeenCalledWith('/login/?returnUrl=%2Fprojects%2F');
    stop();
  });

  it('does not count background API calls as activity', async () => {
    vi.useFakeTimers({ now: Date.parse('2026-10-06T12:00:00Z') });
    const http = await loadTab();
    http.setStoredToken(token({ exp: secondsFromNow(8 * 60 * 60) }));
    const signedInAt = http.getLastActivityAt();
    vi.spyOn(globalThis, 'fetch').mockImplementation(async () => new Response('{"items":[]}', { status: 200 }));
    const stop = http.setupSessionActivityTracking();

    for (let minute = 0; minute < 14; minute += 1) {
      await vi.advanceTimersByTimeAsync(60 * 1000);
      await http.requestJson('http://localhost:5100/api/projects');
    }
    expect(http.getLastActivityAt()).toBe(signedInAt);

    await vi.advanceTimersByTimeAsync(60 * 1000 + 1);
    expect(http.getStoredToken()).toBeNull();
    expect(win.location.assign).toHaveBeenCalledTimes(1);
    stop();
  });

  it('rotates the one-time refresh token in the background and keeps the last user activity', async () => {
    vi.useFakeTimers({ now: Date.parse('2026-10-06T12:00:00Z') });
    const http = await loadTab();
    const refreshed = token({ exp: secondsFromNow(30 * 60), jti: 'refreshed' });
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockImplementation(async input => (String(input).endsWith('/connect/token')
      ? new Response(JSON.stringify({ access_token: refreshed, refresh_token: 'refresh-2' }), { status: 200 })
      : new Response('{}', { status: 200 })));
    http.setStoredTokens({ accessToken: token({ exp: secondsFromNow(4 * 60) }), refreshToken: 'refresh-1', idToken: 'id-1' });
    const signedInAt = http.getLastActivityAt();

    // The refresh timer fires inside the five-minute lead window without any user input.
    await vi.advanceTimersByTimeAsync(30 * 1000);

    await vi.waitFor(() => expect(session.getItem(http.tokenStorageKey)).toBe(refreshed));
    const refreshes = fetchMock.mock.calls.filter(([input]) => String(input).endsWith('/connect/token'));
    expect(refreshes).toHaveLength(1);
    const body = new URLSearchParams(String(refreshes[0][1]?.body));
    expect(Object.fromEntries(body)).toEqual({ grant_type: 'refresh_token', refresh_token: 'refresh-1', client_id: 'cpnucleo-webclient' });
    expect(session.getItem(http.refreshTokenStorageKey)).toBe('refresh-2');
    expect(http.getLastActivityAt()).toBe(signedInAt);
  });

  it('ends the identity session when a refresh is rejected (password change, sign-out, replay)', async () => {
    const http = await loadTab();
    http.setStoredTokens({ accessToken: token({ exp: secondsFromNow(60) }), refreshToken: 'refresh-1', idToken: 'id-1' });
    vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(new Response('{}', { status: 200 }))
      .mockResolvedValueOnce(new Response(JSON.stringify({ error: 'invalid_grant' }), { status: 400 }));

    await http.requestJson('http://localhost:5100/api/projects');
    await vi.waitFor(() => expect(win.location.assign).toHaveBeenCalledWith(
      'http://localhost:5200/connect/endsession?id_token_hint=id-1&post_logout_redirect_uri=http%3A%2F%2Flocalhost%3A5030%2Flogin%2F&state=%2Fprojects%2F'));
    expect(http.getStoredToken()).toBeNull();
    expect(session.getItem(http.refreshTokenStorageKey)).toBeNull();
  });

  it('remembers an expired session so the guard can end the identity session too', async () => {
    vi.useFakeTimers({ now: Date.parse('2026-10-06T12:00:00Z') });
    const http = await loadTab();
    http.setStoredTokens({ accessToken: token({ exp: secondsFromNow(8 * 60 * 60) }), refreshToken: 'refresh-1', idToken: 'id-1' });

    vi.setSystemTime(Date.now() + http.sessionInactivityTimeoutMs + 1000);

    expect(http.getStoredToken()).toBeNull();
    expect(http.hasExpiredSession()).toBe(true);
    http.redirectToLoginForExpiredSession();
    expect(win.location.assign).toHaveBeenCalledWith(expect.stringMatching(/^http:\/\/localhost:5200\/connect\/endsession\?id_token_hint=id-1&/));
    expect(http.hasExpiredSession()).toBe(false);
  });
});

describe('session claims', () => {
  it('reads the user id, login and admin claim from the stored token', async () => {
    const http = await loadTab();
    expect(http.getSessionClaims(token({ 'cpnucleo:login': 'ana', 'cpnucleo:admin': 'true' }))).toEqual({ sub: 'user-1', login: 'ana', isAdmin: true });
    expect(http.getSessionClaims(token({ 'cpnucleo:login': 'bo' }))).toEqual({ sub: 'user-1', login: 'bo', isAdmin: false });
    expect(http.getSessionClaims(token({ 'cpnucleo:admin': 'false' }))?.isAdmin).toBe(false);
    expect(http.getSessionClaims(token({ sub: '' }))).toBeNull();
    expect(http.getSessionClaims(null)).toBeNull();
  });
});

describe('cross-tab logout', () => {
  it('signs out every other tab through BroadcastChannel without echoing to the sender', async () => {
    const tabA = await loadTab();
    const tabB = await loadTab();
    const onLogoutA = vi.fn();
    const onLogoutB = vi.fn();
    const stopA = tabA.subscribeToCrossTabLogout(onLogoutA);
    const stopB = tabB.subscribeToCrossTabLogout(onLogoutB);
    tabA.setStoredToken(token({ exp: secondsFromNow(1800) }));

    tabA.logout();

    await vi.waitFor(() => expect(onLogoutB).toHaveBeenCalledTimes(1));
    expect(onLogoutA).not.toHaveBeenCalled();
    expect(tabB.getStoredToken()).toBeNull();
    stopA(); stopB();
  });

  it('shows the signed-out page in a tracked tab when another tab signs out', async () => {
    const tabA = await loadTab();
    const tabB = await loadTab();
    tabB.setStoredToken(token({ exp: secondsFromNow(1800) }));
    const stop = tabB.setupSessionActivityTracking();

    tabA.logout();

    // No automatic sign-in: the identity session may not have ended yet in the other tab.
    await vi.waitFor(() => expect(win.location.assign).toHaveBeenCalledWith('/login/?signedOut=1'));
    stop();
  });

  it('signing out revokes the refresh token, signs out the other tabs and ends the identity session', async () => {
    const tabA = await loadTab();
    const tabB = await loadTab();
    const onLogoutB = vi.fn();
    const stopB = tabB.subscribeToCrossTabLogout(onLogoutB);
    tabA.setStoredTokens({ accessToken: token({ exp: secondsFromNow(1800) }), refreshToken: 'refresh-1', idToken: 'id-1' });
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(null, { status: 200 }));

    await tabA.signOut();

    const [url, init] = fetchMock.mock.calls[0];
    expect(url).toBe('http://localhost:5200/connect/revocation');
    expect(Object.fromEntries(new URLSearchParams(String(init?.body)))).toEqual({ token: 'refresh-1', token_type_hint: 'refresh_token', client_id: 'cpnucleo-webclient' });
    expect(win.location.assign).toHaveBeenCalledWith(
      'http://localhost:5200/connect/endsession?id_token_hint=id-1&post_logout_redirect_uri=http%3A%2F%2Flocalhost%3A5030%2Flogin%2F');
    await vi.waitFor(() => expect(onLogoutB).toHaveBeenCalledTimes(1));
    expect(tabA.getStoredToken()).toBeNull();
    stopB();
  });

  it('falls back to storage events when BroadcastChannel is unavailable', async () => {
    Object.defineProperty(globalThis, 'BroadcastChannel', { value: undefined, configurable: true, writable: true });
    const tabA = await loadTab();
    const tabB = await loadTab();
    const onLogout = vi.fn();
    const stop = tabB.subscribeToCrossTabLogout(onLogout);

    tabA.logout();
    expect(local.setItem).toHaveBeenCalledWith(tabA.logoutStorageKey, expect.any(String));
    expect(local.removeItem).toHaveBeenCalledWith(tabA.logoutStorageKey);

    // Browsers deliver the write as a `storage` event in the other tabs only.
    win.dispatchEvent(Object.assign(new Event('storage'), { key: tabA.logoutStorageKey, newValue: '1' }));
    expect(onLogout).toHaveBeenCalledTimes(1);
    win.dispatchEvent(Object.assign(new Event('storage'), { key: 'unrelated', newValue: '1' }));
    expect(onLogout).toHaveBeenCalledTimes(1);
    stop();
  });
});

describe('duplicated tabs', () => {
  it('a tab whose sessionStorage was copied from a live tab drops the copied tokens instead of reusing the refresh token', async () => {
    // Both modules share one storage mock: exactly what a copied tab starts with.
    const original = await loadTab();
    original.setStoredTokens({ accessToken: token({ exp: secondsFromNow(1800) }), refreshToken: 'refresh-1', idToken: 'id-1' });
    const stop = original.setupSessionActivityTracking();
    const copy = await loadTab();

    await expect(copy.detectDuplicatedTab()).resolves.toBe(true);
    expect(session.getItem(copy.refreshTokenStorageKey)).toBeNull();
    stop();
  });

  it('a tab without a live original keeps its tokens', async () => {
    const http = await loadTab();
    http.setStoredTokens({ accessToken: token({ exp: secondsFromNow(1800) }), refreshToken: 'refresh-1', idToken: 'id-1' });

    await expect(http.detectDuplicatedTab(20)).resolves.toBe(false);
    expect(session.getItem(http.refreshTokenStorageKey)).toBe('refresh-1');
  });
});
