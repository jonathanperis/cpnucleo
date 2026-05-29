import { describe, expect, it } from 'vitest';
import { readFileSync } from 'node:fs';
import { fileURLToPath } from 'node:url';

const sourcePath = fileURLToPath(new URL('./index.tsx', import.meta.url));
const source = readFileSync(sourcePath, 'utf8');

describe('home page resource counters', () => {
  it('loads counts for every work-area card instead of only the featured cards', () => {
    expect(source).toMatch(/Promise\.all\(\s*resourceMetadata\.map\(/);
    expect(source).not.toMatch(/resourceMetadata\.slice\(\s*0\s*,\s*6\s*\)\.map\(/);
  });

  it('requests the smallest page needed when only totalCount is displayed', () => {
    expect(source).toMatch(/webApiClient\.list\(\s*resource\.key\s*,\s*1\s*,\s*1\s*\)/);
    expect(source).not.toMatch(/webApiClient\.list\(\s*resource\.key\s*,\s*1\s*,\s*5\s*\)/);
  });
});
