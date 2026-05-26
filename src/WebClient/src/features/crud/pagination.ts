export const DEFAULT_PAGE_SIZE = 10;

export const normalizePageSize = (pageSize: number) => Math.max(1, Math.trunc(pageSize) || 1);

export const getLastPage = (totalCount: number, pageSize: number) =>
  Math.max(1, Math.ceil(Math.max(0, totalCount) / normalizePageSize(pageSize)));

export const buildPageOptions = (totalCount: number, pageSize: number) =>
  Array.from({ length: getLastPage(totalCount, pageSize) }, (_, index) => index + 1);
