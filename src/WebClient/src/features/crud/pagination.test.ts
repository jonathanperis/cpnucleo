import { describe, expect, it } from 'vitest';
import { buildPageOptions, buildPaginationItems, DEFAULT_PAGE_SIZE, getLastPage } from './pagination';

describe('CRUD pagination helpers', () => {
  it('builds server-side page options from total count and page size', () => {
    expect(getLastPage(52, 25)).toBe(3);
    expect(buildPageOptions(52, 25)).toEqual([1, 2, 3]);
  });

  it('builds a compact cohesive pagination range with boundary pages and ellipses', () => {
    expect(buildPaginationItems(1, 100)).toEqual([1, 2, 3, 4, 5, 6, 7, 'end-ellipsis', 100]);
    expect(buildPaginationItems(5, 100)).toEqual([1, 2, 3, 4, 5, 6, 7, 'end-ellipsis', 100]);
    expect(buildPaginationItems(50, 100)).toEqual([1, 'start-ellipsis', 47, 48, 49, 50, 51, 52, 53, 'end-ellipsis', 100]);
    expect(buildPaginationItems(96, 100)).toEqual([1, 'start-ellipsis', 94, 95, 96, 97, 98, 99, 100]);
    expect(buildPaginationItems(98, 100)).toEqual([1, 'start-ellipsis', 94, 95, 96, 97, 98, 99, 100]);
  });

  it('uses the server-side default page size for initial listing loads', () => {
    expect(DEFAULT_PAGE_SIZE).toBe(10);
  });

  it('keeps pagination stable for empty totals and invalid page sizes', () => {
    expect(getLastPage(0, 25)).toBe(1);
    expect(buildPageOptions(3, 0)).toEqual([1, 2, 3]);
  });
});
