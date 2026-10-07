// @vitest-environment jsdom
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import type { SessionClaims } from '~/lib/api/http-client';
import type { ApiEntity } from '~/lib/api/types';
import { webApiClient } from '~/lib/api/webapi-client';
import { showBuiltPage } from '~/test/page-markup';
import { mountBoard, taskUpdatePayload } from './board-controller';

const admin: SessionClaims = { sub: 'admin-1', login: 'admin@cpnucleo.test', isAdmin: true };
const steps = [{ id: 'todo', name: 'To do', order: 1 }, { id: 'done', name: 'Done', order: 2 }];
const task: ApiEntity = {
  id: 'task-1', name: 'Plan', description: 'Write it', startDate: '2026-10-01T00:00:00Z', endDate: '2099-10-02T00:00:00Z',
  amountHours: 3, projectId: 'p-1', workflowId: 'todo', userId: 'admin-1', assignmentTypeId: 'type-1', createdAt: '2026-10-01T00:00:00Z',
};
let stop = () => {};
let history = { state: null as unknown, replaceState: vi.fn() };

const mount = (search = '') => {
  showBuiltPage('board');
  history = { state: null, replaceState: vi.fn() };
  stop = mountBoard(document.querySelector<HTMLElement>('[data-board]')!, admin, { location: { pathname: '/board/', search }, history });
  return document.querySelector<HTMLElement>('[data-board]')!;
};

beforeEach(() => {
  vi.spyOn(webApiClient, 'listAll').mockImplementation(async key => (key === 'workflows' ? steps : [{ id: 'p-1', name: 'Atlas' }, { id: 'p-2', name: 'Borealis' }]));
  vi.spyOn(webApiClient, 'lookup').mockResolvedValue([{ id: 'admin-1', name: 'Admin' }]);
  vi.spyOn(webApiClient, 'subscribeList').mockImplementation(async (_key, _page, _size, onPage, signal) => {
    onPage({ items: [{ ...task }], totalCount: 1 }, { live: true });
    await new Promise<void>(done => signal?.addEventListener('abort', () => done(), { once: true }));
  });
});
afterEach(() => { stop(); vi.restoreAllMocks(); document.body.replaceChildren(); });

describe('task board', () => {
  it('builds the full update body with only the changed field', () => {
    expect(taskUpdatePayload(task, { workflowId: 'done' })).toEqual({
      name: 'Plan', description: 'Write it', startDate: '2026-10-01T00:00:00Z', endDate: '2099-10-02T00:00:00Z',
      amountHours: 3, projectId: 'p-1', workflowId: 'done', userId: 'admin-1', assignmentTypeId: 'type-1',
    });
  });

  it('shows one column per step for the first project and follows its live listing', async () => {
    const root = mount();
    await vi.waitFor(() => expect(root.querySelectorAll('[data-step-id]')).toHaveLength(2));
    expect(vi.mocked(webApiClient.subscribeList).mock.calls[0][5]).toMatchObject({ filters: { projectId: 'p-1' } });
    expect(history.replaceState).toHaveBeenLastCalledWith(null, '', '/board/?projectId=p-1');
    expect(root.querySelector('[data-step-id="todo"]')?.textContent).toContain('Plan');
    expect(root.querySelector('[data-board-status]')?.textContent).toBe('1 task · Live');
  });

  it('moves a card with its “Move to” select and confirms it', async () => {
    const update = vi.spyOn(webApiClient, 'update').mockResolvedValue({});
    const root = mount('?projectId=p-2');
    await vi.waitFor(() => expect(vi.mocked(webApiClient.subscribeList).mock.calls[0]?.[5]).toMatchObject({ filters: { projectId: 'p-2' } }));
    const select = await vi.waitFor(() => { const element = root.querySelector<HTMLSelectElement>('[data-move-task="task-1"]'); expect(element).not.toBeNull(); return element!; });
    expect(select.getAttribute('aria-label')).toBe('Move Plan to another step');
    select.value = 'done';
    select.dispatchEvent(new Event('change', { bubbles: true }));
    expect(root.querySelector('[data-step-id="done"]')?.textContent).toContain('Plan');
    await vi.waitFor(() => expect(update).toHaveBeenCalledWith('assignments', 'task-1', expect.objectContaining({ workflowId: 'done', name: 'Plan' })));
  });

  it('puts the card back and explains when the move is rejected', async () => {
    vi.spyOn(webApiClient, 'update').mockRejectedValue(new Error('You are not a member of this project.'));
    const root = mount();
    const select = await vi.waitFor(() => { const element = root.querySelector<HTMLSelectElement>('[data-move-task="task-1"]'); expect(element).not.toBeNull(); return element!; });
    select.value = 'done';
    select.dispatchEvent(new Event('change', { bubbles: true }));
    await vi.waitFor(() => expect(document.querySelector('[data-toasts]')?.textContent).toContain('You are not a member of this project.'));
    expect(root.querySelector('[data-step-id="todo"]')?.textContent).toContain('Plan');
  });
});
