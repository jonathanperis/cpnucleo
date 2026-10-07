// @vitest-environment jsdom
import { afterEach, describe, expect, it, vi } from 'vitest';
import { webApiClient } from '~/lib/api/webapi-client';
import { showBuiltPage } from '~/test/page-markup';
import { groupByWeekday, mountCalendar, parseCalendarState, serializeCalendarState } from './calendar-controller';

afterEach(() => { vi.restoreAllMocks(); vi.useRealTimers(); document.body.replaceChildren(); });

describe('calendar', () => {
  const now = new Date('2026-10-07T12:00:00Z');

  it('keeps the week and the "only mine" choice in the URL', () => {
    expect(parseCalendarState('', now)).toEqual({ week: new Date('2026-10-05T00:00:00Z'), mine: true });
    expect(parseCalendarState('?week=2026-09-30&all=1', now)).toEqual({ week: new Date('2026-09-28T00:00:00Z'), mine: false });
    expect(serializeCalendarState({ week: new Date('2026-10-05T00:00:00Z'), mine: true }, now)).toBe('');
    expect(serializeCalendarState({ week: new Date('2026-09-28T00:00:00Z'), mine: false }, now)).toBe('?week=2026-09-28&all=1');
  });

  it('groups items by UTC weekday and ignores items outside the week', () => {
    const days = groupByWeekday([
      { id: 'a', keepDate: '2026-10-05T23:30:00Z' }, { id: 'b', keepDate: '2026-10-11T08:00:00Z' }, { id: 'c', keepDate: '2026-10-12T00:00:00Z' },
    ], new Date('2026-10-05T00:00:00Z'));
    expect(days.map(day => day.map(entry => entry.id))).toEqual([['a'], [], [], [], [], [], ['b']]);
  });

  it('loads the signed-in person’s week through the date range filter and totals the hours', async () => {
    vi.useFakeTimers({ now, toFake: ['Date'] });
    const listAll = vi.spyOn(webApiClient, 'listAll').mockResolvedValue([
      { id: 'a', keepDate: '2026-10-06T09:00:00Z', amountHours: 2, description: 'Review', assignmentId: 'task-1', userId: 'me' },
      { id: 'b', keepDate: '2026-10-06T14:00:00Z', amountHours: 3, description: 'Pairing', assignmentId: 'task-1', userId: 'me' },
    ]);
    vi.spyOn(webApiClient, 'lookup').mockResolvedValue([{ id: 'task-1', name: 'Plan' }]);
    showBuiltPage('calendar');
    const history = { state: null, replaceState: vi.fn() };
    const root = document.querySelector<HTMLElement>('[data-calendar]')!;
    const stop = mountCalendar(root, { sub: 'me', login: 'me@cpnucleo.test', isAdmin: false }, { location: { pathname: '/calendar/', search: '' }, history });
    await vi.waitFor(() => expect(root.querySelector('[data-calendar-status]')?.textContent).toBe('2 items · 5 hours this week'));
    expect(listAll).toHaveBeenCalledWith('appointments', {
      dateFrom: '2026-10-05T00:00:00.000Z', dateTo: '2026-10-12T00:00:00.000Z', filters: { userId: 'me' }, sort: { column: 'KeepDate', order: 'ASC' },
    }, expect.any(AbortSignal));
    expect(root.querySelectorAll('.calendar-day')).toHaveLength(7);
    expect(root.querySelector('.calendar-today')?.textContent).not.toContain('Review');
    expect(root.querySelectorAll('.calendar-day')[1].textContent).toContain('5 h');
    root.querySelector<HTMLButtonElement>('[data-week="previous"]')!.click();
    await vi.waitFor(() => expect(history.replaceState).toHaveBeenLastCalledWith(null, '', '/calendar/?week=2026-09-28'));
    stop();
  });
});
