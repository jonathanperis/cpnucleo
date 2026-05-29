import { describe, expect, it } from 'vitest';

import { formatFormFieldValue, relationOptionsLoaded, withSelectedRelationOption } from './crud-field-values';

describe('CRUD edit form field values', () => {
  it('formats existing date and datetime values for browser edit controls', () => {
    expect(formatFormFieldValue('2026-05-29T18:42:31.123Z', 'date')).toBe('2026-05-29');
    expect(formatFormFieldValue('2026-05-29T18:42:31.123Z', 'datetime-local')).toBe('2026-05-29T18:42');
    expect(formatFormFieldValue(new Date(Date.UTC(2026, 4, 29, 18, 42)), 'datetime-local')).toBe('2026-05-29T18:42');
  });

  it('keeps relation ids and numeric values available for select and number controls', () => {
    expect(formatFormFieldValue('018fd27a-2145-7dd0-93b4-5d06071c2e9d', 'guid')).toBe('018fd27a-2145-7dd0-93b4-5d06071c2e9d');
    expect(formatFormFieldValue(12.5, 'number')).toBe('12.5');
    expect(formatFormFieldValue(null, 'text')).toBe('');
  });

  it('includes the selected relation id when it is outside the prefetched option page', () => {
    expect(relationOptionsLoaded({}, 'organizations')).toBe(false);
    expect(relationOptionsLoaded({ organizations: [] }, 'organizations')).toBe(true);

    const selected = '019e6fc4-5cf1-7252-b14c-979447ccda20';
    const options = withSelectedRelationOption([{ id: 'other' }], selected);

    expect(options[0]).toEqual({ id: selected });
    expect(withSelectedRelationOption(options, selected)).toHaveLength(2);
  });
});
