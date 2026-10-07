import { getSessionClaims, type SessionClaims } from '~/lib/api/http-client';
import type { ApiEntity } from '~/lib/api/types';
import { webApiClient } from '~/lib/api/webapi-client';
import { addUtcDays, formatUtcDay, formatUtcTime, formatWeekday, isoDay, parseIsoDay, startOfUtcDay, startOfUtcWeek, sumHours } from '~/lib/dates';
import { element, label, link, loadLabels } from '~/features/records/related';

export interface CalendarState {
  /** Monday 00:00 UTC. */
  week: Date;
  mine: boolean;
}

export const parseCalendarState = (search: string, now = new Date()): CalendarState => {
  const params = new URLSearchParams(search);
  return { week: startOfUtcWeek(parseIsoDay(params.get('week')) ?? now), mine: params.get('all') !== '1' };
};

export const serializeCalendarState = ({ week, mine }: CalendarState, now = new Date()) => {
  const params = new URLSearchParams();
  if (week.getTime() !== startOfUtcWeek(now).getTime()) params.set('week', isoDay(week));
  if (!mine) params.set('all', '1');
  const query = params.toString();
  return query ? `?${query}` : '';
};

/** Groups calendar items by their UTC day within the week (index 0 = Monday). */
export const groupByWeekday = (entries: ApiEntity[], week: Date): ApiEntity[][] => {
  const days: ApiEntity[][] = Array.from({ length: 7 }, () => []);
  for (const entry of entries) {
    const date = new Date(String(entry.keepDate));
    const index = Math.floor((startOfUtcDay(date).getTime() - week.getTime()) / 86_400_000);
    if (index >= 0 && index < 7) days[index].push(entry);
  }
  return days;
};

interface CalendarOptions {
  location?: Pick<Location, 'pathname' | 'search'>;
  history?: Pick<History, 'replaceState' | 'state'>;
}

/** Week view of calendar items (time logged against tasks), with daily and weekly hour totals. */
export const mountCalendar = (root: HTMLElement, session: SessionClaims | null = getSessionClaims(), { location = window.location, history = window.history }: CalendarOptions = {}): (() => void) => {
  const query = <T extends Element>(selector: string) => root.querySelector<T>(selector)!;
  const state = parseCalendarState(location.search);
  const grid = query<HTMLElement>('[data-calendar-days]');
  const status = query<HTMLElement>('[data-calendar-status]');
  const error = query<HTMLElement>('[data-calendar-error]');
  const mine = query<HTMLInputElement>('[data-calendar-mine]');
  const lifetime = new AbortController();
  let request = new AbortController();
  mine.checked = state.mine;

  const load = async () => {
    request.abort(); request = new AbortController();
    const signal = request.signal;
    history.replaceState(history.state, '', `${location.pathname}${serializeCalendarState(state)}`);
    const end = addUtcDays(state.week, 7);
    query<HTMLElement>('[data-calendar-range]').textContent = `${formatUtcDay(state.week)} – ${formatUtcDay(addUtcDays(end, -1))}`;
    status.textContent = 'Loading…';
    error.hidden = true;
    grid.setAttribute('aria-busy', 'true');
    try {
      const entries = await webApiClient.listAll<ApiEntity>('appointments', {
        dateFrom: state.week.toISOString(),
        dateTo: end.toISOString(),
        filters: state.mine && session ? { userId: session.sub } : {},
        sort: { column: 'KeepDate', order: 'ASC' },
      }, signal);
      const [tasks, people] = await Promise.all([
        loadLabels('assignments', entries.map(entry => entry.assignmentId), signal, session),
        loadLabels('users', entries.map(entry => entry.userId), signal, session),
      ]);
      if (signal.aborted) return;
      render(entries, tasks, people);
      const total = sumHours(entries);
      status.textContent = `${entries.length} item${entries.length === 1 ? '' : 's'} · ${total} hour${total === 1 ? '' : 's'} this week`;
    } catch (failure) {
      if (signal.aborted) return;
      status.textContent = '';
      error.textContent = failure instanceof Error ? failure.message : 'Unable to load the calendar.';
      error.hidden = false;
    } finally {
      if (!signal.aborted) grid.setAttribute('aria-busy', 'false');
    }
  };

  const render = (entries: ApiEntity[], tasks: Map<string, string>, people: Map<string, string>) => {
    const today = isoDay(new Date());
    grid.replaceChildren(...groupByWeekday(entries, state.week).map((dayEntries, index) => {
      const day = addUtcDays(state.week, index);
      const isToday = isoDay(day) === today;
      const column = element('section', `calendar-day${isToday ? ' calendar-today' : ''}`);
      const headingId = `day-${isoDay(day)}`;
      column.setAttribute('aria-labelledby', headingId);
      const header = element('header', 'flex items-baseline justify-between gap-2 border-b border-line px-3 py-2');
      const title = element('h2', `text-sm font-semibold${isToday ? ' text-accent-text' : ''}`, formatWeekday(day));
      title.id = headingId;
      const hours = sumHours(dayEntries);
      header.append(title, element('span', 'font-mono text-xs tabular-nums text-subtle', hours ? `${hours} h` : ''));
      const list = element('ul', 'grid gap-1.5 p-2');
      list.append(...dayEntries.map(entry => {
        const item = element('li', 'calendar-entry');
        const top = element('div', 'flex items-center justify-between gap-2 text-xs text-subtle');
        top.append(element('span', 'tabular-nums', `${formatUtcTime(entry.keepDate)} UTC`), element('span', 'font-mono tabular-nums', `${Number(entry.amountHours) || 0} h`));
        const task = typeof entry.assignmentId === 'string'
          ? link(`/assignments/view/?id=${encodeURIComponent(entry.assignmentId)}`, label(tasks, entry.assignmentId), 'table-link text-xs')
          : element('span', 'text-xs text-subtle', '—');
        item.append(top, element('p', 'mt-1 line-clamp-3 text-[0.8125rem]', String(entry.description ?? '')), task);
        if (!state.mine) item.append(element('p', 'mt-0.5 truncate text-xs text-subtle', label(people, entry.userId)));
        return item;
      }));
      if (dayEntries.length === 0) list.append(element('li', 'px-1 py-3 text-center text-xs text-subtle', 'Nothing logged'));
      column.append(header, list);
      return column;
    }));
  };

  root.addEventListener('click', event => {
    const target = (event.target as Element).closest<HTMLButtonElement>('button[data-week]');
    if (!target) return;
    const step = target.dataset.week;
    state.week = step === 'today' ? startOfUtcWeek(new Date()) : addUtcDays(state.week, step === 'next' ? 7 : -7);
    void load();
  }, { signal: lifetime.signal });
  mine.addEventListener('change', () => { state.mine = mine.checked; void load(); }, { signal: lifetime.signal });
  void load();
  return () => { lifetime.abort(); request.abort(); };
};
