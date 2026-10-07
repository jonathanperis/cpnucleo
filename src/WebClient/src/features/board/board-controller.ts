import { getSessionClaims, type SessionClaims } from '~/lib/api/http-client';
import { formFields, findResource } from '~/lib/api/resource-metadata';
import type { ApiEntity } from '~/lib/api/types';
import { MAX_PAGE_SIZE, webApiClient } from '~/lib/api/webapi-client';
import { daysUntil, isPastDay } from '~/lib/dates';
import { createIcon } from '~/lib/icons';
import { showToast } from '~/lib/ui/toast';
import { displayEntityLabel } from '~/features/crud/relation-display';
import { watchListing } from '~/features/crud/watch-list';
import { badge, element, isDoneStep, label, link, loadLabels, loadSteps, type Labels } from '~/features/records/related';

/** The full update body for a task with one field changed (PATCH replaces every editable field). */
export const taskUpdatePayload = (task: ApiEntity, changes: Partial<ApiEntity>): Record<string, unknown> => {
  const payload: Record<string, unknown> = {};
  for (const field of formFields(findResource('assignments'))) {
    const value = field.name in changes ? changes[field.name] : task[field.name];
    if (value !== undefined && value !== null) payload[field.name] = value;
  }
  return payload;
};

interface BoardOptions {
  location?: Pick<Location, 'pathname' | 'search'>;
  history?: Pick<History, 'replaceState' | 'state'>;
}

/**
 * Kanban board: one column per progress step (ordered by `order`), the project's tasks as cards.
 * Cards move by drag and drop or with each card's "Move to" select, which is the keyboard and
 * screen reader path. The board follows the same live listing stream as the Tasks list.
 */
