import type { ApiEntity, FieldMetadata, ResourceKey } from '~/lib/api/types';

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
  if (fieldType === 'password') return '';
  if (value == null) return '';
  if (fieldType === 'date') return formatDateInputValue(value);
  if (fieldType === 'datetime-local') return formatDateTimeLocalInputValue(value);
  return String(value);
};

export const serializeFormValue = (value: string, fieldType: FieldMetadata['type']): string | number => {
  if (fieldType === 'number') return Number(value);
  if (fieldType === 'date') return `${value}T00:00:00.000Z`;
  if (fieldType === 'datetime-local') return new Date(`${value}Z`).toISOString();
  return value;
};

export const relationOptionsLoaded = (relations: Partial<Record<ResourceKey, unknown[]>>, relation: ResourceKey | undefined) => {
  if (!relation) return true;
  return Object.prototype.hasOwnProperty.call(relations, relation);
};

export const withSelectedRelationOption = (options: ApiEntity[], selectedValue: string): ApiEntity[] => {
  if (!selectedValue || options.some((option) => String(option.id ?? '') === selectedValue)) return options;
  return [{ id: selectedValue }, ...options];
};
