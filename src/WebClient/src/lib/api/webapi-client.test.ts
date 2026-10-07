import { afterEach, describe, expect, it, vi } from 'vitest';
import { createWebApiClient, normalizeList, parseServerSentEventData } from './webapi-client';
import { requestJson } from './http-client';
import { clearRequestLog, getRequestLog } from './request-log';

const sseResponse = (payload: unknown) => new Response(
  `event: listing\ndata: ${JSON.stringify(payload)}\n\n`,
  { status: 200, headers: { 'Content-Type': 'text/event-stream' } },
);

afterEach(() => vi.restoreAllMocks());

describe('webapi client', () => {
  it('normalizes array and paginated list payloads', () => {
    expect(normalizeList([{ id: '1' }]).totalCount).toBe(1);
    expect(normalizeList({ data: [{ id: '2' }], total: 4, page: 2, pageSize: 1 }).items?.[0].id).toBe('2');
  });

  it('parses server sent event data lines', () => {
    expect(parseServerSentEventData('event: listing\ndata: {"ok":true}\n\n')).toEqual(['{"ok":true}']);
  });

  it('sends each pagination parameter once with flat keys and an explicit stable sort', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify([]), { status: 200, headers: { 'Content-Type': 'application/json' } }));
    const client = createWebApiClient('http://example.test/api');
    await client.list('projects', 2, 10, undefined, '  core  ');
    const listUrl = new URL(fetchMock.mock.calls[0][0]?.toString() ?? '');
    expect(`${listUrl.origin}${listUrl.pathname}`).toBe('http://example.test/api/projects');
    expect([...listUrl.searchParams.keys()].sort()).toEqual(['pageNumber', 'pageSize', 'search', 'sortColumn', 'sortOrder']);
    expect(Object.fromEntries(listUrl.searchParams)).toEqual({ pageNumber: '2', pageSize: '10', search: 'core', sortColumn: 'CreatedAt', sortOrder: 'ASC' });
    expect(new Headers((fetchMock.mock.calls[0][1] as RequestInit).headers).get('Accept')).toBe('application/json');
    fetchMock.mockResolvedValue(new Response(JSON.stringify({ id: 'abc' }), { status: 200 }));
    await client.update('projects', 'abc', { name: 'Demo' });
    expect(fetchMock.mock.calls[1][0]?.toString()).toBe('http://example.test/api/project?id=abc');
    expect((fetchMock.mock.calls[1][1] as RequestInit).method).toBe('PATCH');
  });

  it('looks up relation labels through the flat ids filter in batches of at most 100', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockImplementation(async () => new Response(JSON.stringify({ items: [{ id: 'x' }], totalCount: 1 }), { status: 200 }));
    const client = createWebApiClient('http://example.test/api');
    const ids = Array.from({ length: 150 }, (_, index) => `00000000-0000-0000-0000-${String(index).padStart(12, '0')}`);
    await client.lookup('organizations', [...ids, ids[0]]);
    expect(fetchMock).toHaveBeenCalledTimes(2);
    const first = new URL(String(fetchMock.mock.calls[0][0]));
    expect(first.searchParams.get('ids')?.split(',')).toHaveLength(100);
    expect(first.searchParams.has('pagination.pageSize')).toBe(false);
    expect(new URL(String(fetchMock.mock.calls[1][0])).searchParams.get('ids')?.split(',')).toHaveLength(50);
  });

  it('sends sort, relation filters and date ranges as flat keys', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify([]), { status: 200 }));
    const client = createWebApiClient('http://example.test/api');
    await client.list('appointments', 1, 100, undefined, undefined, {
      sort: { column: 'KeepDate', order: 'DESC' }, filters: { userId: 'u-1', assignmentId: undefined },
      dateFrom: '2026-10-05T00:00:00.000Z', dateTo: '2026-10-12T00:00:00.000Z',
    });
    const url = new URL(String(fetchMock.mock.calls[0][0]));
    expect(Object.fromEntries(url.searchParams)).toEqual({
      pageNumber: '1', pageSize: '100', sortColumn: 'KeepDate', sortOrder: 'DESC', userId: 'u-1',
      dateFrom: '2026-10-05T00:00:00.000Z', dateTo: '2026-10-12T00:00:00.000Z',
    });
  });

  it('streams listings with the search, ids and filters of the current view', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(sseResponse({ items: [], totalCount: 0 }));
    const client = createWebApiClient('http://example.test/api');
    await client.subscribeList('assignments', 2, 25, () => undefined, undefined, {
      search: ' plan ', ids: ['b', 'a', 'b'], sort: { column: 'Name', order: 'ASC' }, filters: { projectId: 'p-1' },
    });
    const url = new URL(String(fetchMock.mock.calls[0][0]));
    expect(url.searchParams.get('search')).toBe('plan');
    expect(url.searchParams.get('ids')).toBe('b,a');
    expect(url.searchParams.get('projectId')).toBe('p-1');
    expect(url.searchParams.get('sortColumn')).toBe('Name');
    expect(url.searchParams.get('pageNumber')).toBe('2');
  });

  it('collects every page for listAll and stops at the limit', async () => {
    const page = (count: number, total: number) => new Response(JSON.stringify({ items: Array.from({ length: count }, (_, index) => ({ id: String(index) })), totalCount: total }), { status: 200 });
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValueOnce(page(100, 230)).mockResolvedValueOnce(page(100, 230)).mockResolvedValueOnce(page(30, 230));
    const client = createWebApiClient('http://example.test/api');
    expect(await client.listAll('assignments')).toHaveLength(230);
    expect(fetchMock).toHaveBeenCalledTimes(3);
    fetchMock.mockReset().mockImplementation(async () => page(100, 900));
    expect(await client.listAll('assignments', {}, undefined, 150)).toHaveLength(150);
    expect(fetchMock).toHaveBeenCalledTimes(2);
  });

  it('restores soft-deleted records with the same 1-100 distinct id rule', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(JSON.stringify({ success: true }), { status: 200 }));
    const client = createWebApiClient('http://example.test/api');
    await client.restore('assignments', ['a', 'a', 'b']);
    expect(fetchMock.mock.calls[0][0]).toBe('http://example.test/api/assignment/restore');
    expect((fetchMock.mock.calls[0][1] as RequestInit).method).toBe('POST');
    expect(JSON.parse((fetchMock.mock.calls[0][1] as RequestInit).body as string)).toEqual({ ids: ['a', 'b'] });
    await expect(client.restore('assignments', [])).rejects.toMatchObject({ status: 400 });
  });

  it('records requests and listing streams for the request inspector, without headers or bodies', async () => {
    clearRequestLog();
    vi.spyOn(globalThis, 'fetch').mockResolvedValueOnce(new Response(JSON.stringify({ id: 'abc' }), { status: 200 })).mockResolvedValueOnce(sseResponse({ items: [], totalCount: 0 }));
    const client = createWebApiClient('http://example.test/api');
    await client.update('projects', 'abc', { name: 'Secret name' });
    await client.subscribeList('projects', 1, 10, () => undefined);
    const [stream, update] = getRequestLog();
    expect(update).toMatchObject({ method: 'PATCH', url: 'http://example.test/api/project?id=abc', status: 200, kind: 'json', open: false });
    expect(stream).toMatchObject({ method: 'GET', kind: 'stream', status: 200, events: 1, open: false });
    expect(JSON.stringify(getRequestLog())).not.toContain('Secret name');
  });

  it('sends 1-100 distinct ids in atomic removal requests', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response(null, { status: 204 }));
    const client = createWebApiClient('http://example.test/api');
    await client.delete('organizations', ['a', 'a', 'b']);
    expect(JSON.parse((fetchMock.mock.calls[0][1] as RequestInit).body as string)).toEqual({ ids: ['a', 'b'] });
    await expect(client.delete('organizations', [])).rejects.toMatchObject({ status: 400 });
    await expect(client.delete('organizations', Array.from({ length: 101 }, (_, index) => `id-${index}`))).rejects.toMatchObject({ status: 400 });
    expect(fetchMock).toHaveBeenCalledTimes(1);
  });

  it('unwraps singular item response envelopes for relation lookups', async () => {
    const client = createWebApiClient('http://example.test/api');
    const fetchMock = vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(new Response(JSON.stringify({
        organization: { id: 'org-1', name: 'Cpnucleo Core' },
      }), { status: 200, headers: { 'Content-Type': 'application/json' } }))
      .mockResolvedValueOnce(new Response(JSON.stringify({
        result: { id: 'org-2', name: 'Cpnucleo Result' },
      }), { status: 200, headers: { 'Content-Type': 'application/json' } }))
      .mockResolvedValueOnce(new Response(JSON.stringify({
        id: 'org-3', name: 'Cpnucleo Raw',
      }), { status: 200, headers: { 'Content-Type': 'application/json' } }));

    await expect(client.get('organizations', 'org-1')).resolves.toEqual({ id: 'org-1', name: 'Cpnucleo Core' });
    await expect(client.get('organizations', 'org-2')).resolves.toEqual({ id: 'org-2', name: 'Cpnucleo Result' });
    await expect(client.get('organizations', 'org-3')).resolves.toEqual({ id: 'org-3', name: 'Cpnucleo Raw' });
    expect(fetchMock).toHaveBeenCalledTimes(3);
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

  it("uses the stream's own first snapshot without a separate JSON request", async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(sseResponse({ result: { data: [{ id: '1' }], totalCount: 3, pageNumber: 1, pageSize: 1 } }));
    const client = createWebApiClient('http://example.test/api');
    const onPage = vi.fn();
    await client.subscribeList('projects', 1, 1, onPage);
    expect(fetchMock).toHaveBeenCalledTimes(1);
    expect(new Headers((fetchMock.mock.calls[0][1] as RequestInit).headers).get('Accept')).toBe('text/event-stream');
    expect(Object.fromEntries(new URL(String(fetchMock.mock.calls[0][0])).searchParams)).toEqual({ pageNumber: '1', pageSize: '1', sortColumn: 'CreatedAt', sortOrder: 'ASC' });
    expect(onPage).toHaveBeenCalledExactlyOnceWith(expect.objectContaining({ totalCount: 3, items: [{ id: '1' }] }), { live: true });
  });

  it('delivers snapshots split across chunks, CRLF events, heartbeats and multi-line data', async () => {
    const encoder = new TextEncoder();
    const chunks = [
      ': heartbeat\r\n\r\nevent: listing\r\ndata: {"items":[{"id":"a"}],',
      '"totalCount":1}\r\n\r\nid: 2\ndata: {"items":[],\ndata: "totalCount":0}\n\n',
    ];
    const body = new ReadableStream<Uint8Array>({ start(controller) { chunks.forEach(chunk => controller.enqueue(encoder.encode(chunk))); controller.close(); } });
    vi.spyOn(globalThis, 'fetch').mockResolvedValueOnce(new Response(body, { status: 200, headers: { 'Content-Type': 'text/event-stream; charset=utf-8' } }));
    const onPage = vi.fn();
    await createWebApiClient('http://example.test/api').subscribeList('projects', 1, 10, onPage);
    expect(onPage.mock.calls.map(([page, info]) => [page.totalCount, page.items.map((item: { id: string }) => item.id), info.live])).toEqual([
      [1, ['a'], true],
      [0, [], true],
    ]);
    expect(parseServerSentEventData('data: one\ndata:two\ndata:  three')).toEqual(['one\ntwo\n three']);
    expect(parseServerSentEventData(': comment only')).toEqual([]);
  });

  it('treats a non-SSE response as one non-live snapshot', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(new Response(JSON.stringify({ result: { data: [{ id: 'seed' }], totalCount: 1, pageNumber: 1, pageSize: 25 } }), { status: 200, headers: { 'Content-Type': 'application/json' } }));
    const client = createWebApiClient('http://example.test/api');
    const onPage = vi.fn();
    await expect(client.subscribeList('projects', 1, 25, onPage)).resolves.toBeUndefined();
    expect(fetchMock).toHaveBeenCalledTimes(1);
    expect(onPage).toHaveBeenCalledWith(expect.objectContaining({ totalCount: 1, items: [{ id: 'seed' }] }), { live: false });
  });

  it('fails closed when the SSE stream ends before data arrives', async () => {
    vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(new Response('', { status: 200, headers: { 'Content-Type': 'text/event-stream' } }));
    const client = createWebApiClient('http://example.test/api');
    await expect(client.subscribeList('projects', 1, 25, () => undefined)).rejects.toMatchObject({ name: 'ApiError', message: 'The listing stream ended before sending data.' });
  });

  it('normalizes malformed SSE payloads as API errors', async () => {
    vi.spyOn(globalThis, 'fetch')
      .mockResolvedValueOnce(new Response('event: listing\ndata: {nope}\n\n', { status: 200, headers: { 'Content-Type': 'text/event-stream' } }));
    const client = createWebApiClient('http://example.test/api');
    await expect(client.subscribeList('projects', 1, 25, () => undefined)).rejects.toMatchObject({ name: 'ApiError', status: 0 });
  });

  it('ends the session on a 401 stream response, like JSON requests do', async () => {
    const assign = vi.fn();
    Object.defineProperty(globalThis, 'window', { value: { location: { pathname: '/projects/', search: '', hash: '', assign } }, configurable: true });
    const storage = new Map<string, string>([['cpnucleo.jwt', 'stale']]);
    Object.defineProperty(globalThis, 'sessionStorage', {
      value: { getItem: (key: string) => storage.get(key) ?? null, setItem: (key: string, value: string) => storage.set(key, value), removeItem: (key: string) => storage.delete(key) },
      configurable: true,
    });
    try {
      vi.spyOn(globalThis, 'fetch').mockResolvedValueOnce(new Response(JSON.stringify({ statusCode: 401, message: 'Your session expired.' }), { status: 401, headers: { 'Content-Type': 'application/json' } }));
      await expect(createWebApiClient('http://example.test/api').subscribeList('projects', 1, 10, () => undefined))
        .rejects.toMatchObject({ name: 'ApiError', status: 401, message: 'Your session expired.' });
      expect(assign).toHaveBeenCalledWith('/login/?returnUrl=%2Fprojects%2F');
      expect(storage.has('cpnucleo.jwt')).toBe(false);
    } finally {
      Reflect.deleteProperty(globalThis, 'window');
      Reflect.deleteProperty(globalThis, 'sessionStorage');
    }
  });

  it('uses server error messages for other stream failures without signing out', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValueOnce(new Response(JSON.stringify({ statusCode: 403, message: 'Administrator access is required.' }), { status: 403 }));
    await expect(createWebApiClient('http://example.test/api').subscribeList('users', 1, 10, () => undefined))
      .rejects.toMatchObject({ status: 403, message: 'Administrator access is required.' });
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
