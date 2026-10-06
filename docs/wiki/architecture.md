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

Normal removal is soft deletion. PostgreSQL triggers keep soft-deleted relationships consistent for every writer: an active row can only reference active parents, a parent with active dependent data can't be removed (HTTP 409 / gRPC FailedPrecondition), and membership links (`UserProjects`, `UserAssignments`) are removed with their parent. Every batch removal is atomic, deduplicated and capped at 100 ids. Updates never write `Id`, `CreatedAt`, `Active` or `DeletedAt`, so a stale update can't bring back a row removed in the meantime. Version-aware project updates compare the observed timestamp in SQL and reject stale writes; older unversioned clients retain last-write-wins behavior.

Entities remain a pragmatic CRUD-oriented model, but every factory and update now enforces the shared invariants (required names and references, positive order and hours, date ordering, bounded lengths, UTC timestamps), with private setters and idempotent removal. Both transports therefore reject the same input regardless of which persistence style serves it. A factory method alone is not evidence of a complete DDD aggregate design.

## Security

IdentityApi issues subject-bearing JWTs (HS256 by default, RS256 when a PEM key pair is configured so API hosts only hold the public key) and uses Argon2id password hashes. Login spends the same hashing work for unknown accounts, locks a login after repeated failures and caps concurrent verifications. WebApi/gRPC validate algorithm, signature, issuer, audience and expiry, then confirm the session: the account must be active, its credentials unchanged since the token was issued (security stamp) and any admin claim still configured. Refresh applies the same checks and retains the original eight-hour session limit.

Authorization is one table (`Application/Common/Security/ResourceAccess`) enforced identically by both transports and both persistence styles: catalog data (organizations, workflows, assignment types, impediments) is readable by everyone and writable by administrators; projects and their assignments, appointments, impediment links and memberships are visible and writable to project members only; members record only their own hours; creating a project makes the creator a member; user administration requires an administrator. Dapper repositories filter reads and authorize writes, EF Core writes go through a save interceptor.

New or changed active logins are checked transactionally using normalized login keys in PostgreSQL. Legacy ambiguous accounts are preserved but cannot authenticate ambiguously. Project membership is not tenant isolation: tenant context types and informational claims still do not partition data.

## Browser and real-time behavior

Astro emits static HTML; native TypeScript controls forms, Fetch, pagination, relations, dialogs, session storage and themes. API data is inserted as text. The frontend framework migration preserves route paths and API contracts while removing the extra rendering runtime and integration patch.

SSE sends an initial snapshot, responds immediately to local notifications, and refreshes every 15 seconds for writes from another process or gRPC. This is bounded convergence, not a durable event bus. Streams end when their access token expires; the client reconnects with its current session.

## Delivery and learning extensions

Production runs behind Traefik and an internal NGINX load balancer. A one-shot migration service precedes API startup. Liveness and database readiness are distinct. Release checks use immutable image tags; secrets remain in deployment configuration.

Read [Learning Lab](../learning-lab/) for persistence benchmarks, concurrency/rollback proofs, recovery and a disposable outbox experiment. See `docs/adr/0001-learning-baseline.md` for the rationale and explicit limitations.
