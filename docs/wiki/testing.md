# Testing

## What each suite proves

| Suite | Scope |
|---|---|
| Architecture.Tests | Explicitly loaded production assemblies, forbidden dependencies, naming and selected source/configuration contracts |
| Application.Unit.Tests | Shared project creation, pagination boundaries and domain invariants |
| Security.Unit.Tests | Argon2 hashing, successful token issuance, password rejection, refresh lifetime/account/privilege checks |
| WebApi.Unit.Tests | Endpoint orchestration and selected failure paths |
| WebApi.Integration.Tests | Authenticated HTTP CRUD for every resource, serialized gRPC parity, PostgreSQL transactions, login contention, project concurrency and SSE external writes |

Run `dotnet test cpnucleo.slnx`. Docker must be available for the integration suite. It provisions its own PostgreSQL container with commit timestamps enabled and applies real migrations. It neither reads nor mutates your application database.

The solution uses the VSTest `dotnet test` experience across NUnit and xUnit projects. Integration tests select `xunit.v3.mtp-off` with the Visual Studio adapter: this is the supported xUnit package variant for keeping VSTest when upgrading the xUnit v3 package family. Enabling Microsoft.Testing.Platform requires a coordinated runner/workflow migration.

The CRUD theory performs all five operations in an independent scenario for each resource. It replaces the old order-dependent suite whose later tests relied on previous classes creating records. Test totals are deliberately reported by the runner rather than frozen in this page.

## Focused commands

```sh
dotnet test tests/Architecture.Tests/
dotnet test tests/Security.Unit.Tests/
dotnet test tests/WebApi.Integration.Tests/ --filter FullyQualifiedName~PersistenceParityTests
dotnet test tests/WebApi.Integration.Tests/ --filter FullyQualifiedName~ConcurrencyAndStreamingTests
```

The SSE scenario may wait one 15-second refresh interval. The gRPC client uses an in-process HTTP/2 test handler, exercising serialization, the gRPC pipeline and authentication without opening a public listener.

## WebClient Vitest suite

From `src/WebClient`:

```sh
bun install --frozen-lockfile
bun run typecheck
bun run test
bun audit
```

`bun run test` builds Astro, then runs Vitest. jsdom needs a real Node runtime: use the version in `.nvmrc` and make sure `node` on `PATH` is not a Bun shim (some Bun installs link `node` to Bun, which breaks jsdom). For example, put an official Node build first on `PATH` for that command only:

```sh
PATH="/path/to/node-v26/bin:$PATH" bun run test
```

What the suite covers:

- **Session and HTTP:** the unified error envelope (field errors and `generalErrors` preferred over the summary), 403 without sign-out, 409 messages, 429 with `Retry-After`, 401 handling for JSON requests, live streams and refreshes (including a refresh rejected after a password change), background calls and refreshes not counting as activity, the inactivity timer re-checking activity, and cross-tab logout through `BroadcastChannel` and the `storage` fallback.
- **Lists:** the SSE parser (chunk splits, CRLF, comments, multi-line data), stream-first snapshots without an extra JSON request, flat parameters with a stable sort, reconnect backoff that resets only after a live snapshot, the cap, jitter and `Retry-After`.
- **CRUD pages (jsdom against generated HTML):** field-error marking and clearing, task date ordering, required workflow order, relation labels outside the loaded page, explicit clears, per-relation abort and "More" query consistency, admin/non-admin gating (including no `/api/users` request for non-admins), distinct row action names, focus return, status versus alert semantics, safe text rendering, stale-page cancellation and home counters.
- **Server scripts:** CSP manifest generation from build-time URLs and the built HTML, header generation, manifest validation, and the static server destroying a response when a read fails after headers were sent.
- **Astro-only pages:** `src/guarantees/astro-pages.test.ts` fails if either site gains a non-`.astro` page, a client UI framework dependency/integration or an HTML file in `public/`, or if any built WebClient HTML lacks Astro's generator marker. `scripts/check-docs-drift.py` (with `--built-site` for `docs/out`) and `tests/Architecture.Tests/AstroPagesTests.cs` enforce the same rule.

These are DOM unit/contract tests, not a real-browser accessibility or rendering certification. Treat jsdom evidence as jsdom evidence; check behavior that depends on layout, CSP enforcement or assistive technology in a real browser.

## CI and interpretation

PR and release workflows run backend behavioral suites and frontend tests. PRs also build/audit documentation and validate generated links/assets with `python3 scripts/check-docs-drift.py --built-site`. The checker regression fixture runs with `python3 scripts/test_docs_drift.py`. Release container checks run the exact immutable amd64 and arm64 images on native runners of each architecture, including database readiness. PRs also build the release Dockerfile configuration (`TRIM=true`, `EXTRA_OPTIMIZE=true`) without pushing. CodeQL analyzes C#, JavaScript/TypeScript and GitHub Actions.

Architecture rules explicitly reference their target assemblies: an unloaded assembly no longer produces a successful no-op. Source checks verify configuration structure but do not replace runtime proofs. PR coverage is collected once from the single solution test run, merged with ReportGenerator and summarized in the job summary; test projects without `coverlet.collector` are listed as missing rather than silently counted. Line coverage is a signal, not proof of behavioral completeness.

User/workflow creation is covered by the PostgreSQL CRUD theory. The two never-executed `DbSet.Any()` mock tests were retired in favor of those real database proofs; the baseline has no skipped test placeholders.

Use [Learning Lab](../learning-lab/) to intentionally break a transaction, concurrency check, domain rule or dependency boundary, observe a failing test, and restore the implementation.
