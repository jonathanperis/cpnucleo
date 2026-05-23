import type { FieldMetadata, ResourceKey, ResourceMetadata } from './types';

const baseFields: FieldMetadata[] = [
  { name: 'id', label: 'ID', type: 'guid', table: false, readOnly: true },
  { name: 'createdAt', label: 'Created', type: 'datetime-local', table: true, readOnly: true },
];

const resource = (
  key: ResourceKey,
  label: string,
  pluralLabel: string,
  itemPath: string,
  routePath: string,
  description: string,
  displayField: string,
  fields: FieldMetadata[],
): ResourceMetadata => ({ key, label, pluralLabel, listPath: `/${key}`, itemPath: `/${itemPath}`, routePath, description, displayField, fields: [...baseFields, ...fields] });

export const resourceMetadata = [
  resource('organizations', 'Organization', 'Organizations', 'organization', '/organizations', 'Companies and teams that own projects.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'description', label: 'Description', type: 'textarea', table: true },
  ]),
  resource('projects', 'Project', 'Projects', 'project', '/projects', 'Project records linked to an organization.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'organizationId', label: 'Organization', type: 'guid', required: true, table: true, relation: 'organizations' },
  ]),
  resource('assignments', 'Assignment', 'Assignments', 'assignment', '/assignments', 'Work items with schedule, budget, workflow, and people links.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'description', label: 'Description', type: 'textarea', table: true },
    { name: 'startDate', label: 'Start date', type: 'date', table: true },
    { name: 'endDate', label: 'End date', type: 'date', table: true },
    { name: 'amountHours', label: 'Hours', type: 'number', table: true },
    { name: 'projectId', label: 'Project', type: 'guid', required: true, table: true, relation: 'projects' },
    { name: 'workflowId', label: 'Workflow', type: 'guid', table: true, relation: 'workflows' },
    { name: 'userId', label: 'User', type: 'guid', table: true, relation: 'users' },
    { name: 'assignmentTypeId', label: 'Assignment type', type: 'guid', table: true, relation: 'assignmentTypes' },
  ]),
  resource('assignmentTypes', 'Assignment type', 'Assignment types', 'assignmentType', '/assignment-types', 'Reusable categories for assignments.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
  ]),
  resource('impediments', 'Impediment', 'Impediments', 'impediment', '/impediments', 'Known blockers that can affect work.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
  ]),
  resource('assignmentImpediments', 'Assignment impediment', 'Assignment impediments', 'assignmentImpediment', '/assignment-impediments', 'Links blockers to assignments with context.', 'description', [
    { name: 'description', label: 'Description', type: 'textarea', required: true, table: true },
    { name: 'assignmentId', label: 'Assignment', type: 'guid', required: true, table: true, relation: 'assignments' },
    { name: 'impedimentId', label: 'Impediment', type: 'guid', required: true, table: true, relation: 'impediments' },
  ]),
  resource('appointments', 'Appointment', 'Appointments', 'appointment', '/appointments', 'Calendar entries for users on assignments.', 'description', [
    { name: 'description', label: 'Description', type: 'textarea', required: true, table: true },
    { name: 'keepDate', label: 'Date', type: 'datetime-local', required: true, table: true },
    { name: 'amountHours', label: 'Hours', type: 'number', table: true },
    { name: 'assignmentId', label: 'Assignment', type: 'guid', required: true, table: true, relation: 'assignments' },
    { name: 'userId', label: 'User', type: 'guid', required: true, table: true, relation: 'users' },
  ]),
  resource('workflows', 'Workflow', 'Workflows', 'workflow', '/workflows', 'Ordered workflow states for assignment progress.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'order', label: 'Order', type: 'number', table: true },
  ]),
  resource('users', 'User', 'Users', 'user', '/users', 'People that can own projects, assignments, and appointments.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'login', label: 'Login', type: 'text', required: true, table: true },
  ]),
  resource('userAssignments', 'User assignment', 'User assignments', 'userAssignment', '/user-assignments', 'Join records between users and assignments.', 'id', [
    { name: 'userId', label: 'User', type: 'guid', required: true, table: true, relation: 'users' },
    { name: 'assignmentId', label: 'Assignment', type: 'guid', required: true, table: true, relation: 'assignments' },
  ]),
  resource('userProjects', 'User project', 'User projects', 'userProject', '/user-projects', 'Join records between users and projects.', 'id', [
    { name: 'userId', label: 'User', type: 'guid', required: true, table: true, relation: 'users' },
    { name: 'projectId', label: 'Project', type: 'guid', required: true, table: true, relation: 'projects' },
  ]),
] as const satisfies readonly ResourceMetadata[];

export const resources = resourceMetadata.map((resource) => resource.key) as ResourceKey[];
export const resourceMap = Object.fromEntries(resourceMetadata.map((resource) => [resource.key, resource])) as Record<ResourceKey, ResourceMetadata>;

export const findResource = (key: ResourceKey) => resourceMap[key];
export const tableFields = (resource: ResourceMetadata) => resource.fields.filter((field) => field.table);
export const formFields = (resource: ResourceMetadata) => resource.fields.filter((field) => !field.readOnly);
