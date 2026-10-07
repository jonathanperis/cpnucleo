import { describe, expect, it } from 'vitest';
import { addUtcDays, daysUntil, isPastDay, isoDay, parseIsoDay, startOfUtcWeek, sumHours } from './dates';

describe('UTC calendar helpers', () => {
  it('starts weeks on Monday in UTC', () => {
    expect(isoDay(startOfUtcWeek(new Date('2026-10-07T23:30:00Z')))).toBe('2026-10-05');
    expect(isoDay(startOfUtcWeek(new Date('2026-10-11T12:00:00Z')))).toBe('2026-10-05');
    expect(isoDay(startOfUtcWeek(new Date('2026-10-12T00:00:00Z')))).toBe('2026-10-12');
    expect(isoDay(addUtcDays(new Date('2026-10-05T00:00:00Z'), 7))).toBe('2026-10-12');
  });

  it('parses only real YYYY-MM-DD days', () => {
    expect(parseIsoDay('2026-10-05')?.toISOString()).toBe('2026-10-05T00:00:00.000Z');
    expect(parseIsoDay('2026-02-30')).toBeUndefined();
    expect(parseIsoDay('10/05/2026')).toBeUndefined();
    expect(parseIsoDay(null)).toBeUndefined();
  });

  it('compares task days with today in UTC', () => {
    const now = new Date('2026-10-07T10:00:00Z');
    expect(isPastDay('2026-10-06T00:00:00Z', now)).toBe(true);
    expect(isPastDay('2026-10-07T00:00:00Z', now)).toBe(false);
    expect(daysUntil('2026-10-10T00:00:00Z', now)).toBe(3);
    expect(daysUntil('2026-10-04T00:00:00Z', now)).toBe(-3);
    expect(daysUntil('nonsense', now)).toBeUndefined();
  });

  it('sums hours from numbers and numeric strings', () => {
    expect(sumHours([{ amountHours: 2 }, { amountHours: '3' }, { amountHours: 'x' }, {}])).toBe(5);
  });
});
