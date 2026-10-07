// @vitest-environment jsdom
import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { ApiError, type SessionClaims } from '~/lib/api/http-client';
import { webApiClient } from '~/lib/api/webapi-client';
import type { ApiEntity, PaginatedResult } from '~/lib/api/types';
import { mountCrudPage } from './crud-controller';

const admin: SessionClaims = { sub: 'admin-1', login: 'admin@cpnucleo.test', isAdmin: true };
const member: SessionClaims = { sub: 'user-7', login: 'learner@cpnucleo.test', isAdmin: false };

let stop = () => {};
const pageMarkup = (route: string) => new DOMParser().parseFromString(
  readFileSync(resolve('dist', route, 'index.html'), 'utf8'), 'text/html').body.innerHTML;

let rows: ApiEntity[] = [];
const mount = (route: string, session: SessionClaims | null = admin) => {
  document.body.innerHTML = pageMarkup(route);
  // AuthGuard reveals the workspace once a session exists; mirror that for the generated markup.
  document.querySelector<HTMLElement>('[data-auth-content]')!.hidden = false;
  const root = document.querySelector<HTMLElement>('[data-crud]')!;
  stop = mountCrudPage(root, session);
  return root;
};
const field = <T extends Element>(root: HTMLElement, name: string) => root.querySelector<HTMLFormElement>('[data-form]')!.elements.namedItem(name) as unknown as T;
const submit = (root: HTMLElement) => root.querySelector<HTMLFormElement>('[data-form]')!.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
const relation = (root: HTMLElement, key: string) => root.querySelector<HTMLElement>(`[data-relation="${key}"]`)!;
const deferred = <T,>() => {
  let resolve!: (value: T) => void;
  const promise = new Promise<T>(done => { resolve = done; });
  return { promise, resolve };
};

// jsdom implements neither showModal() nor close(); mirror the browser behavior the controller relies on.
const polyfillDialogs = () => {
  HTMLDialogElement.prototype.showModal ??= function (this: HTMLDialogElement) { this.open = true; };
  HTMLDialogElement.prototype.close ??= function (this: HTMLDialogElement, value?: string) {
    if (value !== undefined) this.returnValue = value;
    this.open = false;
    this.dispatchEvent(new Event('close'));
  };
};
/** Clicks a row's Delete button and answers the confirmation dialog. */
const deleteRow = async (root: HTMLElement, button: HTMLButtonElement, answer: 'accept' | 'cancel' = 'accept') => {
  button.click();
  await vi.waitFor(() => expect(root.querySelector<HTMLDialogElement>('[data-confirm]')!.open).toBe(true));
  root.querySelector<HTMLButtonElement>(`[data-confirm-${answer}]`)!.click();
};

beforeEach(() => {
  polyfillDialogs();
  rows = [{ id: 'one', createdAt: '2026-09-16T10:00:00Z', name: '<img src=x onerror=alert(1)>', login: 'learner', organizationId: 'org' }];
  vi.spyOn(webApiClient, 'list').mockResolvedValue({ items: [{ id: 'org', name: 'School' }], totalCount: 1 });
  vi.spyOn(webApiClient, 'lookup').mockResolvedValue([{ id: 'org', name: 'School' }]);
  vi.spyOn(webApiClient, 'subscribeList').mockImplementation(async (_key, _page, _size, onPage, signal) => {
    onPage({ items: rows, totalCount: 55 }, { live: true });
    await new Promise<void>(done => signal?.addEventListener('abort', () => done(), { once: true }));
  });
});
afterEach(() => { stop(); vi.restoreAllMocks(); document.body.replaceChildren(); });

