/**
 * In-memory log of the API calls this tab made, shown by the request inspector so learners can
 * follow a click through the REST API. Only method, URL, status and timing are kept: never
 * headers or bodies, so tokens and passwords cannot end up here.
 */
export interface RequestLogEntry {
  id: number;
  method: string;
  url: string;
  kind: 'json' | 'stream';
  startedAt: number;
  status: number | null;
  durationMs: number | null;
  /** Server-sent snapshots received on a listing stream. */
  events: number;
  error?: string;
  open: boolean;
}

export const MAX_LOG_ENTRIES = 60;

let nextId = 1;
const entries: RequestLogEntry[] = [];
const listeners = new Set<() => void>();
const notify = () => listeners.forEach(listener => listener());

export const getRequestLog = (): readonly RequestLogEntry[] => entries;

export const subscribeRequestLog = (listener: () => void): (() => void) => {
  listeners.add(listener);
  return () => listeners.delete(listener);
};

export const clearRequestLog = () => {
  entries.splice(0);
  notify();
};

export interface RequestLogHandle {
  /** The response arrived (streams stay open afterwards). */
  respond(status: number): void;
  /** A listing stream delivered a snapshot. */
  event(): void;
  /** The request or stream ended; an error message marks a failure. */
  finish(error?: string): void;
}

export const beginRequest = (method: string, url: string, kind: RequestLogEntry['kind']): RequestLogHandle => {
  const startedAt = Date.now();
  const entry: RequestLogEntry = { id: nextId++, method: method.toUpperCase(), url, kind, startedAt, status: null, durationMs: null, events: 0, open: true };
  entries.unshift(entry);
  if (entries.length > MAX_LOG_ENTRIES) entries.length = MAX_LOG_ENTRIES;
  notify();
  return {
    respond(status) {
      entry.status = status;
      entry.durationMs = Date.now() - startedAt;
      notify();
    },
    event() {
      entry.events += 1;
      notify();
    },
    finish(error) {
      if (!entry.open) return;
      entry.open = false;
      entry.durationMs ??= Date.now() - startedAt;
      if (error) entry.error = error;
      notify();
    },
  };
};
