import { WEBAPI_BASE_URL } from '../config';
import { findResource } from './resource-metadata';
import { requestJson } from './http-client';
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

const paginationParams = (pageNumber: number, pageSize: number) => ({
  pageNumber,
  pageSize,
  'pagination.pageNumber': pageNumber,
  'pagination.pageSize': pageSize,
});

export const createWebApiClient = (baseUrl = WEBAPI_BASE_URL) => {
  const root = normalizeBase(baseUrl);
  return {
    async list<T extends ApiEntity>(resourceKey: ResourceKey, pageNumber = 1, pageSize = 25, signal?: AbortSignal) {
      const resource = findResource(resourceKey);
      const payload = await requestJson<ListEnvelope<T>>(withQuery(`${root}${resource.listPath}`, paginationParams(pageNumber, pageSize)), { signal });
      return normalizeList(payload);
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