describe('CRUD page basics', () => {
  it('uses generated Astro controls to prefill and save, without rendering API data as HTML', async () => {
    const root = mount('projects');
    const update = vi.spyOn(webApiClient, 'update').mockResolvedValue({ success: true });
    expect(vi.mocked(webApiClient.subscribeList).mock.calls[0][2]).toBe(10);
    expect(root.querySelector<HTMLSelectElement>('[data-page-size]')!.value).toBe('10');
    await vi.waitFor(() => expect(root.querySelector('[data-records]')?.textContent).toContain('School'));
    expect(root.querySelector('[data-summary]')?.textContent).toContain('Live');
    expect(root.querySelector('[data-records] img')).toBeNull();
    root.querySelector<HTMLButtonElement>('[data-action="edit"]')!.click();
    const name = field<HTMLInputElement>(root, 'name');
    expect(name.value).toContain('<img');
    name.value = 'Updated project';
    await vi.waitFor(() => expect(field<HTMLSelectElement>(root, 'organizationId').value).toBe('org'));
    submit(root);
    await vi.waitFor(() => expect(update).toHaveBeenCalledWith('projects', 'one', { name: 'Updated project', organizationId: 'org', expectedVersion: '2026-09-16T10:00:00Z' }));
    await vi.waitFor(() => expect(root.querySelector<HTMLFormElement>('[data-form]')!.hidden).toBe(true));
  });

  it('keeps the password blank and requires it for creation, but not ordinary user edits', () => {
    const root = mount('users');
    root.querySelector<HTMLButtonElement>('[data-action="create"]')!.click();
    const password = root.querySelector<HTMLInputElement>('[name="password"]')!;
    expect(password.required).toBe(true); expect(password.value).toBe('');
    root.querySelector<HTMLButtonElement>('[data-action="edit"]')!.click();
    expect(password.required).toBe(false); expect(password.value).toBe('');
  });

  it('ignores responses from an aborted page and updates the accessible current-page control', () => {
    const root = mount('projects');
    const previous = vi.mocked(webApiClient.subscribeList).mock.calls[0][3];
    root.querySelector<HTMLButtonElement>('[data-action="page"][data-page="2"]')!.click();
    previous({ items: [{ id: 'stale', name: 'Stale page' }], totalCount: 1 }, { live: true });
    expect(root.querySelector('[data-records]')?.textContent).not.toContain('Stale page');
    expect(root.querySelector('[aria-current="page"]')?.getAttribute('aria-label')).toBe('Page 2');
    expect(vi.mocked(webApiClient.subscribeList).mock.calls[0][4]?.aborted).toBe(true);
  });

  it('labels a JSON (non-stream) snapshot as updated rather than live', async () => {
    vi.mocked(webApiClient.subscribeList).mockImplementation(async (_key, _page, _size, onPage, signal) => {
      onPage({ items: rows, totalCount: 1 }, { live: false });
      await new Promise<void>(done => signal?.addEventListener('abort', () => done(), { once: true }));
    });
    const root = mount('projects');
    await vi.waitFor(() => expect(root.querySelector('[data-summary]')?.textContent).toContain('Updated'));
  });

  it('requires a workflow order of at least 1', () => {
    const root = mount('workflows');
    root.querySelector<HTMLButtonElement>('[data-action="create"]')!.click();
    const order = field<HTMLInputElement>(root, 'order');
    expect(order.required).toBe(true);
    expect(order.min).toBe('1');
    order.value = '0';
    expect(order.checkValidity()).toBe(false);
  });
});

