import { afterEach, expect, it, vi } from 'vitest';
import { ApiError } from '~/lib/api/http-client';
import { watchListing } from './watch-list';

afterEach(() => vi.useRealTimers());

it('reconnects after a transport failure and stops when the page is disposed', async () => {
  vi.useFakeTimers();
  const controller = new AbortController();
  const subscribe = vi.fn().mockRejectedValueOnce(new TypeError('Disconnected'))
    .mockImplementationOnce(async () => { controller.abort(); });
  const status = vi.fn();
  const watching = watchListing(subscribe, controller.signal, status);
  await vi.advanceTimersByTimeAsync(1000);
  await watching;
  expect(subscribe).toHaveBeenCalledTimes(2);
  expect(status).toHaveBeenCalledWith('Reconnecting');
});

it('does not retry an authorization failure', async () => {
  const subscribe = vi.fn().mockRejectedValue(new ApiError(403, 'Forbidden'));
  await expect(watchListing(subscribe, new AbortController().signal, () => {})).rejects.toMatchObject({ status: 403 });
  expect(subscribe).toHaveBeenCalledTimes(1);
});
