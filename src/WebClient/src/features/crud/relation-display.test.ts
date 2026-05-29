import { describe, expect, it } from 'vitest';
import type { ApiEntity } from '~/lib/api/types';
import { displayEntityLabel, displayFieldValue } from './relation-display';

const organizations: ApiEntity[] = [
  { id: 'org-1', name: 'Cpnucleo Core' },
];

const tasks: ApiEntity[] = [
  { id: 'task-1', description: 'Write project plan' },
];

describe('CRUD relation display values', () => {
  it('uses related record names in table cells instead of raw ids', () => {
    expect(displayFieldValue('org-1', 'organizations', { organizations })).toBe('Cpnucleo Core');
  });

  it('falls back to related descriptions when a related record has no name', () => {
    expect(displayFieldValue('task-1', 'assignments', { assignments: tasks })).toBe('Write project plan');
  });

  it('keeps the raw id visible when the related record is not loaded yet', () => {
    expect(displayFieldValue('missing-id', 'organizations', { organizations })).toBe('missing-id');
  });

  it('chooses a readable label before an id for relation options and details', () => {
    expect(displayEntityLabel({ id: 'user-1', login: 'demo@cpnucleo.test' })).toBe('demo@cpnucleo.test');
  });
});