describe('server errors in forms', () => {
  it('marks fields from the error envelope and clears them on the next submit', async () => {
    const root = mount('projects');
    vi.spyOn(webApiClient, 'update')
      .mockRejectedValueOnce(new ApiError(400, 'The project is archived. Name is too long.', undefined, { fieldErrors: { name: ['Name is too long.'] }, generalErrors: ['The project is archived.'] }))
      .mockResolvedValueOnce({});
    root.querySelector<HTMLButtonElement>('[data-action="edit"]')!.click();
    await vi.waitFor(() => expect(field<HTMLSelectElement>(root, 'organizationId').value).toBe('org'));
    submit(root);

    const name = field<HTMLInputElement>(root, 'name');
    const message = root.querySelector<HTMLElement>('#projects-name-error')!;
    await vi.waitFor(() => expect(name.getAttribute('aria-invalid')).toBe('true'));
    expect(name.getAttribute('aria-describedby')?.split(' ')).toContain('projects-name-error');
    expect(message.hidden).toBe(false);
    expect(message.textContent).toBe('Name is too long.');
    expect(document.activeElement).toBe(name);
    expect(root.querySelector('[data-error]')?.textContent).toBe('The project is archived. Name is too long.');
    expect(root.querySelector('[data-error]')?.getAttribute('role')).toBe('alert');

    name.value = 'Short';
    submit(root);
    expect(name.hasAttribute('aria-invalid')).toBe(false);
    expect(name.getAttribute('aria-describedby') ?? '').not.toContain('projects-name-error');
    expect(message.hidden).toBe(true);
  });

  it.each([
    [409, 'The project changed. Reload before saving your changes.'],
    [403, 'You do not have permission to change this project.'],
    [429, 'Too many requests. Try again in 30 seconds.'],
  ])('shows the %i message and keeps the form open without signing out', async (status, text) => {
    const root = mount('projects');
    vi.spyOn(webApiClient, 'update').mockRejectedValue(new ApiError(status, text));
    root.querySelector<HTMLButtonElement>('[data-action="edit"]')!.click();
    await vi.waitFor(() => expect(field<HTMLSelectElement>(root, 'organizationId').value).toBe('org'));
    submit(root);
    await vi.waitFor(() => expect(root.querySelector('[data-error]')?.textContent).toBe(text));
    expect(root.querySelector<HTMLElement>('[data-error]')!.hidden).toBe(false);
    expect(root.querySelector<HTMLFormElement>('[data-form]')!.hidden).toBe(false);
  });

  it('asks for confirmation in a dialog and keeps the record when the user cancels', async () => {
    const root = mount('projects');
    const remove = vi.spyOn(webApiClient, 'delete').mockResolvedValue(undefined);
    await vi.waitFor(() => expect(root.querySelector('[data-action="delete"]')).not.toBeNull());
    const button = root.querySelector<HTMLButtonElement>('[data-action="delete"]')!;
    await deleteRow(root, button, 'cancel');
    const message = root.querySelector('[data-confirm-message]')!;
    expect(message.textContent).toContain('“<img src=x onerror=alert(1)>”');
    expect(message.querySelector('img')).toBeNull();
    await vi.waitFor(() => expect(document.activeElement).toBe(button));
    expect(root.querySelector<HTMLDialogElement>('[data-confirm]')!.open).toBe(false);
    expect(remove).not.toHaveBeenCalled();
  });

  it('returns focus to the replacement Delete button when a live snapshot re-renders the row during confirmation', async () => {
    let push: (result: PaginatedResult<ApiEntity>) => void = () => {};
    vi.mocked(webApiClient.subscribeList).mockImplementation(async (_key, _page, _size, onPage, signal) => {
      push = result => onPage(result, { live: true });
      push({ items: rows, totalCount: 1 });
      await new Promise<void>(done => signal?.addEventListener('abort', () => done(), { once: true }));
    });
    const root = mount('projects');
    await vi.waitFor(() => expect(root.querySelector('[data-action="delete"]')).not.toBeNull());
    const original = root.querySelector<HTMLButtonElement>('[data-action="delete"]')!;
    original.click();
    await vi.waitFor(() => expect(root.querySelector<HTMLDialogElement>('[data-confirm]')!.open).toBe(true));
    push({ items: [{ ...rows[0], name: 'Renamed live' }], totalCount: 1 });
    await vi.waitFor(() => expect(root.querySelector('[data-records]')?.textContent).toContain('Renamed live'));
    root.querySelector<HTMLButtonElement>('[data-confirm-cancel]')!.click();
    const replacement = root.querySelector<HTMLButtonElement>('[data-action="delete"][data-id="one"]')!;
    expect(replacement).not.toBe(original);
    await vi.waitFor(() => expect(document.activeElement).toBe(replacement));
  });

  it('shows why a removal was rejected', async () => {
    const root = mount('projects');
    const remove = vi.spyOn(webApiClient, 'delete').mockRejectedValue(new ApiError(409, 'The project still has active tasks.'));
    await vi.waitFor(() => expect(root.querySelector('[data-action="delete"]')).not.toBeNull());
    await deleteRow(root, root.querySelector<HTMLButtonElement>('[data-action="delete"]')!);
    await vi.waitFor(() => expect(root.querySelector('[data-error]')?.textContent).toBe('The project still has active tasks.'));
    expect(remove).toHaveBeenCalledWith('projects', 'one');
  });

  it('rejects a task whose end date is before its start date before calling the API', async () => {
    vi.mocked(webApiClient.list).mockImplementation(async key => ({ items: [{ id: `${key}-1`, name: `${key} option` }], totalCount: 1 }));
    const root = mount('assignments');
    const create = vi.spyOn(webApiClient, 'create').mockResolvedValue({});
    root.querySelector<HTMLButtonElement>('[data-action="create"]')!.click();
    await vi.waitFor(() => expect(field<HTMLSelectElement>(root, 'projectId').options.length).toBe(2));
    field<HTMLInputElement>(root, 'name').value = 'Plan';
    field<HTMLTextAreaElement>(root, 'description').value = 'Write the plan';
    field<HTMLInputElement>(root, 'startDate').value = '2026-10-10';
    field<HTMLInputElement>(root, 'endDate').value = '2026-10-01';
    field<HTMLInputElement>(root, 'amountHours').value = '4';
    for (const [name, key] of [['projectId', 'projects'], ['workflowId', 'workflows'], ['userId', 'users'], ['assignmentTypeId', 'assignmentTypes']]) {
      field<HTMLSelectElement>(root, name).value = `${key}-1`;
    }
    submit(root);
    const endDate = field<HTMLInputElement>(root, 'endDate');
    expect(endDate.getAttribute('aria-invalid')).toBe('true');
    expect(root.querySelector('#assignments-endDate-error')?.textContent).toBe('End date must be on or after the start date.');
    expect(create).not.toHaveBeenCalled();

    endDate.value = '2026-10-12';
    submit(root);
    await vi.waitFor(() => expect(create).toHaveBeenCalledWith('assignments', expect.objectContaining({ startDate: '2026-10-10T00:00:00.000Z', endDate: '2026-10-12T00:00:00.000Z' })));
  });
});

