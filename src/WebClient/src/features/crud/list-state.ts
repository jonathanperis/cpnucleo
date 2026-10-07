import { filterFields, tableFields } from '~/lib/api/resource-metadata';
import type { RelationFilterKey, ResourceMetadata } from '~/lib/api/types';
import { MAX_PAGE_SIZE } from '~/lib/api/webapi-client';
import { DEFAULT_PAGE_SIZE } from './pagination';

export const PAGE_SIZES = [10, 25, 50, 100] as const;
export const MAX_SEARCH_LENGTH = 128;
const uuidPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;

export interface ListState {
  page: number;
  pageSize: number;
  /** Listed field name; `createdAt` descending is the default (newest first). */
  sort: string;
  order: 'asc' | 'desc';
  search: string;
  filters: Partial<Record<RelationFilterKey, string>>;
  /** Restricts the list to these records (`?ids=…`), for links to a single record. */
  ids: string[];
}

/** One-time requests carried by a link: open the create form, or edit one record. */
export interface ListIntent {
  create: boolean;
  editId: string | null;
}

export const DEFAULT_SORT_FIELD = 'createdAt';
export const defaultOrderFor = (field: string): 'asc' | 'desc' => (field === DEFAULT_SORT_FIELD ? 'desc' : 'asc');

export const defaultListState = (): ListState => ({ page: 1, pageSize: DEFAULT_PAGE_SIZE, sort: DEFAULT_SORT_FIELD, order: 'desc', search: '', filters: {}, ids: [] });

const positiveInt = (value: string | null) => {
  const parsed = Number(value);
  return Number.isInteger(parsed) && parsed >= 1 ? parsed : undefined;
};

/**
 * Reads list state from a query string. Anything the resource cannot use (unknown sort fields,
 * filters it does not have, malformed ids) is dropped instead of being sent to the API.
 */
export const parseListState = (resource: ResourceMetadata, search: string): { state: ListState; intent: ListIntent } => {
  const params = new URLSearchParams(search);
  const state = defaultListState();
  state.page = Math.min(positiveInt(params.get('page')) ?? 1, 1_000_000);
  const size = positiveInt(params.get('size'));
  if (size && (PAGE_SIZES as readonly number[]).includes(size)) state.pageSize = size;
  const sort = params.get('sort');
  if (sort && tableFields(resource).some(field => field.name === sort)) {
    state.sort = sort;
    state.order = defaultOrderFor(sort);
  }
  const order = params.get('order');
  if (order === 'asc' || order === 'desc') state.order = order;
  state.search = (params.get('search') ?? '').trim().slice(0, MAX_SEARCH_LENGTH);
  for (const field of filterFields(resource)) {
    const value = params.get(field.name);
    if (value && uuidPattern.test(value)) state.filters[field.name] = value;
  }
  state.ids = [...new Set((params.get('ids') ?? '').split(',').map(id => id.trim()).filter(id => uuidPattern.test(id)))].slice(0, MAX_PAGE_SIZE);
  const edit = params.get('edit');
  return { state, intent: { create: params.get('new') === '1', editId: edit && uuidPattern.test(edit) ? edit : null } };
};

/** The query string for a state; defaults are omitted so plain list links stay clean. */
export const serializeListState = (state: ListState): string => {
  const params = new URLSearchParams();
  if (state.search) params.set('search', state.search);
  for (const [key, value] of Object.entries(state.filters)) if (value) params.set(key, value);
  if (state.ids.length > 0) params.set('ids', state.ids.join(','));
  if (state.sort !== DEFAULT_SORT_FIELD || state.order !== 'desc') {
    params.set('sort', state.sort);
    params.set('order', state.order);
  }
  if (state.pageSize !== DEFAULT_PAGE_SIZE) params.set('size', String(state.pageSize));
  if (state.page > 1) params.set('page', String(state.page));
  const query = params.toString();
  return query ? `?${query}` : '';
};
