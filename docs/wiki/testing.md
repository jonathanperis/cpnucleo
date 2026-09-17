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

`bun run test` builds Astro, then runs API/session utility tests and jsdom interaction tests against the generated HTML. The tests exercise native form events, password requirements, safe text rendering, stale-page cancellation, relation search/paging and all home counters. They are DOM unit/contract tests, not a real-browser accessibility or rendering certification.

## CI and interpretation

PR and release workflows run backend behavioral suites and frontend tests. PRs also build/audit documentation and validate generated links/assets with `python3 scripts/check-docs-drift.py --built-site`. The checker regression fixture runs with `python3 scripts/test_docs_drift.py`. Release container checks use exact immutable amd64 images and database readiness. CodeQL analyzes C#, JavaScript/TypeScript and GitHub Actions.

Architecture rules explicitly reference their target assemblies: an unloaded assembly no longer produces a successful no-op. Source checks verify configuration structure but do not replace runtime proofs. The architecture-only Codecov upload should not be interpreted as whole-application behavioral coverage.

User/workflow creation is covered by the PostgreSQL CRUD theory. The two never-executed `DbSet.Any()` mock tests were retired in favor of those real database proofs; the baseline has no skipped test placeholders.

Use [Learning Lab](../learning-lab/) to intentionally break a transaction, concurrency check, domain rule or dependency boundary, observe a failing test, and restore the implementation.
