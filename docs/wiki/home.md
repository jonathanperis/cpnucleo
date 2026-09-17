# Project Overview

Cpnucleo is a .NET project-management learning laboratory. Compare REST and gRPC, EF Core and Dapper, domain behavior, authentication, Astro/native TypeScript, observability and deployment against one inspectable PostgreSQL model.

## Start here

| Path | What to inspect |
|---|---|
| [Getting Started](../getting-started/) | Minimal Docker stack, explicit seed and source development |
| [Learning Lab](../learning-lab/) | Guided exercises, failure cases, measurements and recovery |
| [Architecture](../architecture/) | Project boundaries, persistence comparisons and maturity limits |
| [API Reference](../api-reference/) | Actual REST routes, identity/session behavior and gRPC contracts |
| [WebClient CRUD](../webclient-crud/) | Native forms, relations, concurrency and real-time lists |
| [Database](../database/) | Migrations, durability, lifecycle and data tools |
| [Testing](../testing/) | What each suite proves and how to run it |
| [Deployment](../deployment/) | Lab versus production, release gates and Pages publication |
| [Project Structure](../project-structure/) | Source, tests, labs and documentation map |
| [Technologies](../technologies/) | Stack roles and authoritative version manifests |

## Maturity

- **Baseline:** authenticated CRUD, soft deletion, bounded lists, password hashing, static Astro client and additive schema migrations.
- **Verified examples:** project concurrency and transactional batches, HTTP/gRPC parity, normalized-login contention and cross-instance SSE convergence.
- **Incremental pilot:** shared Application use cases and richer domain behavior.
- **Foundations/experiments:** tenant context without data isolation; Native AOT and Dapper.AOT compatibility; isolated outbox delivery.

The hosts share a database and workspace. This is not a claim of independently owned microservice data, complete CQRS/DDD or comprehensive production coverage. User administration requires an admin claim on both transports.

[Inspect the repository](https://github.com/jonathanperis/cpnucleo) or return to the [documentation index](../).
