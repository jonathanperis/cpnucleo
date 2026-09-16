# ADR 0001: A dependable learning baseline

Status: Accepted, 2026-09-16.

## Context

The owner explicitly defines Cpnucleo as a broad learning sandbox and requested the audit improvements, deployment, and removal of the additional client framework where Astro can preserve the features.

## Decisions

- Keep REST/gRPC and EF/Dapper as comparison surfaces. Share business invariants and contract proofs rather than forcing every implementation into one generic abstraction.
- Use Astro templates plus native TypeScript. The UI needs forms, Fetch, storage, dialogs and SSE; a second rendering runtime and build patch add no necessary capability.
- Preserve production data. Apply additive migrations in a one-shot service. Seed/reset only through explicit commands against disposable development databases.
- Keep existing ambiguous logins unchanged, reject ambiguous authentication, and serialize new normalized-login registrations in PostgreSQL. The application's write paths use Read Committed isolation; legacy account cleanup is an explicit administrative decision.
- Use process-local SSE notifications plus periodic snapshots to provide bounded cross-instance convergence without introducing a message broker.
- Demonstrate optimistic concurrency on projects with an optional observed timestamp. The native client uses it; older clients retain their existing last-write-wins contract.
- Keep PostgreSQL integration tests disposable and independent. Pair structural architecture checks with observable HTTP/gRPC/database behavior.

## Consequences

The baseline is simpler to run and easier to verify. It is still a shared workspace, not a tenant-isolated SaaS. Snapshot polling costs database reads; richer messaging remains an explicit exercise. Native DOM controllers require interaction tests, lifecycle cleanup and safe text rendering. Readiness proves database/schema reachability, not every business workflow.
