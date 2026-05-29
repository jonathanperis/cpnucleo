import type { ApiEntity, ResourceKey } from '~/lib/api/types';

export type RelationRecords = Partial<Record<ResourceKey, ApiEntity[]>>;

export const formatValue = (value: unknown): string => {
  if (value === null || value === undefined || value === '') return '—';
  if (typeof value === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(value)) return new Date(value).toLocaleString();
  return String(value);
};

export const displayEntityLabel = (entity: ApiEntity | undefined): string => {
  if (!entity) return '—';
  return formatValue(entity.name ?? entity.description ?? entity.login ?? entity.id);
};

export const displayFieldValue = (value: unknown, relation: ResourceKey | undefined, relations: RelationRecords): string => {
  if (!relation) return formatValue(value);
  const related = relations[relation]?.find((entity) => String(entity.id ?? '') === String(value ?? ''));
  return related ? displayEntityLabel(related) : formatValue(value);
};
