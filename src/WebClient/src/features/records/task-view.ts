import { getSessionClaims, type SessionClaims } from '~/lib/api/http-client';
import type { ApiEntity } from '~/lib/api/types';
import { webApiClient } from '~/lib/api/webapi-client';
import { daysUntil, formatUtcDay, formatUtcTime, isPastDay, sumHours } from '~/lib/dates';
import { badge, element, fillList, isDoneStep, label, link, loadLabels, loadSteps } from './related';

const messageOf = (failure: unknown, fallback: string) => (failure instanceof Error ? failure.message : fallback);

/** Task overview: schedule, time logged against the estimate, blockers and the people on it. */
export const mountTaskView = async (root: HTMLElement, id: string | null, signal: AbortSignal, session: SessionClaims | null = getSessionClaims()) => {
  const query = <T extends Element>(selector: string) => root.querySelector<T>(selector)!;
  const error = query<HTMLElement>('[data-view-error]');
  const show = (message: string) => { error.textContent = message; error.hidden = false; query<HTMLElement>('[data-view-body]').hidden = true; };
  if (!id) { show('No task was selected. Open a task from the Tasks list.'); return; }

  let task: ApiEntity;
  try {
    task = await webApiClient.get<ApiEntity>('assignments', id, signal);
  } catch (failure) {
    if (signal.aborted) return;
    show(failure instanceof Error && 'status' in failure && failure.status === 404
      ? 'This task was not found. It may have been deleted, or you are not a member of its project.'
      : messageOf(failure, 'Unable to load the task.'));
    return;
  }
  if (signal.aborted) return;
  const name = String(task.name ?? 'Task');
  document.title = `${name} · Cpnucleo`;
  query<HTMLElement>('[data-title]').textContent = name;
  document.querySelector('[data-breadcrumb-current]')?.replaceChildren(name);
  query<HTMLElement>('[data-description]').textContent = String(task.description ?? '');
  const encoded = encodeURIComponent(id);
  for (const anchor of root.querySelectorAll<HTMLAnchorElement>('[data-href]')) anchor.href = anchor.dataset.href!.replaceAll('{id}', encoded).replaceAll('{projectId}', encodeURIComponent(String(task.projectId)));

  let blockers: ApiEntity[]; let people: ApiEntity[]; let entries: ApiEntity[]; let steps: ApiEntity[];
  try {
    [blockers, people, entries, steps] = await Promise.all([
      webApiClient.listAll<ApiEntity>('assignmentImpediments', { filters: { assignmentId: id } }, signal),
      webApiClient.listAll<ApiEntity>('userAssignments', { filters: { assignmentId: id } }, signal),
      webApiClient.listAll<ApiEntity>('appointments', { filters: { assignmentId: id }, sort: { column: 'KeepDate', order: 'DESC' } }, signal),
      loadSteps(signal),
    ]);
  } catch (failure) {
    if (!signal.aborted) show(messageOf(failure, 'Unable to load the task.'));
    return;
  }
  const [projects, types, impediments, users] = await Promise.all([
    loadLabels('projects', [task.projectId], signal, session),
    loadLabels('assignmentTypes', [task.assignmentTypeId], signal, session),
    loadLabels('impediments', blockers.map(blocker => blocker.impedimentId), signal, session),
    loadLabels('users', [task.userId, ...people.map(person => person.userId), ...entries.map(entry => entry.userId)], signal, session),
  ]);
  if (signal.aborted) return;

  const step = steps.find(candidate => candidate.id === task.workflowId);
  const done = isDoneStep(steps, task.workflowId);
  const facts = query<HTMLElement>('[data-facts]');
  const fact = (term: string, ...value: (Node | string)[]) => {
    const row = element('div', 'grid gap-1 py-3 text-sm sm:grid-cols-3 sm:gap-4');
    const dd = element('dd', 'flex flex-wrap items-center gap-2 sm:col-span-2');
    dd.append(...value);
    row.append(element('dt', 'text-subtle', term), dd);
    return row;
  };
  const due = daysUntil(task.endDate);
  // The progress step already says "Done"; finished tasks need no due-date note.
  const dueNote = done ? ''
    : isPastDay(task.endDate) ? badge(`Overdue by ${-due!} day${due === -1 ? '' : 's'}`, 'danger')
    : due !== undefined && due <= 7 ? badge(due === 0 ? 'Due today' : `Due in ${due} day${due === 1 ? '' : 's'}`, 'warning') : '';
  facts.replaceChildren(
    fact('Project', link(`/projects/view/?id=${encodeURIComponent(String(task.projectId))}`, label(projects, task.projectId))),
    fact('Progress step', badge(String(step?.name ?? 'Unknown step'), done ? 'success' : 'neutral')),
    fact('Owner', label(users, task.userId)),
    fact('Task type', label(types, task.assignmentTypeId)),
    fact('Dates', `${formatUtcDay(task.startDate)} – ${formatUtcDay(task.endDate)}`, dueNote),
  );

  const logged = sumHours(entries);
  const estimate = Number(task.amountHours) || 0;
  query<HTMLElement>('[data-hours-logged]').textContent = String(logged);
  query<HTMLElement>('[data-hours-estimate]').textContent = String(estimate);
  const meter = query<HTMLElement>('[data-hours-meter]');
  const ratio = estimate > 0 ? logged / estimate : 0;
  meter.style.width = `${Math.min(ratio, 1) * 100}%`;
  meter.classList.toggle('bg-danger', ratio > 1);
  query<HTMLElement>('[data-hours-bar]').setAttribute('aria-valuenow', String(Math.round(ratio * 100)));
  const hours = (value: number) => `${value} hour${value === 1 ? '' : 's'}`;
  query<HTMLElement>('[data-hours-note]').textContent = estimate === 0 ? 'No estimate.'
    : ratio > 1 ? `${hours(logged - estimate)} over the estimate.` : `${hours(estimate - logged)} left in the estimate.`;

  fillList(query('[data-blockers]'), blockers.map(blocker => {
    const row = element('li', 'px-5 py-3 text-sm');
    row.append(element('p', 'font-medium', label(impediments, blocker.impedimentId)), element('p', 'mt-0.5 text-muted', String(blocker.description ?? '')));
    return row;
  }), 'Nothing is blocking this task.');
  fillList(query('[data-people]'), people.map(person => element('li', 'px-5 py-3 text-sm', label(users, person.userId))), 'Nobody else is on this task yet.');
  fillList(query('[data-entries]'), entries.map(entry => {
    const row = element('li', 'flex items-start gap-4 px-5 py-3 text-sm');
    const when = element('div', 'w-28 flex-none text-xs tabular-nums text-subtle');
    when.append(element('p', '', formatUtcDay(entry.keepDate)), element('p', '', `${formatUtcTime(entry.keepDate)} UTC`));
    const what = element('div', 'min-w-0 flex-1');
    what.append(element('p', '', String(entry.description ?? '')), element('p', 'mt-0.5 text-xs text-subtle', label(users, entry.userId)));
    row.append(when, what, element('span', 'flex-none font-mono tabular-nums text-muted', `${Number(entry.amountHours) || 0} h`));
    return row;
  }), 'No time logged yet.');
  query<HTMLElement>('[data-view-body]').setAttribute('aria-busy', 'false');
};
