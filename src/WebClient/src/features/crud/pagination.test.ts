import { describe, expect, it } from 'vitest';
import { buildPageOptions, DEFAULT_PAGE_SIZE, getLastPage } from './pagination';

describe('CRUD pagination helpers', () => {
  it('builds server-side page options from total count and page size', () => {
    expect(getLastPage(52, 25)).toBe(3);
    expect(buildPageOptions(52, 25)).toEqual([1, 2, 3]);
  });

  it('uses the server-side default page size for initial listing loads', () => {
    expect(DEFAULT_PAGE_SIZE).toBe(10);
  });

  it('keeps pagination stable for empty totals and invalid page sizes', () => {
    expect(getLastPage(0, 25)).toBe(1);
    expect(buildPageOptions(3, 0)).toEqual([1, 2, 3]);
  });
});
