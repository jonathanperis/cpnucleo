import type { SessionClaims } from '~/lib/api/http-client';
import type { ApiEntity, ResourceKey } from '~/lib/api/types';
import { webApiClient } from '~/lib/api/webapi-client';
import { createIcon, type IconName } from '~/lib/icons';
import { displayEntityLabel } from '~/features/crud/relation-display';

export type Labels = Map<string, string>;

/** A record's own name for links and lists (the pickers' "name — description" is too long here). */
export const shortLabel = (record: ApiEntity) => {
  for (const key of ['name', 'description', 'login']) {
    const value = record[key];
    if (typeof value === 'string' && value.trim()) return value.trim();
  }
  return displayEntityLabel(record);
};

/**
 * Display labels for related records. Members cannot list team members, so person labels fall
 * back to the signed-in account (and "Team member" for anyone else) instead of failing the page.
 */
export const loadLabels = async (key: ResourceKey, ids: unknown[], signal: AbortSignal, session: SessionClaims | null): Promise<Labels> => {
  const distinct = [...new Set(ids.filter((id): id is string => typeof id === 'string' && id !== ''))];
  const labels: Labels = new Map();
  if (distinct.length === 0) return labels;
  if (key === 'users' && !session?.isAdmin) {
    for (const id of distinct) labels.set(id, id === session?.sub ? `${session.login} (you)` : 'Team member');
    return labels;
  }
  const records = await webApiClient.lookup(key, distinct, signal).catch(() => [] as ApiEntity[]);
  for (const record of records) labels.set(String(record.id), shortLabel(record));
  return labels;
};

export const label = (labels: Labels, id: unknown, fallback = 'Unavailable') => (typeof id === 'string' && labels.get(id)) || fallback;

/** Workflow steps ordered by `order`; the last one counts as "done". */
export const loadSteps = async (signal: AbortSignal) => {
  const steps = await webApiClient.listAll<ApiEntity>('workflows', { sort: { column: 'Order', order: 'ASC' } }, signal, 100);
  return steps.sort((a, b) => Number(a.order) - Number(b.order));
};

export const isDoneStep = (steps: ApiEntity[], workflowId: unknown) => steps.length > 1 && steps.at(-1)?.id === workflowId;

export const element = <K extends keyof HTMLElementTagNameMap>(tag: K, className = '', text?: string): HTMLElementTagNameMap[K] => {
  const node = document.createElement(tag);
  if (className) node.className = className;
  if (text !== undefined) node.textContent = text;
  return node;
};

export const link = (href: string, text: string, className = 'table-link') => {
  const anchor = element('a', className, text);
  anchor.href = href;
  return anchor;
};

export const iconLink = (href: string, icon: IconName, text: string, className = 'btn btn-secondary btn-sm') => {
  const anchor = element('a', className);
  anchor.href = href;
  anchor.append(createIcon(icon), document.createTextNode(text));
  return anchor;
};

export const badge = (text: string, tone: 'danger' | 'warning' | 'success' | 'neutral' = 'neutral') =>
  element('span', `badge badge-${tone}`, text);

/** A compact list section body: rows, or an empty message. */
export const fillList = (list: HTMLElement, rows: HTMLElement[], emptyText: string) => {
  if (rows.length === 0) {
    list.replaceChildren(element('li', 'px-5 py-6 text-center text-sm text-muted', emptyText));
    return;
  }
  list.replaceChildren(...rows);
};
