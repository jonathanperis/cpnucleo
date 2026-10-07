export type FieldType = 'text' | 'textarea' | 'number' | 'date' | 'datetime-local' | 'guid' | 'password';

export type ResourceKey =
  | 'organizations'
  | 'projects'
  | 'assignments'
  | 'assignmentTypes'
  | 'impediments'
  | 'assignmentImpediments'
  | 'appointments'
  | 'workflows'
  | 'users'
  | 'userAssignments'
  | 'userProjects';

export interface FieldMetadata {
  name: string;
  label: string;
  type: FieldType;
  required?: boolean;
  requiredOnCreate?: boolean;
  table?: boolean;
  relation?: ResourceKey;
  readOnly?: boolean;
  /** Minimum for number inputs (whole numbers). */
  min?: number;
  /** Listed column that starts hidden; people can show it from the Columns menu. */
  hiddenByDefault?: boolean;
}

/** Flat list filters the APIs accept (`?projectId=…`); each narrows a list to one related record. */
export const relationFilterKeys = ['organizationId', 'projectId', 'assignmentId', 'userId', 'workflowId'] as const;
export type RelationFilterKey = typeof relationFilterKeys[number];

export interface ListOptions {
  /** Persisted canonical column (PascalCase) and direction. */
  sort?: { column: string; order: 'ASC' | 'DESC' };
  filters?: Partial<Record<RelationFilterKey, string>>;
  /** ISO-8601 instants: `KeepDate` within the range, or start/end dates overlapping it. */
  dateFrom?: string;
  dateTo?: string;
}

export interface ResourceAccess {
  /** Listing/reading requires the `cpnucleo:admin` claim (team members). */
  adminRead?: boolean;
  /** Create, update and delete require the `cpnucleo:admin` claim (reference data and users). */
  adminWrite?: boolean;
}

export interface ResourceMetadata {
  key: ResourceKey;
  label: string;
  pluralLabel: string;
  listPath: string;
  itemPath: string;
  routePath: string;
  description: string;
  displayField: string;
  fields: FieldMetadata[];
  access: ResourceAccess;
  /** Client-side check that `end` is on or after `start` (same rule as the domain factories). */
  dateRange?: { start: string; end: string };
}

export interface ApiEntity {
  id?: string;
  createdAt?: string;
  [key: string]: unknown;
}

export interface PaginatedResult<T> {
  items?: T[];
  data?: T[];
  results?: T[];
  totalCount?: number;
  total?: number;
  pageNumber?: number;
  page?: number;
  pageSize?: number;
}

export interface ApiErrorShape {
  status: number;
  message: string;
  details?: unknown;
}
