# Architecture

## Purpose and boundaries

This is a learning laboratory with a shared PostgreSQL model and alternative implementations. Domain and Application define behavior/ports; Infrastructure implements persistence and hashing; transport hosts compose them. Architecture tests check each forbidden project dependency using explicitly loaded assemblies.

```text
Astro + native TypeScript ──HTTP── WebApi       IdentityApi
                                REST           JWT/Argon2id
gRPC clients ────────────────── GrpcServer
                                   │
                        Application use-case pilot
                                   │
                   Infrastructure: EF Core / Dapper
                                   │
                         Domain ports/entities
                                   │
                              PostgreSQL
```

The diagram shows the request path, not dependency direction: Domain does not depend on Infrastructure or PostgreSQL. WebApi and GrpcServer are independent projects; they share Domain, Application and Infrastructure contracts instead of referencing one another.

## Persistence comparisons

REST intentionally demonstrates three approaches: EF Core, an explicit project repository, and a generic Dapper repository with Unit of Work. gRPC uses Dapper. Project creation is the shared Application-layer pilot, so a REST/gRPC comparison does not require duplicating its business rules.

Normal removal is soft deletion. Relationships remain stored; deleting a parent does not implicitly archive every child. Project batches use one transaction. Version-aware project updates compare the observed timestamp in SQL and reject stale writes; older unversioned clients retain last-write-wins behavior.

Entities remain a pragmatic CRUD-oriented model. Selected factories/updates enforce real invariants, including assignment date ordering and positive hours. A factory method alone is not evidence of a complete DDD aggregate design.

## Security

IdentityApi issues subject-bearing JWTs and uses Argon2id password hashes. WebApi/gRPC validate signature, issuer, audience and expiry. User administration requires explicit admin privileges. Refresh checks the active account, recalculates privileges and retains the original eight-hour session limit.

New or changed active logins are checked transactionally using normalized login keys in PostgreSQL. Legacy ambiguous accounts are preserved but cannot authenticate ambiguously. The baseline uses a shared workspace; tenant context types and informational claims do not establish data isolation.

## Browser and real-time behavior

Astro emits static HTML; native TypeScript controls forms, Fetch, pagination, relations, dialogs, session storage and themes. API data is inserted as text. The frontend framework migration preserves route paths and API contracts while removing the extra rendering runtime and integration patch.

SSE sends an initial snapshot, responds immediately to local notifications, and refreshes every 15 seconds for writes from another process or gRPC. This is bounded convergence, not a durable event bus. Streams end when their access token expires; the client reconnects with its current session.

## Delivery and learning extensions

Production runs behind Traefik and an internal NGINX load balancer. A one-shot migration service precedes API startup. Liveness and database readiness are distinct. Release checks use immutable image tags; secrets remain in deployment configuration.

Read [Learning Lab](../learning-lab/) for persistence benchmarks, concurrency/rollback proofs, recovery and a disposable outbox experiment. See `docs/adr/0001-learning-baseline.md` for the rationale and explicit limitations.
