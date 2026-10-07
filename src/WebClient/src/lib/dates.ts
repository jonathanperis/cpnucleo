/**
 * Calendar math in UTC. Task dates are stored as UTC calendar days and calendar item times are
 * edited in UTC, so weeks, "today" and "overdue" are UTC too; that keeps every view consistent
 * with what the forms show.
 */
export const DAY_MS = 24 * 60 * 60 * 1000;

export const startOfUtcDay = (date: Date) => new Date(Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate()));

/** Monday 00:00 UTC of the week containing `date`. */
export const startOfUtcWeek = (date: Date) => {
  const day = startOfUtcDay(date);
  const offset = (day.getUTCDay() + 6) % 7;
  return new Date(day.getTime() - offset * DAY_MS);
};

export const addUtcDays = (date: Date, days: number) => new Date(date.getTime() + days * DAY_MS);

/** `YYYY-MM-DD` of a UTC day. */
export const isoDay = (date: Date) => date.toISOString().slice(0, 10);

/** Parses `YYYY-MM-DD` as a UTC day; undefined when malformed. */
export const parseIsoDay = (value: string | null | undefined): Date | undefined => {
  if (!value || !/^\d{4}-\d{2}-\d{2}$/.test(value)) return undefined;
  const date = new Date(`${value}T00:00:00.000Z`);
  return Number.isNaN(date.getTime()) || isoDay(date) !== value ? undefined : date;
};

const toDate = (value: unknown) => {
  if (typeof value !== 'string' && !(value instanceof Date)) return undefined;
  const date = new Date(value);
  return Number.isNaN(date.getTime()) ? undefined : date;
};

const dayFormat = new Intl.DateTimeFormat(undefined, { dateStyle: 'medium', timeZone: 'UTC' });
const weekdayFormat = new Intl.DateTimeFormat(undefined, { weekday: 'short', day: 'numeric', month: 'short', timeZone: 'UTC' });
const timeFormat = new Intl.DateTimeFormat(undefined, { hour: '2-digit', minute: '2-digit', timeZone: 'UTC' });

export const formatUtcDay = (value: unknown) => {
  const date = toDate(value);
  return date ? dayFormat.format(date) : '—';
};
export const formatWeekday = (date: Date) => weekdayFormat.format(date);
export const formatUtcTime = (value: unknown) => {
  const date = toDate(value);
  return date ? timeFormat.format(date) : '';
};

/** True when a task's end day is before today (UTC). */
export const isPastDay = (value: unknown, now = new Date()) => {
  const date = toDate(value);
  return date !== undefined && startOfUtcDay(date).getTime() < startOfUtcDay(now).getTime();
};

/** Whole days from today (UTC) to `value`; negative when it is in the past. */
export const daysUntil = (value: unknown, now = new Date()) => {
  const date = toDate(value);
  return date === undefined ? undefined : Math.round((startOfUtcDay(date).getTime() - startOfUtcDay(now).getTime()) / DAY_MS);
};

export const sumHours = (records: Record<string, unknown>[]) =>
  records.reduce((total, record) => total + (typeof record.amountHours === 'number' ? record.amountHours : Number(record.amountHours) || 0), 0);
