import { resourceMetadata } from '~/lib/api/resource-metadata';
import { webApiClient } from '~/lib/api/webapi-client';

export const loadHomeCounters = async (root: HTMLElement, signal: AbortSignal) => {
  await Promise.all(resourceMetadata.map(async resource => {
    const element = root.querySelector<HTMLElement>(`[data-count="${resource.key}"]`);
    if (!element) return;
    try {
      const result = await webApiClient.list(resource.key, 1, 1, signal);
      if (!signal.aborted) element.textContent = String(result.totalCount ?? 0);
    } catch {
      if (!signal.aborted) element.textContent = 'Unavailable';
    }
  }));
};
