import { clearRequestLog, getRequestLog, subscribeRequestLog, type RequestLogEntry } from '~/lib/api/request-log';

export const inspectorStorageKey = 'cpnucleo.inspectorOpen';

const describeUrl = (url: string) => {
  try {
    const parsed = new URL(url);
    return { host: parsed.host, path: `${parsed.pathname}${parsed.search}` };
  } catch {
    return { host: '', path: url };
  }
};

/** Outcome text and tone for one request, as shown in the inspector. */
export const describeEntry = (entry: RequestLogEntry): { label: string; tone: string } => {
  if (entry.error && entry.status === null) return { label: entry.error, tone: 'text-danger' };
  if (entry.status === null) return { label: 'Pending', tone: 'text-subtle' };
  const tone = entry.status >= 500 ? 'text-danger' : entry.status >= 400 ? 'text-warning' : 'text-success';
  if (entry.kind === 'stream' && entry.open) return { label: `${entry.status} · live · ${entry.events} snapshot${entry.events === 1 ? '' : 's'}`, tone };
  if (entry.kind === 'stream') return { label: `${entry.status} · closed · ${entry.events} snapshot${entry.events === 1 ? '' : 's'}`, tone };
  return { label: String(entry.status), tone };
};

/**
 * Non-modal panel listing the API calls this tab makes, newest first, so a click can be followed
 * from the page to the REST endpoint (and from there into the API's traces and PostgreSQL).
 */
export const mountRequestInspector = (panel: HTMLElement, toggles: HTMLButtonElement[]): (() => void) => {
  const list = panel.querySelector<HTMLElement>('[data-inspector-list]')!;
  const empty = panel.querySelector<HTMLElement>('[data-inspector-empty]')!;
  const lifetime = new AbortController();
  let frame = 0;

  const render = () => {
    frame = 0;
    if (panel.hidden) return;
    const entries = getRequestLog();
    empty.hidden = entries.length > 0;
    list.replaceChildren(...entries.map(entry => {
      const { host, path } = describeUrl(entry.url);
      const outcome = describeEntry(entry);
      const row = document.createElement('li');
      row.className = 'grid gap-1 px-4 py-2.5';
      const top = document.createElement('div');
      top.className = 'flex items-center gap-2 text-xs';
      const method = document.createElement('span');
      method.className = 'method-badge';
      method.textContent = entry.kind === 'stream' ? 'SSE' : entry.method;
      const status = document.createElement('span');
      status.className = `font-medium ${outcome.tone}`;
      status.textContent = outcome.label;
      const timing = document.createElement('span');
      timing.className = 'ml-auto font-mono tabular-nums text-subtle';
      timing.textContent = entry.durationMs === null ? '…' : `${entry.durationMs} ms`;
      top.append(method, status, timing);
      const url = document.createElement('p');
      url.className = 'break-all font-mono text-[0.75rem] text-muted';
      url.textContent = path;
      url.title = `${host}${path}`;
      row.append(top, url);
      return row;
    }));
  };
  const schedule = () => { if (!frame) frame = requestAnimationFrame(render); };

  const setOpen = (open: boolean) => {
    panel.hidden = !open;
    toggles.forEach(toggle => toggle.setAttribute('aria-expanded', String(open)));
    try { sessionStorage.setItem(inspectorStorageKey, open ? '1' : ''); } catch { /* storage unavailable */ }
    if (open) render();
  };

  toggles.forEach(toggle => toggle.addEventListener('click', () => setOpen(Boolean(panel.hidden)), { signal: lifetime.signal }));
  panel.querySelector('[data-inspector-close]')!.addEventListener('click', () => { setOpen(false); toggles[0]?.focus(); }, { signal: lifetime.signal });
  panel.querySelector('[data-inspector-clear]')!.addEventListener('click', () => clearRequestLog(), { signal: lifetime.signal });
  panel.addEventListener('keydown', event => { if (event.key === 'Escape') { setOpen(false); toggles[0]?.focus(); } }, { signal: lifetime.signal });
  const unsubscribe = subscribeRequestLog(schedule);
  setOpen(sessionStorage.getItem(inspectorStorageKey) === '1');

  return () => { unsubscribe(); lifetime.abort(); cancelAnimationFrame(frame); };
};
