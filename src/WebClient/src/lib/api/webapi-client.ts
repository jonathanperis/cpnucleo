import { WEBAPI_BASE_URL } from '../config';
import { findResource } from './resource-metadata';
import { ApiError, getStoredToken, readErrorResponse, requestJson } from './http-client';
import type { ApiEntity, PaginatedResult, ResourceKey } from './types';

const normalizeBase = (baseUrl: string) => baseUrl.replace(/\/$/, '');
const withQuery = (url: string, params?: Record<string, string | number | undefined>) => {
  const next = new URL(url);
  Object.entries(params || {}).forEach(([key, value]) => {
    if (value !== undefined && value !== '') next.searchParams.set(key, String(value));
  });
  return next.toString();
};

type ListEnvelope<T extends ApiEntity> = T[] | PaginatedResult<T> | { result?: PaginatedResult<T> };
type ItemEnvelope<T extends ApiEntity> = T | { result?: T } | Record<string, unknown>;
export interface ListSnapshotInfo {
  /** True for snapshots delivered by the SSE stream; false for a plain JSON fallback. */
  live: boolean;
}
export type ListSubscriber<T extends ApiEntity> = (page: PaginatedResult<T>, info: ListSnapshotInfo) => void;

export const normalizeList = <T extends ApiEntity>(payload: ListEnvelope<T>): PaginatedResult<T> => {
  if (Array.isArray(payload)) return { items: payload, totalCount: payload.length, pageNumber: 1, pageSize: payload.length };
  const page = (payload && typeof payload === 'object' && 'result' in payload && payload.result ? payload.result : payload) as PaginatedResult<T>;
  return {
    ...page,
    items: page.items ?? page.data ?? page.results ?? [],
    totalCount: page.totalCount ?? page.total ?? page.items?.length ?? page.data?.length ?? page.results?.length ?? 0,
    pageNumber: page.pageNumber ?? page.page ?? 1,
    pageSize: page.pageSize ?? page.items?.length ?? page.data?.length ?? page.results?.length ?? 0,
  };
};

export const normalizeItem = <T extends ApiEntity>(payload: ItemEnvelope<T>, envelopeKey: string): T => {
  if (payload && typeof payload === 'object') {
    const record = payload as Record<string, unknown>;
    if (record.id !== undefined) return payload as T;
    if (record.result && typeof record.result === 'object') return record.result as T;
    if (record[envelopeKey] && typeof record[envelopeKey] === 'object') return record[envelopeKey] as T;
  }
  return payload as T;
};

export const parseServerSentEventData = (event: string): string[] => {
  const dataLines = event
    .split(/\r?\n/)
    .filter((line) => line.startsWith('data:'))
    .map((line) => line.slice(5).replace(/^ /, ''));
  return dataLines.length > 0 ? [dataLines.join('\n')] : [];
};

/** Stable default order for every list request: canonical persisted column + direction. */
export const DEFAULT_SORT = { sortColumn: 'CreatedAt', sortOrder: 'ASC' } as const;
export const MAX_PAGE_SIZE = 100;

// Flat scalar query keys only (nested `pagination.*` keys break FastEndpoints query binding).
const listParams = (pageNumber: number, pageSize: number, extra: Record<string, string | undefined> = {}) => ({
  pageNumber,
  pageSize,
  ...DEFAULT_SORT,
  ...extra,
});

const createAbortError = () => new DOMException('The operation was aborted.', 'AbortError');

const throwIfAborted = (signal?: AbortSignal) => {
  if (signal?.aborted) throw createAbortError();
};

const isAbortError = (error: unknown) => error instanceof DOMException && error.name === 'AbortError';

const toApiError = (error: unknown) => {
  if (error instanceof ApiError) return error;
  if (isAbortError(error)) return error;
  return new ApiError(0, error instanceof Error ? error.message : 'Unable to read the listing stream.', error);
};

const parseListPage = <T extends ApiEntity>(data: string): PaginatedResult<T> =>
  normalizeList<T>(JSON.parse(data) as ListEnvelope<T>);

const prepareWriteBody = (resourceKey: ResourceKey, body: Record<string, unknown>) => {
  if (resourceKey !== 'appointments' || typeof body.name === 'string' || typeof body.description !== 'string') return body;
  return { ...body, name: body.description };
};

/**
 * Opens the listing stream. The server sends a snapshot immediately and then one per change (or
 * every 15 seconds), so no separate JSON request is made. A server that answers without SSE is
 * treated as a single non-live snapshot. Non-2xx responses go through the shared error handling
 * (a 401 ends the session).
 */
