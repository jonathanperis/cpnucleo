import { accountClient } from '~/lib/api/account-client';
import { getSessionClaims, type SessionClaims } from '~/lib/api/http-client';
import type { ApiEntity } from '~/lib/api/types';
import { webApiClient } from '~/lib/api/webapi-client';
import { addUtcDays, daysUntil, formatUtcDay, isPastDay, startOfUtcWeek, sumHours } from '~/lib/dates';
import { badge, element, fillList, isDoneStep, label, link, loadLabels, loadSteps } from '~/features/records/related';

export const MY_TASKS_SHOWN = 6;

/** Open tasks first by due day; overdue and due-soon counts for the summary. */
export const summarizeTasks = (tasks: ApiEntity[], steps: ApiEntity[], now = new Date()) => {
  const open = tasks.filter(task => !isDoneStep(steps, task.workflowId))
    .sort((a, b) => String(a.endDate).localeCompare(String(b.endDate)));
  const overdue = open.filter(task => isPastDay(task.endDate, now)).length;
  const dueSoon = open.filter(task => { const days = daysUntil(task.endDate, now); return days !== undefined && days >= 0 && days <= 7; }).length;
  return { open, overdue, dueSoon };
};

/** "My work" on Home: the signed-in person's open tasks, hours this week and projects. */
export const loadMyWork = async (root: HTMLElement, signal: AbortSignal, session: SessionClaims | null = getSessionClaims()) => {
  if (!session) return;
  const query = <T extends Element>(selector: string) => root.querySelector<T>(selector)!;
  const week = startOfUtcWeek(new Date());
  const greeting = query<HTMLElement>('[data-greeting]');
  greeting.textContent = `Welcome back, ${session.login}`;
  void accountClient.getProfile(signal).then(profile => {
    if (!signal.aborted && profile.name?.trim()) greeting.textContent = `Welcome back, ${profile.name.trim()}`;
  }).catch(() => undefined);

  const failSection = (selector: string, error: unknown) => {
    if (signal.aborted) return;
    query<HTMLElement>(selector).replaceChildren(element('li', 'px-5 py-6 text-center text-sm text-danger', error instanceof Error ? error.message : 'Unavailable.'));
  };

  await Promise.all([
    (async () => {
      try {
        const [tasks, steps] = await Promise.all([
          webApiClient.listAll<ApiEntity>('assignments', { filters: { userId: session.sub }, sort: { column: 'EndDate', order: 'ASC' } }, signal, 300),
          loadSteps(signal),
        ]);
        const projects = await loadLabels('projects', tasks.map(task => task.projectId), signal, session);
        if (signal.aborted) return;
        const { open, overdue, dueSoon } = summarizeTasks(tasks, steps);
        query<HTMLElement>('[data-my-stat="open"]').textContent = String(open.length);
        query<HTMLElement>('[data-my-stat="overdue"]').textContent = String(overdue);
        query<HTMLElement>('[data-my-stat="soon"]').textContent = String(dueSoon);
        fillList(query('[data-my-tasks]'), open.slice(0, MY_TASKS_SHOWN).map(task => {
          const row = element('li', 'flex flex-wrap items-center gap-x-3 gap-y-1 px-5 py-3');
          const text = element('div', 'min-w-0 flex-1');
          text.append(link(`/assignments/view/?id=${encodeURIComponent(String(task.id))}`, String(task.name ?? 'Task'), 'table-link text-sm font-medium'),
            element('p', 'mt-0.5 truncate text-xs text-subtle', label(projects, task.projectId)));
          const due = daysUntil(task.endDate);
          const meta = element('div', 'flex items-center gap-2 text-xs');
          if (isPastDay(task.endDate)) meta.append(badge('Overdue', 'danger'));
          else if (due !== undefined && due <= 7) meta.append(badge(due === 0 ? 'Due today' : `Due in ${due}d`, 'warning'));
          meta.append(element('span', 'tabular-nums text-muted', formatUtcDay(task.endDate)));
          row.append(text, meta);
          return row;
        }), 'No open tasks assigned to you.');
        query<HTMLAnchorElement>('[data-my-tasks-all]').href = `/assignments/?userId=${encodeURIComponent(session.sub)}&sort=endDate&order=asc`;
      } catch (error) { failSection('[data-my-tasks]', error); }
    })(),
    (async () => {
      try {
        const entries = await webApiClient.listAll<ApiEntity>('appointments', {
          filters: { userId: session.sub }, dateFrom: week.toISOString(), dateTo: addUtcDays(week, 7).toISOString(),
        }, signal, 500);
        if (signal.aborted) return;
        const hours = sumHours(entries);
        query<HTMLElement>('[data-my-hours]').textContent = String(hours);
        query<HTMLElement>('[data-my-hours-note]').textContent = `${entries.length} calendar item${entries.length === 1 ? '' : 's'} since Monday (UTC).`;
      } catch (error) {
        if (signal.aborted) return;
        query<HTMLElement>('[data-my-hours-note]').textContent = error instanceof Error ? error.message : 'Unavailable.';
      }
    })(),
    (async () => {
      try {
        const memberships = await webApiClient.listAll<ApiEntity>('userProjects', { filters: { userId: session.sub } }, signal, 100);
        const projects = await loadLabels('projects', memberships.map(membership => membership.projectId), signal, session);
        if (signal.aborted) return;
        fillList(query('[data-my-projects]'), memberships.map(membership => {
          const row = element('li');
          row.append(link(`/projects/view/?id=${encodeURIComponent(String(membership.projectId))}`, label(projects, membership.projectId), 'flex items-center gap-2 px-5 py-2.5 text-sm hover:bg-raised/45'));
          return row;
        }), 'You are not on any project yet.');
      } catch (error) { failSection('[data-my-projects]', error); }
    })(),
  ]);
  if (!signal.aborted) query<HTMLElement>('[data-my-work]').setAttribute('aria-busy', 'false');
};