describe('relation pickers', () => {
  it('searches and paginates relation options beyond the first page', async () => {
    vi.mocked(webApiClient.list).mockResolvedValue({ items: [{ id: 'org', name: 'School' }], totalCount: 150 });
    const root = mount('projects');
    root.querySelector<HTMLButtonElement>('[data-action="create"]')!.click();
    const container = relation(root, 'organizations');
    await vi.waitFor(() => expect(container.querySelector<HTMLButtonElement>('[data-action="more"]')!.hidden).toBe(false));
    container.querySelector<HTMLButtonElement>('[data-action="more"]')!.click();
    await vi.waitFor(() => expect(webApiClient.list).toHaveBeenCalledWith('organizations', 2, 100, expect.any(AbortSignal), ''));
    container.querySelector<HTMLInputElement>('[data-query]')!.value = 'School';
    container.querySelector<HTMLButtonElement>('[data-action="search"]')!.click();
    await vi.waitFor(() => expect(webApiClient.list).toHaveBeenCalledWith('organizations', 1, 100, expect.any(AbortSignal), 'School'));
  });

  it('keeps the human label of a selected relation that is not in the search results', async () => {
    rows = [{ id: 'one', createdAt: '2026-09-16T10:00:00Z', name: 'Atlas', organizationId: 'org-far' }];
    vi.mocked(webApiClient.lookup).mockResolvedValue([{ id: 'org-far', name: 'Faraway Org' }]);
    vi.mocked(webApiClient.list).mockResolvedValue({ items: [{ id: 'org-near', name: 'Nearby Org' }], totalCount: 1 });
    const root = mount('projects');
    await vi.waitFor(() => expect(root.querySelector('[data-records]')?.textContent).toContain('Faraway Org'));
    root.querySelector<HTMLButtonElement>('[data-action="edit"]')!.click();
    const select = field<HTMLSelectElement>(root, 'organizationId');
    await vi.waitFor(() => expect(relation(root, 'organizations').querySelector('[data-relation-status]')?.textContent).toContain('options loaded'));
    expect(select.value).toBe('org-far');
    expect(select.selectedOptions[0].textContent).toBe('Faraway Org');
    expect([...select.options].map(option => option.textContent)).toContain('Nearby Org');
  });

  it('honors an explicit clear instead of resurrecting the original value after a search', async () => {
    const root = mount('projects');
    root.querySelector<HTMLButtonElement>('[data-action="edit"]')!.click();
    const select = field<HTMLSelectElement>(root, 'organizationId');
    await vi.waitFor(() => expect(select.value).toBe('org'));
    select.value = '';
    select.dispatchEvent(new Event('change', { bubbles: true }));
    const container = relation(root, 'organizations');
    container.querySelector<HTMLInputElement>('[data-query]')!.value = 'Sch';
    container.querySelector<HTMLButtonElement>('[data-action="search"]')!.click();
    await vi.waitFor(() => expect(webApiClient.list).toHaveBeenCalledWith('organizations', 1, 100, expect.any(AbortSignal), 'Sch'));
    await vi.waitFor(() => expect(container.querySelector('[data-relation-status]')?.textContent).toContain('for “Sch”'));
    expect(select.value).toBe('');
  });

  it('aborts the previous search for the same relation and ignores its late response', async () => {
    const first = deferred<PaginatedResult<ApiEntity>>();
    const second = deferred<PaginatedResult<ApiEntity>>();
    const root = mount('projects');
    vi.mocked(webApiClient.list).mockReset()
      .mockImplementationOnce(() => first.promise)
      .mockImplementationOnce(() => second.promise);
    root.querySelector<HTMLButtonElement>('[data-action="create"]')!.click();
    const container = relation(root, 'organizations');
    container.querySelector<HTMLInputElement>('[data-query]')!.value = 'Beta';
    container.querySelector<HTMLButtonElement>('[data-action="search"]')!.click();

    const firstSignal = vi.mocked(webApiClient.list).mock.calls[0][3]!;
    const secondSignal = vi.mocked(webApiClient.list).mock.calls[1][3]!;
    expect(firstSignal.aborted).toBe(true);
    expect(secondSignal.aborted).toBe(false);
    expect(container.querySelector('[data-relation-status]')?.getAttribute('role')).toBe('status');
    expect(container.querySelector('[data-relation-status]')?.textContent).toBe('Loading options…');

    second.resolve({ items: [{ id: 'beta', name: 'Beta Org' }], totalCount: 1 });
    first.resolve({ items: [{ id: 'alpha', name: 'Alpha Org' }], totalCount: 1 });
    const select = field<HTMLSelectElement>(root, 'organizationId');
    await vi.waitFor(() => expect([...select.options].map(option => option.textContent)).toContain('Beta Org'));
    await Promise.resolve();
    expect([...select.options].map(option => option.textContent)).not.toContain('Alpha Org');
    expect(container.querySelector('[data-relation-error]')?.textContent).toBe('');
  });

  it('continues the query its page counter belongs to when loading more', async () => {
    const root = mount('projects');
    vi.mocked(webApiClient.list).mockImplementation(async (_key, pageNumber) => ({ items: [{ id: `org-${pageNumber}`, name: `School ${pageNumber}` }], totalCount: 250 }));
    root.querySelector<HTMLButtonElement>('[data-action="create"]')!.click();
    const container = relation(root, 'organizations');
    const queryInput = container.querySelector<HTMLInputElement>('[data-query]')!;
    queryInput.value = 'Alpha';
    container.querySelector<HTMLButtonElement>('[data-action="search"]')!.click();
    await vi.waitFor(() => expect(container.querySelector('[data-relation-status]')?.textContent).toContain('for “Alpha”'));
    queryInput.value = 'Beta'; // typed, but not searched yet
    container.querySelector<HTMLButtonElement>('[data-action="more"]')!.click();
    await vi.waitFor(() => expect(webApiClient.list).toHaveBeenLastCalledWith('organizations', 2, 100, expect.any(AbortSignal), 'Alpha'));
    await vi.waitFor(() => expect(container.querySelector('[data-relation-status]')?.textContent).toContain('2 of 250'));
    container.querySelector<HTMLButtonElement>('[data-action="more"]')!.click();
    await vi.waitFor(() => expect(webApiClient.list).toHaveBeenLastCalledWith('organizations', 3, 100, expect.any(AbortSignal), 'Alpha'));
    container.querySelector<HTMLButtonElement>('[data-action="search"]')!.click();
    await vi.waitFor(() => expect(webApiClient.list).toHaveBeenLastCalledWith('organizations', 1, 100, expect.any(AbortSignal), 'Beta'));
  });
});

