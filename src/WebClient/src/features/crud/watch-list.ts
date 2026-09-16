import { ApiError } from '~/lib/api/http-client';

export const watchListing = async (subscribe: () => Promise<unknown>, signal: AbortSignal, status: (value: string) => void) => {
  let delay = 1000;
  while (!signal.aborted) {
    try {
      status('Connecting');
      await subscribe();
      delay = 1000;
    } catch (error) {
      if (signal.aborted) return;
      if (error instanceof ApiError && [400, 401, 403, 404].includes(error.status)) throw error;
    }
    if (signal.aborted) return;
    status('Reconnecting');
    await new Promise<void>((resolve) => {
      const done = () => { clearTimeout(timer); signal.removeEventListener('abort', done); resolve(); };
      const timer = setTimeout(done, delay);
      signal.addEventListener('abort', done, { once: true });
    });
    delay = Math.min(delay * 2, 15000);
  }
};
