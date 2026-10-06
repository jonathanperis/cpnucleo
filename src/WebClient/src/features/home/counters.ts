import { getSessionClaims, type SessionClaims } from '~/lib/api/http-client';
import { resourceMetadata } from '~/lib/api/resource-metadata';
import { webApiClient } from '~/lib/api/webapi-client';

/**
 * Loads one count per work area. The individual badges are not live regions (eleven competing
 * announcements); a single polite status summarizes the result once every count has settled.
 */
export const loadHomeCounters = async (root: HTMLElement, signal: AbortSignal, session: SessionClaims | null = getSessionClaims()) => {
  let unavailable = 0;
  await Promise.all(resourceMetadata.map(async resource => {
    const element = root.querySelector<HTMLElement>(`[data-count="${resource.key}"]`);
    if (!element) return;
    if (resource.access.adminRead && !session?.isAdmin) {
      element.textContent = 'Admin only';
      return;
    }
    try {
      const result = await webApiClient.list(resource.key, 1, 1, signal);
      if (!signal.aborted) element.textContent = String(result.totalCount ?? 0);
    } catch {
      if (signal.aborted) return;
      unavailable += 1;
      element.textContent = 'Unavailable';
    }
  }));
  if (signal.aborted) return;
  root.querySelector('[data-work-areas]')?.setAttribute('aria-busy', 'false');
  const summary = root.querySelector<HTMLElement>('[data-home-summary]');
  if (summary) {
    summary.textContent = unavailable === 0
      ? `Record counts loaded for ${resourceMetadata.length} work areas.`
      : `Record counts loaded; ${unavailable} of ${resourceMetadata.length} work areas are unavailable.`;
  }
};
