export type FieldType = 'text' | 'textarea' | 'number' | 'date' | 'datetime-local' | 'guid';

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
  table?: boolean;
  relation?: ResourceKey;
  readOnly?: boolean;
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