export const mountBoard = (root: HTMLElement, session: SessionClaims | null = getSessionClaims(), { location = window.location, history = window.history }: BoardOptions = {}): (() => void) => {
  const query = <T extends Element>(selector: string) => root.querySelector<T>(selector)!;
  const projectSelect = query<HTMLSelectElement>('[data-board-project]');
  const mineOnly = query<HTMLInputElement>('[data-board-mine]');
  const columns = query<HTMLElement>('[data-board-columns]');
  const statusText = query<HTMLElement>('[data-board-status]');
  const note = query<HTMLElement>('[data-board-note]');
  const error = query<HTMLElement>('[data-board-error]');
  const lifetime = new AbortController();
  let stream = new AbortController();
  let steps: ApiEntity[] = [];
  let tasks: ApiEntity[] = [];
  let owners: Labels = new Map();
  let projectId = new URLSearchParams(location.search).get('projectId') ?? '';
  let dragged: string | null = null;
  const pending = new Map<string, unknown>();

  const fail = (message: string) => { error.textContent = message; error.hidden = false; };

  const card = (task: ApiEntity) => {
    const id = String(task.id);
    const item = element('li', 'board-card');
    item.draggable = true;
    item.dataset.taskId = id;
    const title = link(`/assignments/view/?id=${encodeURIComponent(id)}`, String(task.name ?? 'Task'), 'table-link block font-medium');
    const meta = element('div', 'mt-2 flex flex-wrap items-center gap-1.5 text-xs text-subtle');
    const due = daysUntil(task.endDate);
    if (!isDoneStep(steps, task.workflowId)) {
      if (isPastDay(task.endDate)) meta.append(badge('Overdue', 'danger'));
      else if (due !== undefined && due <= 3) meta.append(badge(due === 0 ? 'Due today' : `Due in ${due}d`, 'warning'));
    }
    meta.append(element('span', 'truncate', label(owners, task.userId)), element('span', 'ml-auto font-mono tabular-nums', `${Number(task.amountHours) || 0} h`));
    const move = element('label', 'mt-2.5 flex items-center gap-2 text-xs text-subtle');
    const select = element('select', 'field field-sm min-w-0 flex-1');
    select.dataset.moveTask = id;
    select.setAttribute('aria-label', `Move ${String(task.name ?? 'task')} to another step`);
    select.append(...steps.map(step => new Option(String(step.name ?? ''), String(step.id), false, step.id === task.workflowId)));
    move.append('Move to', select);
    item.append(title, meta, move);
    if (pending.has(id)) item.classList.add('opacity-60');
    return item;
  };

  const render = () => {
    const visible = mineOnly.checked && session ? tasks.filter(task => task.userId === session.sub) : tasks;
    const focused = (document.activeElement as HTMLElement | null)?.dataset.moveTask;
    columns.replaceChildren(...steps.map(step => {
      const inStep = visible.filter(task => task.workflowId === step.id);
      const column = element('section', 'board-column');
      column.dataset.stepId = String(step.id);
      const headingId = `step-${String(step.id)}`;
      column.setAttribute('aria-labelledby', headingId);
      const header = element('header', 'flex items-center gap-2 px-3 py-2.5');
      header.append(element('h2', 'flex-1 truncate text-sm font-semibold', String(step.name ?? '')), element('span', 'font-mono text-xs tabular-nums text-subtle', String(inStep.length)));
      header.firstElementChild!.id = headingId;
      const list = element('ul', 'board-list');
      list.append(...inStep.map(card));
      if (inStep.length === 0) list.append(element('li', 'px-2 py-6 text-center text-xs text-subtle', 'No tasks'));
      column.append(header, list);
      return column;
    }));
    if (focused) root.querySelector<HTMLSelectElement>(`[data-move-task="${focused}"]`)?.focus();
  };

  const move = async (taskId: string, stepId: string) => {
    const task = tasks.find(candidate => candidate.id === taskId);
    if (!task || task.workflowId === stepId || pending.has(taskId)) return;
    const previous = task.workflowId;
    const stepName = String(steps.find(step => step.id === stepId)?.name ?? 'the new step');
    pending.set(taskId, previous);
    task.workflowId = stepId;
    render();
    try {
      await webApiClient.update('assignments', taskId, taskUpdatePayload(task, { workflowId: stepId }));
      if (lifetime.signal.aborted) return;
      showToast(`“${String(task.name)}” moved to ${stepName}.`);
    } catch (failure) {
      if (lifetime.signal.aborted) return;
      task.workflowId = previous;
      showToast(`Could not move “${String(task.name)}”: ${failure instanceof Error ? failure.message : 'the update failed.'}`, { tone: 'danger' });
    } finally {
      pending.delete(taskId);
      if (!lifetime.signal.aborted) render();
    }
  };

  const subscribe = () => {
    stream.abort(); stream = new AbortController();
    const signal = stream.signal;
    error.hidden = true;
    if (!projectId) { columns.replaceChildren(); statusText.textContent = 'Choose a project to see its board.'; return; }
    void watchListing(onLive => webApiClient.subscribeList<ApiEntity>('assignments', 1, MAX_PAGE_SIZE, async (result, info) => {
      if (signal.aborted) return;
      if (info.live) onLive();
      // Moves still being saved keep their optimistic step until the server confirms them.
      tasks = (result.items ?? []).map(task => (pending.has(String(task.id)) ? { ...task, workflowId: tasks.find(local => local.id === task.id)?.workflowId ?? task.workflowId } : task));
      const total = result.totalCount ?? tasks.length;
      note.hidden = total <= MAX_PAGE_SIZE;
      note.textContent = `Showing the ${MAX_PAGE_SIZE} tasks due first out of ${total}. Use the Tasks list to see the rest.`;
      const missing = tasks.map(task => task.userId).filter(id => typeof id === 'string' && !owners.has(id));
      if (missing.length > 0) owners = new Map([...owners, ...await loadLabels('users', missing, signal, session)]);
      if (signal.aborted) return;
      statusText.textContent = `${tasks.length} task${tasks.length === 1 ? '' : 's'} · ${info.live ? 'Live' : 'Updated'}`;
      render();
    }, signal, { filters: { projectId }, sort: { column: 'EndDate', order: 'ASC' } }), signal, value => { statusText.textContent = value; })
      .catch(failure => { if (!signal.aborted) fail(failure instanceof Error ? failure.message : 'Unable to load the board.'); });
  };

  const selectProject = (id: string) => {
    projectId = id;
    history.replaceState(history.state, '', `${location.pathname}${id ? `?projectId=${encodeURIComponent(id)}` : ''}`);
    query<HTMLAnchorElement>('[data-board-project-link]').href = id ? `/projects/view/?id=${encodeURIComponent(id)}` : '/projects/';
    query<HTMLAnchorElement>('[data-board-new-task]').href = id ? `/assignments/?projectId=${encodeURIComponent(id)}&new=1` : '/assignments/?new=1';
    tasks = [];
    subscribe();
  };

  columns.addEventListener('change', event => {
    const select = (event.target as Element).closest<HTMLSelectElement>('[data-move-task]');
    if (select) void move(select.dataset.moveTask!, select.value);
  }, { signal: lifetime.signal });
  columns.addEventListener('dragstart', event => {
    const item = (event.target as Element).closest<HTMLElement>('[data-task-id]');
    if (!item) return;
    dragged = item.dataset.taskId!;
    event.dataTransfer?.setData('text/plain', dragged);
    item.classList.add('dragging');
  }, { signal: lifetime.signal });
  columns.addEventListener('dragend', () => { dragged = null; root.querySelectorAll('.dragging, .drop-target').forEach(node => node.classList.remove('dragging', 'drop-target')); }, { signal: lifetime.signal });
  columns.addEventListener('dragover', event => {
    const column = (event.target as Element).closest<HTMLElement>('[data-step-id]');
    if (!column || !dragged) return;
    event.preventDefault();
    root.querySelectorAll('.drop-target').forEach(node => { if (node !== column) node.classList.remove('drop-target'); });
    column.classList.add('drop-target');
  }, { signal: lifetime.signal });
  columns.addEventListener('drop', event => {
    const column = (event.target as Element).closest<HTMLElement>('[data-step-id]');
    if (!column || !dragged) return;
    event.preventDefault();
    void move(dragged, column.dataset.stepId!);
  }, { signal: lifetime.signal });
  projectSelect.addEventListener('change', () => selectProject(projectSelect.value), { signal: lifetime.signal });
  mineOnly.addEventListener('change', render, { signal: lifetime.signal });

  void (async () => {
    try {
      const [loadedSteps, projects] = await Promise.all([
        loadSteps(lifetime.signal),
        webApiClient.listAll<ApiEntity>('projects', { sort: { column: 'Name', order: 'ASC' } }, lifetime.signal, 500),
      ]);
      if (lifetime.signal.aborted) return;
      steps = loadedSteps;
      projectSelect.replaceChildren(...projects.map(project => new Option(displayEntityLabel(project), String(project.id))));
      if (projects.length === 0) {
        statusText.textContent = 'You are not a member of any project yet.';
        projectSelect.disabled = true;
        return;
      }
      if (!projects.some(project => project.id === projectId)) projectId = String(projects[0].id);
      projectSelect.value = projectId;
      if (steps.length === 0) { fail('No progress steps exist yet. An administrator can add them under Progress steps.'); return; }
      selectProject(projectId);
    } catch (failure) {
      if (!lifetime.signal.aborted) fail(failure instanceof Error ? failure.message : 'Unable to load the board.');
    }
  })();

  query<HTMLElement>('[data-board-legend]').replaceChildren(createIcon('alert', 'size-3.5'), ' Drag cards between columns, or use each card’s “Move to” menu.');
  return () => { lifetime.abort(); stream.abort(); };
};
