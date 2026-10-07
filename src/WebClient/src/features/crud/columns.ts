import { tableFields } from '~/lib/api/resource-metadata';
import type { FieldMetadata, ResourceMetadata } from '~/lib/api/types';

export const columnsStorageKey = (resource: ResourceMetadata) => `cpnucleo.columns.${resource.key}`;

/** Hidden column names: the stored choice, or the resource's `hiddenByDefault` columns. */
export const readHiddenColumns = (resource: ResourceMetadata, storage: Pick<Storage, 'getItem'> | undefined = globalThis.localStorage): Set<string> => {
  const names = new Set(tableFields(resource).map(field => field.name));
  try {
    const stored = storage?.getItem(columnsStorageKey(resource));
    if (stored !== null && stored !== undefined) {
      const parsed = JSON.parse(stored) as unknown;
      if (Array.isArray(parsed)) return new Set(parsed.filter((name): name is string => typeof name === 'string' && names.has(name)));
    }
  } catch {
    // Unreadable preferences fall back to the defaults.
  }
  return new Set(tableFields(resource).filter(field => field.hiddenByDefault).map(field => field.name));
};

export const writeHiddenColumns = (resource: ResourceMetadata, hidden: Set<string>, storage: Pick<Storage, 'setItem'> | undefined = globalThis.localStorage) => {
  try {
    storage?.setItem(columnsStorageKey(resource), JSON.stringify([...hidden]));
  } catch {
    // Storage can be unavailable (privacy mode); the choice still applies to this page.
  }
};

/** Listed columns that are shown; the first column (the record's name) can never be hidden. */
export const visibleColumns = (resource: ResourceMetadata, hidden: Set<string>): FieldMetadata[] => {
  const fields = tableFields(resource);
  return fields.filter((field, index) => index === 0 || !hidden.has(field.name));
};
