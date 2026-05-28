import { readFileSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';
import { describe, expect, it } from 'vitest';

const source = readFileSync(join(dirname(fileURLToPath(import.meta.url)), 'crud-page.tsx'), 'utf8');

describe('CRUD pagination GitHub-style controls', () => {
  it('uses a GitHub search inspired pagination landmark and page control styling', () => {
    expect(source).toContain('aria-label="Pagination"');
    expect(source).toContain('mr-1 inline-flex h-8 min-w-8 items-center justify-center rounded-md px-1.5 py-2 text-sm font-normal');
    expect(source).toContain('bg-[#0969da] text-white');
    expect(source).toContain('text-[#0969da] transition hover:bg-raised hover:text-[#0969da]');
    expect(source).toContain('aria-label={`Page ${paginationItem}`}');
    expect(source).toContain('aria-disabled={page.value <= 1 ? \'true\' : undefined}');
    expect(source).toContain('aria-disabled={page.value >= getLastPage(total.value, pageSize.value) ? \'true\' : undefined}');
  });
});
