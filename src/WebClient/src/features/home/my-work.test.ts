import { describe, expect, it } from 'vitest';
import { summarizeTasks } from './my-work';

describe('my work summary', () => {
  it('lists open tasks by due day and counts overdue and due-this-week tasks', () => {
    const steps = [{ id: 'todo', order: 1 }, { id: 'done', order: 2 }];
    const now = new Date('2026-10-07T12:00:00Z');
    const { open, overdue, dueSoon } = summarizeTasks([
      { id: 'late', endDate: '2026-10-01T00:00:00Z', workflowId: 'todo' },
      { id: 'finished', endDate: '2026-09-01T00:00:00Z', workflowId: 'done' },
      { id: 'later', endDate: '2026-11-01T00:00:00Z', workflowId: 'todo' },
      { id: 'soon', endDate: '2026-10-09T00:00:00Z', workflowId: 'todo' },
    ], steps, now);
    expect(open.map(task => task.id)).toEqual(['late', 'soon', 'later']);
    expect(overdue).toBe(1);
    expect(dueSoon).toBe(1);
  });
});
