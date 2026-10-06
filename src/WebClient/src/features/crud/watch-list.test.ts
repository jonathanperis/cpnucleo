import { afterEach, describe, expect, it, vi } from 'vitest';
import { ApiError } from '~/lib/api/http-client';
import { RECONNECT_MAX_DELAY_MS, reconnectDelay, watchListing } from './watch-list';

afterEach(() => vi.useRealTimers());

describe('listing reconnects', () => {
  it('reconnects after a transport failure and stops when the page is disposed', async () => {
    vi.useFakeTimers();
    const controller = new AbortController();
    const subscribe = vi.fn().mockRejectedValueOnce(new TypeError('Disconnected'))
      .mockImplementationOnce(async () => { controller.abort(); });
    const status = vi.fn();
    const watching = watchListing(subscribe, controller.signal, status, { random: () => 1 });
    await vi.advanceTimersByTimeAsync(2000);
    await watching;
    expect(subscribe).toHaveBeenCalledTimes(2);
    expect(status).toHaveBeenCalledWith('Reconnecting');
  });

  it.each([401, 403, 404, 400])('does not retry a %i failure', async status => {
    const subscribe = vi.fn().mockRejectedValue(new ApiError(status, 'No'));
    await expect(watchListing(subscribe, new AbortController().signal, () => {})).rejects.toMatchObject({ status });
    expect(subscribe).toHaveBeenCalledTimes(1);
  });

  it('backs off exponentially until a live snapshot arrives, then resets', async () => {
    vi.useFakeTimers({ now: 0 });
    const controller = new AbortController();
    const startedAt: number[] = [];
    const attempts: Array<(onLiveSnapshot: () => void) => Promise<void>> = [
      async () => { throw new TypeError('connection refused'); },
      async () => undefined, // stream ended immediately without a snapshot
      async () => undefined, // non-SSE / empty response
      async onLiveSnapshot => { onLiveSnapshot(); }, // one snapshot, then the server closed the stream
      async () => { controller.abort(); },
    ];
    const subscribe = vi.fn(async (onLiveSnapshot: () => void) => {
      startedAt.push(Date.now());
      await attempts[startedAt.length - 1](onLiveSnapshot);
    });

    const watching = watchListing(subscribe, controller.signal, () => {}, { random: () => 1 });
    await vi.advanceTimersByTimeAsync(60_000);
    await watching;

    const gaps = startedAt.slice(1).map((time, index) => time - startedAt[index]);
    expect(gaps).toEqual([2000, 4000, 8000, 1000]);
  });

  it('caps the delay and applies jitter within half to the full delay', () => {
    expect(reconnectDelay(0, () => 1)).toBe(1000);
    expect(reconnectDelay(3, () => 0)).toBe(4000);
    expect(reconnectDelay(3, () => 1)).toBe(8000);
    expect(reconnectDelay(20, () => 1)).toBe(RECONNECT_MAX_DELAY_MS);
    expect(reconnectDelay(20, () => 0)).toBe(RECONNECT_MAX_DELAY_MS / 2);
    const sampled = Array.from({ length: 50 }, () => reconnectDelay(2));
    expect(sampled.every(delay => delay >= 2000 && delay <= 4000)).toBe(true);
  });

  it('waits at least the Retry-After delay after a rate-limited attempt', async () => {
    vi.useFakeTimers({ now: 0 });
    const controller = new AbortController();
    const startedAt: number[] = [];
    const subscribe = vi.fn(async () => {
      startedAt.push(Date.now());
      if (startedAt.length === 1) throw new ApiError(429, 'Slow down', undefined, { retryAfterSeconds: 30 });
      controller.abort();
    });
    const watching = watchListing(subscribe, controller.signal, () => {}, { random: () => 1 });
    await vi.advanceTimersByTimeAsync(31_000);
    await watching;
    expect(startedAt[1] - startedAt[0]).toBe(30_000);
  });
});