describe('authorization in the UI', () => {
  it('hides reference-data writes for non-admins and explains why', async () => {
    const root = mount('organizations', member);
    await vi.waitFor(() => expect(root.querySelector('[data-records] tr')).not.toBeNull());
    expect(root.querySelector<HTMLButtonElement>('[data-action="create"]')!.hidden).toBe(true);
    const note = root.querySelector<HTMLElement>('[data-permission-note]')!;
    expect(note.hidden).toBe(false);
    expect(note.getAttribute('role')).toBe('note');
    expect(note.textContent).toContain('Only administrators can create, edit or delete organizations');
    expect(root.querySelector('[data-records] [data-action="edit"]')).toBeNull();
    expect(root.querySelector('[data-records] [data-action="delete"]')).toBeNull();
    expect(root.querySelector('[data-records] [data-action="details"]')).not.toBeNull();
  });

  it('offers all actions to administrators', async () => {
    const root = mount('organizations', admin);
    await vi.waitFor(() => expect(root.querySelector('[data-records] [data-action="edit"]')).not.toBeNull());
    expect(root.querySelector<HTMLButtonElement>('[data-action="create"]')!.hidden).toBe(false);
    expect(root.querySelector<HTMLElement>('[data-permission-note]')!.hidden).toBe(true);
  });

  it('does not request the admin-only team list for non-admins', () => {
    const root = mount('users', member);
    expect(webApiClient.subscribeList).not.toHaveBeenCalled();
    expect(root.querySelector<HTMLElement>('[data-listing]')!.hidden).toBe(true);
    expect(root.querySelector('[data-permission-note]')?.textContent).toContain('administrator access');
  });

  it('offers only the signed-in user in person pickers for non-admins and defaults calendar items to them', async () => {
    rows = [{ id: 'appt-1', createdAt: '2026-09-16T10:00:00Z', description: 'Standup', userId: 'someone-else', assignmentId: 'task-1' }];
    vi.mocked(webApiClient.lookup).mockResolvedValue([{ id: 'task-1', name: 'Task one' }]);
    const root = mount('appointments', member);
    await vi.waitFor(() => expect(root.querySelector('[data-records]')?.textContent).toContain('Task one'));
    root.querySelector<HTMLButtonElement>('[data-action="create"]')!.click();
    const person = field<HTMLSelectElement>(root, 'userId');
    expect(person.value).toBe('user-7');
    expect(person.selectedOptions[0].textContent).toBe('learner@cpnucleo.test');
    const picker = relation(root, 'users');
    expect(picker.querySelector<HTMLElement>('[data-relation-search]')!.hidden).toBe(true);
    expect(picker.querySelector<HTMLElement>('[data-relation-note]')!.hidden).toBe(false);
    await vi.waitFor(() => expect(webApiClient.list).toHaveBeenCalledWith('assignments', 1, 100, expect.any(AbortSignal), ''));
    expect(vi.mocked(webApiClient.list).mock.calls.some(([key]) => key === 'users')).toBe(false);
    expect(vi.mocked(webApiClient.lookup).mock.calls.some(([key]) => key === 'users')).toBe(false);
    expect([...person.options].map(option => option.value)).toEqual(['', 'user-7']);
  });

  it('defaults new calendar items to the signed-in administrator too', async () => {
    vi.mocked(webApiClient.list).mockImplementation(async key => ({ items: key === 'users' ? [{ id: 'admin-1', name: 'Admin', login: 'admin@cpnucleo.test' }] : [], totalCount: 1 }));
    const root = mount('appointments', admin);
    root.querySelector<HTMLButtonElement>('[data-action="create"]')!.click();
    expect(field<HTMLSelectElement>(root, 'userId').value).toBe('admin-1');
    await vi.waitFor(() => expect(webApiClient.list).toHaveBeenCalledWith('users', 1, 100, expect.any(AbortSignal), ''));
  });
});

