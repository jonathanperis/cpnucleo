import { describe, expect, it, vi } from 'vitest';
import { beginRequest, clearRequestLog, getRequestLog, MAX_LOG_ENTRIES, subscribeRequestLog } from './request-log';
import { describeEntry } from '~/features/inspector/request-inspector';

describe('request log', () => {
  it('keeps the newest entries first, bounded, and notifies subscribers', () => {
    clearRequestLog();
    const listener = vi.fn();
    const unsubscribe = subscribeRequestLog(listener);
    for (let index = 0; index < MAX_LOG_ENTRIES + 5; index += 1) beginRequest('get', `http://api.test/${index}`, 'json').finish();
    expect(getRequestLog()).toHaveLength(MAX_LOG_ENTRIES);
    expect(getRequestLog()[0].url).toBe(`http://api.test/${MAX_LOG_ENTRIES + 4}`);
    expect(getRequestLog()[0].method).toBe('GET');
    expect(listener).toHaveBeenCalled();
    unsubscribe();
  });

  it('describes outcomes for the inspector', () => {
    clearRequestLog();
    const failed = beginRequest('GET', 'http://api.test/a', 'json');
    failed.finish('Network error');
    const stream = beginRequest('GET', 'http://api.test/b', 'stream');
    stream.respond(200); stream.event(); stream.event();
    const rejected = beginRequest('DELETE', 'http://api.test/c', 'json');
    rejected.respond(409); rejected.finish();
    const [conflict, live, error] = getRequestLog();
    expect(describeEntry(error)).toEqual({ label: 'Network error', tone: 'text-danger' });
    expect(describeEntry(live)).toEqual({ label: '200 · live · 2 snapshots', tone: 'text-success' });
    expect(describeEntry(conflict)).toEqual({ label: '409', tone: 'text-warning' });
    stream.finish();
    expect(describeEntry(getRequestLog()[1]).label).toBe('200 · closed · 2 snapshots');
  });
});
