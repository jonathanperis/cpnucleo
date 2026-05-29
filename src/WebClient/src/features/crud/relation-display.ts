import type { ApiEntity, FieldMetadata, ResourceKey } from '~/lib/api/types';

export type RelationRecords = Partial<Record<ResourceKey, ApiEntity[]>>;

export const formatValue = (value: unknown): string => {
  if (value === null || value === undefined || value === '') return '—';
  if (typeof value === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(value)) return new Date(value).toLocaleString();
  return String(value);
};

const nonEmptyString = (value: unknown): string => typeof value === 'string' && value.trim() ? value.trim() : '';

export const mergeRelationRecords = (existing: ApiEntity[], incoming: ApiEntity[]): ApiEntity[] => {
  const existingIds = new Set(existing.map((entity) => String(entity.id ?? '')));
  return [...existing, ...incoming.filter((entity) => !existingIds.has(String(entity.id ?? '')))];
};

export const displayEntityLabel = (entity: ApiEntity | undefined): string => {
  if (!entity) return '—';
  const name = nonEmptyString(entity.name);
  const description = nonEmptyString(entity.description);
  const login = nonEmptyString(entity.login);
  if (name && description && description !== name) return `${name} — ${description}`;
  if (name && login && login !== name) return `${name} (${login})`;
  return formatValue(name || description || login || entity.id);
};

export const displayFieldValue = (value: unknown, relation: ResourceKey | undefined, relations: RelationRecords): string => {
  if (!relation) return formatValue(value);
  const related = relations[relation]?.find((entity) => String(entity.id ?? '') === String(value ?? ''));
  return related ? displayEntityLabel(related) : formatValue(value);
};

export const collectMissingRelationIds = (items: ApiEntity[], fields: FieldMetadata[], relations: RelationRecords): Partial<Record<ResourceKey, string[]>> => {
  const missing: Partial<Record<ResourceKey, string[]>> = {};

  for (const field of fields) {
    if (!field.relation) continue;
    const loadedIds = new Set((relations[field.relation] ?? []).map((entity) => String(entity.id ?? '')));
    const missingIds = new Set<string>();

    for (const item of items) {
      const value = item[field.name];
      if (value === null || value === undefined || value === '') continue;
      const id = String(value);
      if (!loadedIds.has(id)) missingIds.add(id);
    }

    if (missingIds.size > 0) missing[field.relation] = [...missingIds];
  }

  return missing;
};
