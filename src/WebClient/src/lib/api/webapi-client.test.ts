import { afterEach, describe, expect, it, vi } from 'vitest';
import { createWebApiClient, normalizeList } from './webapi-client';
import { requestJson } from './http-client';

afterEach(() => vi.restoreAllMocks());

describe('webapi client', () => {
  it('normalizes array and paginated list payloads', () => {
    expect(normalizeList([{ id: '1' }]).totalCount).toBe(1);
    expect(normalizeList({ data: [{ id: '2' }], total: 4, page: 2, pageSize: 1 }).items?.[0].id).toBe('2');
  });

  it('uses plural list and singular item endpoints', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify([]), { status: 200 }));
    const client = createWebApiClient('http://example.test/api');
    await client.list('projects', 2, 10);
    const listUrl = new URL(fetchMock.mock.calls[0][0]?.toString() ?? '');
    expect(`${listUrl.origin}${listUrl.pathname}`).toBe('http://example.test/api/projects');
    expect(listUrl.searchParams.get('pageNumber')).toBe('2');
    expect(listUrl.searchParams.get('pageSize')).toBe('10');
    expect(listUrl.searchParams.get('pagination.pageNumber')).toBe('2');
    expect(listUrl.searchParams.get('pagination.pageSize')).toBe('10');
    fetchMock.mockResolvedValue(new Response(JSON.stringify({ id: 'abc' }), { status: 200 }));
    await client.update('projects', 'abc', { name: 'Demo' });
    expect(fetchMock.mock.calls[1][0]?.toString()).toBe('http://example.test/api/project?id=abc');
    expect((fetchMock.mock.calls[1][1] as RequestInit).method).toBe('PATCH');
  });

  it('normalizes API errors', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify({ title: 'Too many requests' }), { status: 429 }));
    await expect(requestJson('http://example.test')).rejects.toMatchObject({ name: 'ApiError', status: 429, message: 'Too many requests' });
  });
});
