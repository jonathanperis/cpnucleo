export const DEFAULT_PAGE_SIZE = 10;

export const normalizePageSize = (pageSize: number) => Math.max(1, Math.trunc(pageSize) || 1);

export const getLastPage = (totalCount: number, pageSize: number) =>
  Math.max(1, Math.ceil(Math.max(0, totalCount) / normalizePageSize(pageSize)));

export const buildPageOptions = (totalCount: number, pageSize: number) =>
  Array.from({ length: getLastPage(totalCount, pageSize) }, (_, index) => index + 1);

export type PaginationItem = number | 'start-ellipsis' | 'end-ellipsis';

const COMPACT_EDGE_COUNT = 7;
const COMPACT_WINDOW_RADIUS = 3;

export const buildPaginationItems = (currentPage: number, totalPages: number): PaginationItem[] => {
  const lastPage = Math.max(1, Math.trunc(totalPages) || 1);
  const activePage = Math.min(Math.max(1, Math.trunc(currentPage) || 1), lastPage);

  if (lastPage <= COMPACT_EDGE_COUNT + 2) {
    return Array.from({ length: lastPage }, (_, index) => index + 1);
  }

  if (activePage <= COMPACT_EDGE_COUNT - COMPACT_WINDOW_RADIUS + 1) {
    return [...Array.from({ length: COMPACT_EDGE_COUNT }, (_, index) => index + 1), 'end-ellipsis', lastPage];
  }

  if (activePage >= lastPage - (COMPACT_EDGE_COUNT - COMPACT_WINDOW_RADIUS)) {
    const start = lastPage - COMPACT_EDGE_COUNT + 1;
    return [1, 'start-ellipsis', ...Array.from({ length: COMPACT_EDGE_COUNT }, (_, index) => start + index)];
  }

  const windowStart = activePage - COMPACT_WINDOW_RADIUS;
  return [
    1,
    'start-ellipsis',
    ...Array.from({ length: COMPACT_WINDOW_RADIUS * 2 + 1 }, (_, index) => windowStart + index),
    'end-ellipsis',
    lastPage,
  ];
};
