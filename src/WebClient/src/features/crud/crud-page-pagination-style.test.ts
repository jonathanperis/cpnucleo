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
    expect(source).toContain('aria-disabled={isFirstPage ? \'true\' : undefined}');
    expect(source).toContain('aria-disabled={isLastPage ? \'true\' : undefined}');
  });

  it('uses one derived display page for the summary and active controls', () => {
    expect(source).toContain('const displayPage = Math.min(Math.max(1, page.value), lastPage);');
    expect(source).toContain('records · page {displayPage} of {lastPage}');
    expect(source).toContain('data-current-page={displayPage}');
    expect(source).toContain('buildPaginationItems(displayPage, lastPage)');
    expect(source).toContain('displayPage === paginationItem ? \'bg-[#0969da] text-white\'');
    expect(source).toContain("key={`${paginationItem}-${displayPage === paginationItem ? 'active' : 'idle'}`}");
  });

  it('ignores stale listing responses after the selected page changes', () => {
    expect(source).toContain('const requestedPage = page.value;');
    expect(source).toContain('const requestedPageSize = pageSize.value;');
    expect(source).toContain('webApiClient.subscribeList(resource.key, requestedPage, requestedPageSize');
    expect(source).toContain('if (controller.signal.aborted || page.value !== requestedPage || pageSize.value !== requestedPageSize) return;');
  });
});
