import { describe, expect, it } from 'vitest';
import { findResource } from '~/lib/api/resource-metadata';
import { defaultListState, parseListState, serializeListState } from './list-state';

const project = 'a1b2c3d4-0000-4000-8000-000000000001';
const tasks = findResource('assignments');

describe('list state in the URL', () => {
  it('defaults to newest first on page 1 and serializes defaults to an empty query', () => {
    const { state, intent } = parseListState(tasks, '');
    expect(state).toEqual(defaultListState());
    expect(state).toMatchObject({ sort: 'createdAt', order: 'desc', page: 1, pageSize: 10 });
    expect(intent).toEqual({ create: false, editId: null });
    expect(serializeListState(state)).toBe('');
  });

  it('round-trips search, sort, page size, page and the filters this resource supports', () => {
    const query = `?search=plan&projectId=${project}&sort=endDate&order=asc&size=25&page=3`;
    const { state } = parseListState(tasks, query);
    expect(state).toMatchObject({ search: 'plan', filters: { projectId: project }, sort: 'endDate', order: 'asc', pageSize: 25, page: 3 });
    expect(serializeListState(state)).toBe(query);
  });

  it('drops values the resource cannot use instead of sending them to the API', () => {
    const { state } = parseListState(findResource('organizations'), `?projectId=${project}&sort=password&size=7&page=-2&ids=nope,${project}`);
    expect(state.filters).toEqual({});
    expect(state.sort).toBe('createdAt');
    expect(state.pageSize).toBe(10);
    expect(state.page).toBe(1);
    expect(state.ids).toEqual([project]);
    expect(parseListState(tasks, '?projectId=not-a-uuid').state.filters).toEqual({});
    expect(parseListState(tasks, `?search=${'x'.repeat(300)}`).state.search).toHaveLength(128);
  });

  it('sorts text columns ascending by default and the created date descending', () => {
    expect(parseListState(tasks, '?sort=name').state.order).toBe('asc');
    expect(parseListState(tasks, '?sort=createdAt').state.order).toBe('desc');
  });

  it('reads one-time intents to open the create or edit form', () => {
    expect(parseListState(tasks, '?new=1').intent).toEqual({ create: true, editId: null });
    expect(parseListState(tasks, `?edit=${project}`).intent).toEqual({ create: false, editId: project });
    expect(parseListState(tasks, '?edit=1;drop').intent.editId).toBeNull();
    expect(serializeListState(parseListState(tasks, `?new=1&edit=${project}`).state)).toBe('');
  });
});
