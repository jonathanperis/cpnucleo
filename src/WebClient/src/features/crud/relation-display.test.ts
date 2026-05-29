import { describe, expect, it } from 'vitest';
import type { ApiEntity, FieldMetadata } from '~/lib/api/types';
import { displayEntityLabel, displayFieldValue, collectMissingRelationIds } from './relation-display';

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

  it('skips empty display fields before falling back to the next readable value', () => {
    expect(displayEntityLabel({ id: 'project-1', name: '', description: 'Readable description' })).toBe('Readable description');
  });

  it('collects only visible relation ids missing from the prefetched relation records', () => {
    const fields = [
      { name: 'projectId', label: 'Project', type: 'guid', relation: 'projects' },
      { name: 'workflowId', label: 'Progress step', type: 'guid', relation: 'workflows' },
      { name: 'name', label: 'Name', type: 'text' },
    ] satisfies FieldMetadata[];

    expect(collectMissingRelationIds([
      { projectId: 'project-1', workflowId: 'todo' },
      { projectId: 'project-2', workflowId: 'todo' },
      { projectId: 'project-2', workflowId: '' },
    ], fields, {
      projects: [{ id: 'project-1', name: 'Already loaded' }],
      workflows: [],
    })).toEqual({
      projects: ['project-2'],
      workflows: ['todo'],
    });
  });
});
