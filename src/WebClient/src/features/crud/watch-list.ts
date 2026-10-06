import { ApiError } from '~/lib/api/http-client';

export const RECONNECT_BASE_DELAY_MS = 1000;
export const RECONNECT_MAX_DELAY_MS = 15000;

/** Errors that a reconnect cannot fix. 401 has already ended the session in the HTTP layer. */
const fatalStatuses = new Set([400, 401, 403, 404]);

/**
 * Delay before the next attempt. `failures` counts consecutive attempts that ended without a live
 * snapshot. Equal jitter keeps at least half of the exponential delay so many tabs do not
 * reconnect in lockstep after a server restart.
 */
export const reconnectDelay = (failures: number, random: () => number = Math.random) => {
  const ceiling = Math.min(RECONNECT_BASE_DELAY_MS * 2 ** Math.max(0, failures), RECONNECT_MAX_DELAY_MS);
  return Math.round(ceiling / 2 + random() * (ceiling / 2));
};

export interface WatchListingOptions {
  random?: () => number;
}

/**
 * Keeps a listing subscription open. `subscribe` receives `onLiveSnapshot`, which the caller
 * invokes whenever the stream delivered a snapshot. Backoff resets only after such a snapshot; a
 * stream that ends immediately, a non-SSE response or a transport error backs off exponentially.
 */
export const watchListing = async (
  subscribe: (onLiveSnapshot: () => void) => Promise<unknown>,
  signal: AbortSignal,
  status: (value: string) => void,
  { random = Math.random }: WatchListingOptions = {},
) => {
  let failures = 0;
  while (!signal.aborted) {
    let receivedLiveSnapshot = false;
    let retryAfterMs = 0;
    try {
      status('Connecting');
      await subscribe(() => { receivedLiveSnapshot = true; });
    } catch (error) {
      if (signal.aborted) return;
      if (error instanceof ApiError && fatalStatuses.has(error.status)) throw error;
      if (error instanceof ApiError && error.retryAfterSeconds !== undefined) retryAfterMs = error.retryAfterSeconds * 1000;
    }
    if (signal.aborted) return;
    failures = receivedLiveSnapshot ? 0 : failures + 1;
    status('Reconnecting');
    const delay = Math.max(reconnectDelay(failures, random), retryAfterMs);
    await new Promise<void>((resolve) => {
      const done = () => { clearTimeout(timer); signal.removeEventListener('abort', done); resolve(); };
      const timer = setTimeout(done, delay);
      signal.addEventListener('abort', done, { once: true });
    });
  }
};
