import { describe, expect, it } from 'vitest';
import { readFileSync } from 'node:fs';
import { fileURLToPath } from 'node:url';

const sourcePath = fileURLToPath(new URL('./index.tsx', import.meta.url));
const source = readFileSync(sourcePath, 'utf8');

describe('home page resource counters', () => {
  it('loads counts for every work-area card instead of only the featured cards', () => {
    expect(source).toContain('Promise.all(resourceMetadata.map(async (resource) => {');
    expect(source).not.toContain('resourceMetadata.slice(0, 6).map(async (resource)');
  });

  it('requests the smallest page needed when only totalCount is displayed', () => {
    expect(source).toContain('webApiClient.list(resource.key, 1, 1)');
    expect(source).not.toContain('webApiClient.list(resource.key, 1, 5)');
  });
});
