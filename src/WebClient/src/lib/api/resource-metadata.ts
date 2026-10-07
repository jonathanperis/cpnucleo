import { relationFilterKeys, type FieldMetadata, type RelationFilterKey, type ResourceAccess, type ResourceKey, type ResourceMetadata } from './types';

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
  extra: Pick<ResourceMetadata, 'dateRange'> & { access?: ResourceAccess } = {},
): ResourceMetadata => ({ key, label, pluralLabel, listPath: `/${key}`, itemPath: `/${itemPath}`, routePath, description, displayField, fields: [...baseFields, ...fields], ...extra, access: extra.access ?? {} });

// UI gating mirrors the API authorization model; the APIs remain the enforcement point.
const adminWrite: ResourceAccess = { adminWrite: true };

export const resourceMetadata = [
  resource('organizations', 'Organization', 'Organizations', 'organization', '/organizations/', 'Teams and groups that own projects.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'description', label: 'Description', type: 'textarea', table: true },
  ], { access: adminWrite }),
  resource('projects', 'Project', 'Projects', 'project', '/projects/', 'Project spaces connected to an organization.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'organizationId', label: 'Organization', type: 'guid', required: true, table: true, relation: 'organizations' },
  ]),
  resource('assignments', 'Task', 'Tasks', 'assignment', '/assignments/', 'Work items with dates, hours, progress, and owner links.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'description', label: 'Description', type: 'textarea', required: true, table: true, hiddenByDefault: true },
    { name: 'startDate', label: 'Start date', type: 'date', required: true, table: true },
    { name: 'endDate', label: 'End date', type: 'date', required: true, table: true },
    { name: 'amountHours', label: 'Hours', type: 'number', required: true, min: 1, table: true },
    { name: 'projectId', label: 'Project', type: 'guid', required: true, table: true, relation: 'projects' },
    { name: 'workflowId', label: 'Progress step', type: 'guid', required: true, table: true, relation: 'workflows' },
    { name: 'userId', label: 'Owner', type: 'guid', required: true, table: true, relation: 'users' },
    { name: 'assignmentTypeId', label: 'Task type', type: 'guid', required: true, table: true, relation: 'assignmentTypes', hiddenByDefault: true },
  ], { dateRange: { start: 'startDate', end: 'endDate' } }),
  resource('assignmentTypes', 'Task type', 'Task types', 'assignmentType', '/assignment-types/', 'Reusable labels for different kinds of work.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
  ], { access: adminWrite }),
  resource('impediments', 'Blocker', 'Blockers', 'impediment', '/impediments/', 'Issues that may slow work down.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
  ], { access: adminWrite }),
  resource('assignmentImpediments', 'Task blocker', 'Task blockers', 'assignmentImpediment', '/assignment-impediments/', 'Notes that connect blockers to a task.', 'description', [
    { name: 'description', label: 'Description', type: 'textarea', required: true, table: true },
    { name: 'assignmentId', label: 'Task', type: 'guid', required: true, table: true, relation: 'assignments' },
    { name: 'impedimentId', label: 'Blocker', type: 'guid', required: true, table: true, relation: 'impediments' },
  ]),
  resource('appointments', 'Calendar item', 'Calendar items', 'appointment', '/appointments/', 'Scheduled moments connected to people and tasks.', 'description', [
    { name: 'description', label: 'Description', type: 'textarea', required: true, table: true },
    { name: 'keepDate', label: 'Date', type: 'datetime-local', required: true, table: true },
    { name: 'amountHours', label: 'Hours', type: 'number', required: true, min: 1, table: true },
    { name: 'assignmentId', label: 'Task', type: 'guid', required: true, table: true, relation: 'assignments' },
    { name: 'userId', label: 'Person', type: 'guid', required: true, table: true, relation: 'users' },
  ]),
  resource('workflows', 'Progress step', 'Progress steps', 'workflow', '/workflows/', 'Steps that show where a task is in the work path.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'order', label: 'Order', type: 'number', required: true, min: 1, table: true },
  ], { access: adminWrite }),
  resource('users', 'Team member', 'Team members', 'user', '/users/', 'People who can own projects, tasks, and calendar items.', 'name', [
    { name: 'name', label: 'Name', type: 'text', required: true, table: true },
    { name: 'login', label: 'Login', type: 'text', required: true, table: true },
    { name: 'password', label: 'Password (leave blank to keep it when editing)', type: 'password', requiredOnCreate: true },
  ], { access: { adminRead: true, adminWrite: true } }),
  resource('userAssignments', 'Person on task', 'People on tasks', 'userAssignment', '/user-assignments/', 'Connections between people and the tasks they help with.', 'id', [
    { name: 'userId', label: 'Person', type: 'guid', required: true, table: true, relation: 'users' },
    { name: 'assignmentId', label: 'Task', type: 'guid', required: true, table: true, relation: 'assignments' },
  ]),
  resource('userProjects', 'Person on project', 'People on projects', 'userProject', '/user-projects/', 'Connections between people and their project spaces.', 'id', [
    { name: 'userId', label: 'Person', type: 'guid', required: true, table: true, relation: 'users' },
    { name: 'projectId', label: 'Project', type: 'guid', required: true, table: true, relation: 'projects' },
  ]),
] as const satisfies readonly ResourceMetadata[];

export const resources = resourceMetadata.map((resource) => resource.key) as ResourceKey[];
export const resourceMap = Object.fromEntries(resourceMetadata.map((resource) => [resource.key, resource])) as Record<ResourceKey, ResourceMetadata>;

export const findResource = (key: ResourceKey) => resourceMap[key];
/** Listed columns; Created goes last so each row leads with its own data. */
export const tableFields = (resource: ResourceMetadata) => {
  const listed = resource.fields.filter((field) => field.table);
  return [...listed.filter((field) => field.name !== 'createdAt'), ...listed.filter((field) => field.name === 'createdAt')];
};
export const formFields = (resource: ResourceMetadata) => resource.fields.filter((field) => !field.readOnly);

/** Relation fields this resource's list can be narrowed by (`/assignments/?projectId=…`). */
export const filterFields = (resource: ResourceMetadata) =>
  resource.fields.filter((field): field is FieldMetadata & { name: RelationFilterKey } =>
    Boolean(field.relation) && (relationFilterKeys as readonly string[]).includes(field.name));

/** Resources with their own detail page. */
const detailRoutes: Partial<Record<ResourceKey, string>> = { projects: '/projects/view/', assignments: '/assignments/view/' };

/** The page that shows one record: its detail page, or its list narrowed to that record. */
export const recordHref = (key: ResourceKey, id: string) => {
  const detail = detailRoutes[key];
  return detail ? `${detail}?id=${encodeURIComponent(id)}` : `${findResource(key).routePath}?ids=${encodeURIComponent(id)}`;
};
