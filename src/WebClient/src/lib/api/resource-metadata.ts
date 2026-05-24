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
  resource('organizations', 'Organization', 'Organizations', 'organization', '/organizations', 'Teams and groups that own projects.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'description', label: 'Description', type: 'textarea', table: true },
  ]),
  resource('projects', 'Project', 'Projects', 'project', '/projects', 'Project spaces connected to an organization.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'organizationId', label: 'Organization', type: 'guid', required: true, table: true, relation: 'organizations' },
  ]),
  resource('assignments', 'Task', 'Tasks', 'assignment', '/assignments', 'Work items with dates, hours, progress, and owner links.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'description', label: 'Description', type: 'textarea', table: true },
    { name: 'startDate', label: 'Start date', type: 'date', table: true },
    { name: 'endDate', label: 'End date', type: 'date', table: true },
    { name: 'amountHours', label: 'Hours', type: 'number', table: true },
    { name: 'projectId', label: 'Project', type: 'guid', required: true, table: true, relation: 'projects' },
    { name: 'workflowId', label: 'Progress step', type: 'guid', table: true, relation: 'workflows' },
    { name: 'userId', label: 'Owner', type: 'guid', table: true, relation: 'users' },
    { name: 'assignmentTypeId', label: 'Task type', type: 'guid', table: true, relation: 'assignmentTypes' },
  ]),
  resource('assignmentTypes', 'Task type', 'Task types', 'assignmentType', '/assignment-types', 'Reusable labels for different kinds of work.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
  ]),
  resource('impediments', 'Blocker', 'Blockers', 'impediment', '/impediments', 'Issues that may slow work down.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
  ]),
  resource('assignmentImpediments', 'Task blocker', 'Task blockers', 'assignmentImpediment', '/assignment-impediments', 'Notes that connect blockers to a task.', 'description', [
    { name: 'description', label: 'Description', type: 'textarea', required: true, table: true },
    { name: 'assignmentId', label: 'Task', type: 'guid', required: true, table: true, relation: 'assignments' },
    { name: 'impedimentId', label: 'Blocker', type: 'guid', required: true, table: true, relation: 'impediments' },
  ]),
  resource('appointments', 'Calendar item', 'Calendar items', 'appointment', '/appointments', 'Scheduled moments connected to people and tasks.', 'description', [
    { name: 'description', label: 'Description', type: 'textarea', required: true, table: true },
    { name: 'keepDate', label: 'Date', type: 'datetime-local', required: true, table: true },
    { name: 'amountHours', label: 'Hours', type: 'number', table: true },
    { name: 'assignmentId', label: 'Task', type: 'guid', required: true, table: true, relation: 'assignments' },
    { name: 'userId', label: 'Person', type: 'guid', required: true, table: true, relation: 'users' },
  ]),
  resource('workflows', 'Progress step', 'Progress steps', 'workflow', '/workflows', 'Steps that show where a task is in the work path.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'order', label: 'Order', type: 'number', table: true },
  ]),
  resource('users', 'Team member', 'Team members', 'user', '/users', 'People who can own projects, tasks, and calendar items.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'login', label: 'Login', type: 'text', required: true, table: true },
  ]),
  resource('userAssignments', 'Person on task', 'People on tasks', 'userAssignment', '/user-assignments', 'Connections between people and the tasks they help with.', 'id', [
    { name: 'userId', label: 'Person', type: 'guid', required: true, table: true, relation: 'users' },
    { name: 'assignmentId', label: 'Task', type: 'guid', required: true, table: true, relation: 'assignments' },
  ]),
  resource('userProjects', 'Person on project', 'People on projects', 'userProject', '/user-projects', 'Connections between people and their project spaces.', 'id', [
    { name: 'userId', label: 'Person', type: 'guid', required: true, table: true, relation: 'users' },
    { name: 'projectId', label: 'Project', type: 'guid', required: true, table: true, relation: 'projects' },
  ]),
] as const satisfies readonly ResourceMetadata[];

export const resources = resourceMetadata.map((resource) => resource.key) as ResourceKey[];
export const resourceMap = Object.fromEntries(resourceMetadata.map((resource) => [resource.key, resource])) as Record<ResourceKey, ResourceMetadata>;

export const findResource = (key: ResourceKey) => resourceMap[key];
export const tableFields = (resource: ResourceMetadata) => resource.fields.filter((field) => field.table);
export const formFields = (resource: ResourceMetadata) => resource.fields.filter((field) => !field.readOnly);