const streamList = async <T extends ApiEntity>(url: string, onPage: ListSubscriber<T>, signal?: AbortSignal): Promise<void> => {
  throwIfAborted(signal);

  const headers = new Headers({ Accept: 'text/event-stream' });
  const token = getStoredToken();
  if (token) headers.set('Authorization', `Bearer ${token}`);

  let response: Response;
  try {
    response = await fetch(url, { headers, signal });
  } catch (error) {
    if (signal?.aborted) throw createAbortError();
    throw new ApiError(0, 'Network error. Please check your connection and try again.', error);
  }

  if (!response.ok) throw await readErrorResponse(response);
  throwIfAborted(signal);

  if (!response.headers.get('Content-Type')?.toLowerCase().includes('text/event-stream')) {
    let page: PaginatedResult<T>;
    try {
      page = normalizeList<T>(await response.json() as ListEnvelope<T>);
    } catch (error) {
      throw toApiError(isAbortError(error) ? error : new Error('The listing response was not valid JSON.'));
    }
    throwIfAborted(signal);
    onPage(page, { live: false });
    return;
  }
  if (!response.body) throw new ApiError(0, 'The listing stream ended before sending data.');

  const reader = response.body.getReader();
  const decoder = new TextDecoder();
  let buffer = '';
  let receivedData = false;

  const handleEvent = (event: string) => {
    for (const data of parseServerSentEventData(event)) {
      throwIfAborted(signal);
      const page = parseListPage<T>(data);
      receivedData = true;
      onPage(page, { live: true });
    }
  };

  try {
    while (true) {
      throwIfAborted(signal);
      const chunk = await reader.read();
      throwIfAborted(signal);
      buffer += decoder.decode(chunk.value, { stream: !chunk.done });
      const events = buffer.split(/\r?\n\r?\n/);
      buffer = events.pop() ?? '';
      events.forEach(handleEvent);
      if (chunk.done) break;
    }
    if (buffer.trim()) handleEvent(buffer);
  } catch (error) {
    throw toApiError(error);
  } finally {
    reader.releaseLock();
  }

  if (!receivedData) throw new ApiError(0, 'The listing stream ended before sending data.');
};

const uniqueIds = (ids: string[]) => [...new Set(ids.map(id => id.trim()).filter(Boolean))];

export const createWebApiClient = (baseUrl = WEBAPI_BASE_URL) => {
  const root = normalizeBase(baseUrl);
  const listUrl = (resourceKey: ResourceKey, pageNumber: number, pageSize: number, extra?: Record<string, string | undefined>) =>
    withQuery(`${root}${findResource(resourceKey).listPath}`, listParams(pageNumber, pageSize, extra));
  return {
    /** Batched label lookup through the flat `ids` filter, at most 100 ids per request. */
    async lookup(resourceKey: ResourceKey, ids: string[], signal?: AbortSignal): Promise<ApiEntity[]> {
      const distinct = uniqueIds(ids);
      const batches: string[][] = [];
      for (let index = 0; index < distinct.length; index += MAX_PAGE_SIZE) batches.push(distinct.slice(index, index + MAX_PAGE_SIZE));
      const pages = await Promise.all(batches.map(async batch =>
        normalizeList(await requestJson<ListEnvelope<ApiEntity>>(listUrl(resourceKey, 1, MAX_PAGE_SIZE, { ids: batch.join(',') }), { signal })).items ?? []));
      return pages.flat();
    },
    async list<T extends ApiEntity>(resourceKey: ResourceKey, pageNumber = 1, pageSize = 25, signal?: AbortSignal, search?: string) {
      const payload = await requestJson<ListEnvelope<T>>(listUrl(resourceKey, pageNumber, pageSize, { search: search?.trim() || undefined }), { signal });
      return normalizeList<T>(payload);
    },
    async subscribeList<T extends ApiEntity>(resourceKey: ResourceKey, pageNumber: number, pageSize: number, onPage: ListSubscriber<T>, signal?: AbortSignal) {
      await streamList<T>(listUrl(resourceKey, pageNumber, pageSize), onPage, signal);
    },
    async get<T extends ApiEntity>(resourceKey: ResourceKey, id: string, signal?: AbortSignal) {
      const resource = findResource(resourceKey);
      const envelopeKey = resource.itemPath.replace(/^\//, '');
      return normalizeItem<T>(await requestJson<ItemEnvelope<T>>(withQuery(`${root}${resource.itemPath}`, { id }), { signal }), envelopeKey);
    },
    async create<T extends ApiEntity>(resourceKey: ResourceKey, body: Record<string, unknown>) {
      const resource = findResource(resourceKey);
      return requestJson<T>(`${root}${resource.itemPath}`, { method: 'POST', body: JSON.stringify(prepareWriteBody(resourceKey, { id: crypto.randomUUID(), ...body })) });
    },
    async update<T extends ApiEntity>(resourceKey: ResourceKey, id: string, body: Record<string, unknown>) {
      const resource = findResource(resourceKey);
      return requestJson<T>(withQuery(`${root}${resource.itemPath}`, { id }), { method: 'PATCH', body: JSON.stringify(prepareWriteBody(resourceKey, { ...body, id })) });
    },
    /** Atomic removal: 1–100 distinct ids; the server rejects the whole batch on 404/409. */
    async delete(resourceKey: ResourceKey, ids: string | string[]) {
      const distinct = uniqueIds(Array.isArray(ids) ? ids : [ids]);
      if (distinct.length < 1 || distinct.length > MAX_PAGE_SIZE) throw new ApiError(400, `Select between 1 and ${MAX_PAGE_SIZE} records to delete.`);
      const resource = findResource(resourceKey);
      await requestJson<unknown>(`${root}${resource.itemPath}`, { method: 'DELETE', body: JSON.stringify({ ids: distinct }) });
    },
  };
};

export const webApiClient = createWebApiClient();
