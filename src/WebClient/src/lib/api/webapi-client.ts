import { WEBAPI_BASE_URL } from '../config';
import { findResource } from './resource-metadata';
import { ApiError, getStoredToken, requestJson } from './http-client';
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
type ListSubscriber<T extends ApiEntity> = (page: PaginatedResult<T>) => void;

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

export const parseServerSentEventData = (event: string): string[] => {
  const dataLines = event
    .split(/\r?\n/)
    .filter((line) => line.startsWith('data:'))
    .map((line) => line.slice(5).trimStart());
  return dataLines.length > 0 ? [dataLines.join('\n')] : [];
};

const paginationParams = (pageNumber: number, pageSize: number) => ({
  pageNumber,
  pageSize,
  'pagination.pageNumber': pageNumber,
  'pagination.pageSize': pageSize,
});

const createAbortError = () => new DOMException('The operation was aborted.', 'AbortError');

const throwIfAborted = (signal?: AbortSignal) => {
  if (signal?.aborted) throw createAbortError();
};

const toApiError = (error: unknown) => {
  if (error instanceof ApiError) return error;
  if (error instanceof DOMException && error.name === 'AbortError') return error;
  return new ApiError(0, error instanceof Error ? error.message : 'Unable to read the listing stream.', error);
};

const parseListPage = <T extends ApiEntity>(data: string): PaginatedResult<T> =>
  normalizeList<T>(JSON.parse(data) as ListEnvelope<T>);

const streamList = async <T extends ApiEntity>(url: string, onPage: ListSubscriber<T>, signal?: AbortSignal, stopAfterFirst = false): Promise<PaginatedResult<T>> => {
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

  if (!response.ok) throw new ApiError(response.status, `Request failed with status ${response.status}.`);
  if (!response.headers.get('Content-Type')?.toLowerCase().includes('text/event-stream')) {
    throw new ApiError(0, 'The server did not open a listing stream.');
  }
  if (!response.body) throw new ApiError(0, 'The server did not open a listing stream.');

  const reader = response.body.getReader();
  const decoder = new TextDecoder();
  let buffer = '';
  let lastPage: PaginatedResult<T> | undefined;
  let receivedData = false;

  const handleEvent = async (event: string): Promise<PaginatedResult<T> | undefined> => {
    for (const data of parseServerSentEventData(event)) {
      throwIfAborted(signal);
      receivedData = true;
      const page = parseListPage<T>(data);
      lastPage = page;
      throwIfAborted(signal);
      onPage(page);
      if (stopAfterFirst) {
        throwIfAborted(signal);
        await reader.cancel();
        throwIfAborted(signal);
        return page;
      }
    }
    return undefined;
  };

  try {
    while (true) {
      throwIfAborted(signal);
      let chunk: ReadableStreamReadResult<Uint8Array>;
      try {
        chunk = await reader.read();
        throwIfAborted(signal);
        buffer += decoder.decode(chunk.value, { stream: !chunk.done });
        const events = buffer.split(/\r?\n\r?\n/);
        buffer = events.pop() ?? '';

        for (const event of events) {
          const page = await handleEvent(event);
          if (page) return page;
        }
      } catch (error) {
        throw toApiError(error);
      }

      if (chunk.done) break;
    }
  } finally {
    reader.releaseLock();
  }

  if (buffer.trim()) {
    throwIfAborted(signal);
    try {
      const page = await handleEvent(buffer);
      if (page) return page;
    } catch (error) {
      throw toApiError(error);
    }
  }

  if (!receivedData) throw new ApiError(0, 'The listing stream ended before sending data.');
  return lastPage!;
};

export const createWebApiClient = (baseUrl = WEBAPI_BASE_URL) => {
  const root = normalizeBase(baseUrl);
  return {
    async list<T extends ApiEntity>(resourceKey: ResourceKey, pageNumber = 1, pageSize = 25, signal?: AbortSignal) {
      const resource = findResource(resourceKey);
      const url = withQuery(`${root}${resource.listPath}`, paginationParams(pageNumber, pageSize));
      const payload = await requestJson<ListEnvelope<T>>(url, { signal });
      return normalizeList<T>(payload);
    },
    async subscribeList<T extends ApiEntity>(resourceKey: ResourceKey, pageNumber: number, pageSize: number, onPage: ListSubscriber<T>, signal?: AbortSignal) {
      const resource = findResource(resourceKey);
      const url = withQuery(`${root}${resource.listPath}`, paginationParams(pageNumber, pageSize));
      onPage(normalizeList<T>(await requestJson<ListEnvelope<T>>(url, { signal })));
      await streamList<T>(url, onPage, signal);
    },
    async get<T extends ApiEntity>(resourceKey: ResourceKey, id: string, signal?: AbortSignal) {
      const resource = findResource(resourceKey);
      return requestJson<T>(withQuery(`${root}${resource.itemPath}`, { id }), { signal });
    },
    async create<T extends ApiEntity>(resourceKey: ResourceKey, body: Record<string, unknown>) {
      const resource = findResource(resourceKey);
      return requestJson<T>(`${root}${resource.itemPath}`, { method: 'POST', body: JSON.stringify({ id: crypto.randomUUID(), ...body }) });
    },
    async update<T extends ApiEntity>(resourceKey: ResourceKey, id: string, body: Record<string, unknown>) {
      const resource = findResource(resourceKey);
      return requestJson<T>(withQuery(`${root}${resource.itemPath}`, { id }), { method: 'PATCH', body: JSON.stringify({ ...body, id }) });
    },
    async delete(resourceKey: ResourceKey, id: string) {
      const resource = findResource(resourceKey);
      await requestJson<unknown>(`${root}${resource.itemPath}`, { method: 'DELETE', body: JSON.stringify({ ids: [id] }) });
    },
  };
};

export const webApiClient = createWebApiClient();
