import { getSessionClaims, type SessionClaims } from '~/lib/api/http-client';
import type { ApiEntity } from '~/lib/api/types';
import { webApiClient } from '~/lib/api/webapi-client';
import { formatUtcDay, isPastDay, sumHours } from '~/lib/dates';
import { badge, element, fillList, isDoneStep, label, link, loadLabels, loadSteps, type Labels } from './related';

/** Project overview: its tasks by progress, its members and shortcuts into the filtered lists. */
export const mountProjectView = async (root: HTMLElement, id: string | null, signal: AbortSignal, session: SessionClaims | null = getSessionClaims()) => {
  const query = <T extends Element>(selector: string) => root.querySelector<T>(selector)!;
  const error = query<HTMLElement>('[data-view-error]');
  const show = (message: string) => { error.textContent = message; error.hidden = false; query<HTMLElement>('[data-view-body]').hidden = true; };
  if (!id) { show('No project was selected. Open a project from the Projects list.'); return; }

  let project: ApiEntity;
  try {
    project = await webApiClient.get<ApiEntity>('projects', id, signal);
  } catch (failure) {
    if (signal.aborted) return;
    show(failure instanceof Error && 'status' in failure && failure.status === 404
      ? 'This project was not found. It may have been deleted, or you are not one of its members.'
      : failure instanceof Error ? failure.message : 'Unable to load the project.');
    return;
  }
  if (signal.aborted) return;
  const name = String(project.name ?? 'Project');
  document.title = `${name} · Cpnucleo`;
  query<HTMLElement>('[data-title]').textContent = name;
  document.querySelector('[data-breadcrumb-current]')?.replaceChildren(name);
  const encoded = encodeURIComponent(id);
  for (const anchor of root.querySelectorAll<HTMLAnchorElement>('[data-href]')) anchor.href = anchor.dataset.href!.replaceAll('{id}', encoded);

  let organizations: Labels; let tasks: ApiEntity[]; let members: ApiEntity[]; let steps: ApiEntity[];
  try {
    [organizations, tasks, members, steps] = await Promise.all([
      loadLabels('organizations', [project.organizationId], signal, session),
      webApiClient.listAll<ApiEntity>('assignments', { filters: { projectId: id }, sort: { column: 'EndDate', order: 'ASC' } }, signal),
      webApiClient.listAll<ApiEntity>('userProjects', { filters: { projectId: id } }, signal),
      loadSteps(signal),
    ]);
  } catch (failure) {
    if (!signal.aborted) show(failure instanceof Error ? failure.message : 'Unable to load the project.');
    return;
  }
  if (signal.aborted) return;

  const organization = query<HTMLElement>('[data-organization]');
  organization.replaceChildren('Organization: ', link(`/projects/?organizationId=${encodeURIComponent(String(project.organizationId))}`, label(organizations, project.organizationId)));
  organization.title = 'Projects of this organization';

  const [owners, people] = await Promise.all([
    loadLabels('users', tasks.map(task => task.userId), signal, session),
    loadLabels('users', members.map(member => member.userId), signal, session),
  ]);
  if (signal.aborted) return;
  const stepNames = new Map(steps.map(step => [String(step.id), String(step.name ?? '')]));
  const open = tasks.filter(task => !isDoneStep(steps, task.workflowId));
  const overdue = open.filter(task => isPastDay(task.endDate));

  query<HTMLElement>('[data-stat="tasks"]').textContent = String(tasks.length);
  query<HTMLElement>('[data-stat="open"]').textContent = String(open.length);
  query<HTMLElement>('[data-stat="overdue"]').textContent = String(overdue.length);
  query<HTMLElement>('[data-stat="members"]').textContent = String(members.length);
  query<HTMLElement>('[data-stat="hours"]').textContent = String(sumHours(tasks));

  fillList(query('[data-tasks]'), tasks.map(task => {
    const row = element('li', 'flex flex-wrap items-center gap-x-4 gap-y-1 px-5 py-3');
    const title = element('div', 'min-w-0 flex-1');
    title.append(link(`/assignments/view/?id=${encodeURIComponent(String(task.id))}`, String(task.name ?? 'Task'), 'table-link font-medium'));
    title.append(element('p', 'mt-0.5 truncate text-xs text-subtle', label(owners, task.userId)));
    const meta = element('div', 'flex flex-wrap items-center gap-2 text-xs');
    meta.append(badge(stepNames.get(String(task.workflowId)) ?? 'Unknown step', isDoneStep(steps, task.workflowId) ? 'success' : 'neutral'));
    if (!isDoneStep(steps, task.workflowId) && isPastDay(task.endDate)) meta.append(badge('Overdue', 'danger'));
    meta.append(element('span', 'tabular-nums text-muted', `Due ${formatUtcDay(task.endDate)}`));
    row.append(title, meta);
    return row;
  }), 'This project has no tasks yet.');

  fillList(query('[data-members]'), members.map(member => {
    const row = element('li', 'flex items-center gap-3 px-5 py-3 text-sm');
    const person = label(people, member.userId);
    row.append(element('span', 'flex size-7 flex-none items-center justify-center rounded-full bg-raised text-xs font-semibold uppercase text-muted ring-1 ring-line', person.charAt(0) || '?'), element('span', 'truncate', person));
    return row;
  }), 'No members yet.');
  query<HTMLElement>('[data-view-body]').setAttribute('aria-busy', 'false');
};
