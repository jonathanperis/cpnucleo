import { describe, expect, it } from 'vitest';
import { formFields, resourceMetadata, resources, tableFields } from './resource-metadata';

describe('resource metadata', () => {
  it('covers all WebApi resources with singular item and plural list paths', () => {
    expect(resources).toEqual([
      'organizations', 'projects', 'assignments', 'assignmentTypes', 'impediments', 'assignmentImpediments', 'appointments', 'workflows', 'users', 'userAssignments', 'userProjects',
    ]);
    expect(resourceMetadata).toHaveLength(11);
    expect(resourceMetadata.every((resource) => resource.listPath.startsWith('/') && resource.itemPath.startsWith('/'))).toBe(true);
  });

  it('uses canonical trailing-slash static page routes', () => {
    expect(resourceMetadata.every((resource) => resource.routePath.startsWith('/') && resource.routePath.endsWith('/'))).toBe(true);
  });

  it('keeps passwords out of user table/form metadata', () => {
    const users = resourceMetadata.find((resource) => resource.key === 'users');
    expect(users).toBeDefined();
    expect(tableFields(users!).map((field) => field.name)).toEqual(['createdAt', 'name', 'login']);
    expect(formFields(users!).some((field) => /password/i.test(field.name))).toBe(false);
  });

  it('marks fields required when WebApi update endpoints require them', () => {
    const assignments = resourceMetadata.find((resource) => resource.key === 'assignments')!;
    const appointments = resourceMetadata.find((resource) => resource.key === 'appointments')!;

    expect(formFields(assignments).filter((field) => field.required).map((field) => field.name)).toEqual([
      'name', 'description', 'startDate', 'endDate', 'amountHours', 'projectId', 'workflowId', 'userId', 'assignmentTypeId',
    ]);
    expect(formFields(appointments).find((field) => field.name === 'amountHours')).toMatchObject({ required: true });
  });

  it('declares relation selectors for foreign keys', () => {
    const assignments = resourceMetadata.find((resource) => resource.key === 'assignments')!;
    expect(assignments.fields.filter((field) => field.relation).map((field) => field.relation)).toEqual(['projects', 'workflows', 'users', 'assignmentTypes']);
  });
});
