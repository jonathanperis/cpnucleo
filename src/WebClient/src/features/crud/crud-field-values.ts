import type { FieldMetadata, ResourceKey } from '~/lib/api/types';

const toDate = (value: unknown) => {
  if (value instanceof Date && !Number.isNaN(value.getTime())) return value;
  if (typeof value !== 'string' && typeof value !== 'number') return undefined;
  const date = new Date(value);
  return Number.isNaN(date.getTime()) ? undefined : date;
};

const pad = (value: number) => String(value).padStart(2, '0');

const formatDateInputValue = (value: unknown) => {
  if (typeof value === 'string') {
    const match = value.match(/^(\d{4}-\d{2}-\d{2})/);
    if (match) return match[1];
  }

  const date = toDate(value);
  if (!date) return value == null ? '' : String(value);
  return `${date.getUTCFullYear()}-${pad(date.getUTCMonth() + 1)}-${pad(date.getUTCDate())}`;
};

const formatDateTimeLocalInputValue = (value: unknown) => {
  if (typeof value === 'string') {
    const match = value.match(/^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2})/);
    if (match) return match[1];
  }

  const date = toDate(value);
  if (!date) return value == null ? '' : String(value);
  return `${date.getUTCFullYear()}-${pad(date.getUTCMonth() + 1)}-${pad(date.getUTCDate())}T${pad(date.getUTCHours())}:${pad(date.getUTCMinutes())}`;
};

export const formatFormFieldValue = (value: unknown, fieldType: FieldMetadata['type']) => {
  if (value == null) return '';
  if (fieldType === 'date') return formatDateInputValue(value);
  if (fieldType === 'datetime-local') return formatDateTimeLocalInputValue(value);
  return String(value);
};

export const relationOptionsLoaded = (relations: Partial<Record<ResourceKey, unknown[]>>, relation: ResourceKey | undefined) => {
  if (!relation) return true;
  return Object.prototype.hasOwnProperty.call(relations, relation);
};
