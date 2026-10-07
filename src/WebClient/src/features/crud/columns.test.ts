import { describe, expect, it } from 'vitest';
import { findResource } from '~/lib/api/resource-metadata';
import { columnsStorageKey, readHiddenColumns, visibleColumns, writeHiddenColumns } from './columns';

const storage = () => {
  const values = new Map<string, string>();
  return { getItem: (key: string) => values.get(key) ?? null, setItem: (key: string, value: string) => { values.set(key, value); } };
};

describe('column preferences', () => {
  const tasks = findResource('assignments');

  it('hides the resource defaults until the person chooses otherwise', () => {
    const store = storage();
    expect([...readHiddenColumns(tasks, store)]).toEqual(['description', 'assignmentTypeId']);
    writeHiddenColumns(tasks, new Set(['hours', 'amountHours']), store);
    expect([...readHiddenColumns(tasks, store)]).toEqual(['amountHours']);
    expect(store.getItem(columnsStorageKey(tasks))).toBe('["hours","amountHours"]');
  });

  it('never hides the first column and ignores unreadable preferences', () => {
    const store = storage();
    store.setItem(columnsStorageKey(tasks), '{oops');
    expect([...readHiddenColumns(tasks, store)]).toEqual(['description', 'assignmentTypeId']);
    expect(visibleColumns(tasks, new Set(['name', 'startDate'])).map(field => field.name)).toEqual(
      ['name', 'description', 'endDate', 'amountHours', 'projectId', 'workflowId', 'userId', 'assignmentTypeId', 'createdAt']);
  });
});
