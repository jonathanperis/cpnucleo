import { afterEach, describe, expect, it, vi } from 'vitest';
import { createWebApiClient, normalizeList, parseServerSentEventData } from './webapi-client';
import { requestJson } from './http-client';

const sseResponse = (payload: unknown) => new Response(
  `event: listing\ndata: ${JSON.stringify(payload)}\n\n`,
  { status: 200, headers: { 'Content-Type': 'text/event-stream' } },
);

afterEach(() => vi.restoreAllMocks());

const waitForExpectation = async (assertion: () => void) => {
  const startedAt = Date.now();
  let lastError: unknown;
  while (Date.now() - startedAt < 1000) {
    try {
      assertion();
      return;
    } catch (error) {
      lastError = error;
      await new Promise((resolve) => setTimeout(resolve, 10));
    }
  }
  throw lastError;
};

describe('webapi client', () => {
  it('normalizes array and paginated list payloads', () => {
    expect(normalizeList([{ id: '1' }]).totalCount).toBe(1);
    expect(normalizeList({ data: [{ id: '2' }], total: 4, page: 2, pageSize: 1 }).items?.[0].id).toBe('2');
  });

  it('parses server sent event data lines', () => {
    expect(parseServerSentEventData('event: listing\ndata: {"ok":true}\n\n')).toEqual(['{"ok":true}']);
  });

  it('uses paginated JSON lists and singular item endpoints', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify([]), { status: 200, headers: { 'Content-Type': 'application/json' } }));
    const client = createWebApiClient('http://example.test/api');
    await client.list('projects', 2, 10);
    const listUrl = new URL(fetchMock.mock.calls[0][0]?.toString() ?? '');
    expect(`${listUrl.origin}${listUrl.pathname}`).toBe('http://example.test/api/projects');
    expect(listUrl.searchParams.get('pageNumber')).toBe('2');
    expect(listUrl.searchParams.get('pageSize')).toBe('10');
    expect(listUrl.searchParams.get('pagination.pageNumber')).toBe('2');
    expect(listUrl.searchParams.get('pagination.pageSize')).toBe('10');
    expect(new Headers((fetchMock.mock.calls[0][1] as RequestInit).headers).get('Accept')).toBe('application/json');
    fetchMock.mockResolvedValue(new Response(JSON.stringify({ id: 'abc' }), { status: 200 }));
    await client.update('projects', 'abc', { name: 'Demo' });
    expect(fetchMock.mock.calls[1][0]?.toString()).toBe('http://example.test/api/project?id=abc');
    expect((fetchMock.mock.calls[1][1] as RequestInit).method).toBe('PATCH');
  });

  it('adds the appointment name required by WebApi from the visible description field', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify({ id: 'abc' }), { status: 200 }));
    const client = createWebApiClient('http://example.test/api');

    await client.update('appointments', 'abc', { description: 'Planning', amountHours: 1 });

    expect(JSON.parse((fetchMock.mock.calls[0][1] as RequestInit).body as string)).toMatchObject({
      id: 'abc',
      name: 'Planning',
      description: 'Planning',
      amountHours: 1,
    });
  });

  it('notifies subscribers for streamed list pages', async () => {
    vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(new Response(JSON.stringify({ result: { data: [], totalCount: 0, pageNumber: 1, pageSize: 1 } }), { status: 200, headers: { 'Content-Type': 'application/json' } }))
      .mockResolvedValueOnce(sseResponse({ result: { data: [{ id: '1' }], totalCount: 3, pageNumber: 1, pageSize: 1 } }));
    const client = createWebApiClient('http://example.test/api');
    const onPage = vi.fn();
    await client.subscribeList('projects', 1, 1, onPage);
    expect(onPage).toHaveBeenCalledWith(expect.objectContaining({ totalCount: 3, items: [{ id: '1' }] }));
  });

  it('seeds subscribers from the paged JSON endpoint before waiting for live SSE updates', async () => {
    vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(new Response(JSON.stringify({ result: { data: [{ id: 'seed' }], totalCount: 7, pageNumber: 1, pageSize: 5 } }), { status: 200, headers: { 'Content-Type': 'application/json' } }))
      .mockReturnValueOnce(new Promise(() => undefined));
    const client = createWebApiClient('http://example.test/api');
    const onPage = vi.fn();
    void client.subscribeList('projects', 1, 5, onPage).catch(() => undefined);
    await waitForExpectation(() => expect(onPage).toHaveBeenCalledWith(expect.objectContaining({ totalCount: 7, items: [{ id: 'seed' }] })));
  });

  it('keeps the seeded page when the server does not open an SSE stream', async () => {
    vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(new Response(JSON.stringify({ result: { data: [{ id: 'seed' }], totalCount: 1, pageNumber: 1, pageSize: 25 } }), { status: 200, headers: { 'Content-Type': 'application/json' } }))
      .mockResolvedValueOnce(new Response(JSON.stringify([]), { status: 200, headers: { 'Content-Type': 'application/json' } }));
    const client = createWebApiClient('http://example.test/api');
    const onPage = vi.fn();
    await expect(client.subscribeList('projects', 1, 25, onPage)).resolves.toBeUndefined();
    expect(onPage).toHaveBeenCalledWith(expect.objectContaining({ totalCount: 1, items: [{ id: 'seed' }] }));
  });

  it('fails closed when the SSE stream ends before data arrives', async () => {
    vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(new Response(JSON.stringify([]), { status: 200, headers: { 'Content-Type': 'application/json' } }))
      .mockResolvedValueOnce(new Response('', { status: 200, headers: { 'Content-Type': 'text/event-stream' } }));
    const client = createWebApiClient('http://example.test/api');
    await expect(client.subscribeList('projects', 1, 25, () => undefined)).rejects.toMatchObject({ name: 'ApiError', message: 'The listing stream ended before sending data.' });
  });

  it('normalizes malformed SSE payloads as API errors', async () => {
    vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(new Response(JSON.stringify([]), { status: 200, headers: { 'Content-Type': 'application/json' } }))
      .mockResolvedValueOnce(new Response('event: listing\ndata: {nope}\n\n', { status: 200, headers: { 'Content-Type': 'text/event-stream' } }));
    const client = createWebApiClient('http://example.test/api');
    await expect(client.subscribeList('projects', 1, 25, () => undefined)).rejects.toMatchObject({ name: 'ApiError', status: 0 });
  });

  it('normalizes API errors', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify({ title: 'Too many requests' }), { status: 429 }));
    await expect(requestJson('http://example.test')).rejects.toMatchObject({ name: 'ApiError', status: 429, message: 'Too many requests' });
  });

  it('normalizes fetch transport failures', async () => {
    vi.spyOn(globalThis, 'fetch').mockRejectedValue(new TypeError('Failed to fetch'));
    await expect(requestJson('http://example.test')).rejects.toMatchObject({ name: 'ApiError', status: 0, message: 'Network error. Please check your connection and try again.' });
  });
});