describe('accessibility', () => {
  it('gives each row action a distinct accessible name', async () => {
    rows = [
      { id: 'a', createdAt: '2026-09-16T10:00:00Z', name: 'Atlas', organizationId: 'org' },
      { id: 'b', createdAt: '2026-09-16T10:00:00Z', name: 'Borealis', organizationId: 'org' },
    ];
    const root = mount('projects');
    await vi.waitFor(() => expect(root.querySelectorAll('[data-records] tr')).toHaveLength(2));
    const names = [...root.querySelectorAll('[data-records] button')].map(button => button.getAttribute('aria-label'));
    expect(names).toEqual(['Details for Atlas', 'Edit Atlas', 'Delete Atlas', 'Details for Borealis', 'Edit Borealis', 'Delete Borealis']);
  });

  it('names link records by their related labels', async () => {
    rows = [{ id: 'link', createdAt: '2026-09-16T10:00:00Z', userId: 'user-1', projectId: 'project-1' }];
    vi.mocked(webApiClient.lookup).mockImplementation(async key => (key === 'users' ? [{ id: 'user-1', name: 'Ana', login: 'ana' }] : [{ id: 'project-1', name: 'Atlas' }]));
    const root = mount('user-projects');
    await vi.waitFor(() => expect(root.querySelector('[data-records] [data-action="edit"]')?.getAttribute('aria-label')).toBe('Edit Ana (ana) / Atlas'));
  });

  it('returns focus to the triggering control when the form is cancelled or saved', async () => {
    const root = mount('projects');
    vi.spyOn(webApiClient, 'update').mockImplementation(async () => {
      rows = [{ ...rows[0], name: 'Renamed' }];
      return {};
    });
    await vi.waitFor(() => expect(root.querySelector('[data-records] [data-action="edit"]')).not.toBeNull());
    const create = root.querySelector<HTMLButtonElement>('[data-action="create"]')!;
    create.focus(); create.click();
    expect(root.contains(document.activeElement) && document.activeElement?.closest('[data-form]')).toBeTruthy();
    root.querySelector<HTMLButtonElement>('[data-action="cancel"]')!.click();
    expect(document.activeElement).toBe(create);

    const edit = root.querySelector<HTMLButtonElement>('[data-records] [data-action="edit"]')!;
    edit.focus(); edit.click();
    await vi.waitFor(() => expect(field<HTMLSelectElement>(root, 'organizationId').value).toBe('org'));
    submit(root);
    // Saving re-subscribes; the new snapshot replaces the rows, and focus follows to the new Edit button.
    await vi.waitFor(() => expect(root.querySelector('[data-records]')?.textContent).toContain('Renamed'));
    expect(document.activeElement).toBe(root.querySelector('[data-records] [data-action="edit"][data-id="one"]'));
    expect(document.activeElement).not.toBe(edit);
  });

  it('moves focus to a stable control after the deleted row disappears', async () => {
    const root = mount('projects');
    vi.spyOn(webApiClient, 'delete').mockImplementation(async () => { rows = []; });
    await vi.waitFor(() => expect(root.querySelector('[data-action="delete"]')).not.toBeNull());
    const remove = root.querySelector<HTMLButtonElement>('[data-action="delete"]')!;
    remove.focus();
    await deleteRow(root, remove);
    await vi.waitFor(() => expect(root.querySelectorAll('[data-records] tr')).toHaveLength(0));
    expect(document.activeElement).toBe(root.querySelector('[data-action="create"]'));
  });

  it('reports option loading through a polite status and errors through an alert', async () => {
    const root = mount('projects');
    vi.mocked(webApiClient.list).mockRejectedValueOnce(new ApiError(500, 'An unexpected error occurred.'));
    root.querySelector<HTMLButtonElement>('[data-action="create"]')!.click();
    const container = relation(root, 'organizations');
    expect(container.querySelector('[data-relation-status]')?.textContent).toBe('Loading options…');
    await vi.waitFor(() => expect(container.querySelector('[data-relation-error]')?.textContent).toBe('An unexpected error occurred.'));
    expect(container.querySelector('[data-relation-error]')?.getAttribute('role')).toBe('alert');
    expect(container.querySelector('[data-relation-status]')?.textContent).toBe('');
  });
});
