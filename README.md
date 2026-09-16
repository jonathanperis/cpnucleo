# cpnucleo

> A hands-on .NET 10 architecture laboratory: compare implementations, explore tradeoffs, and verify the behavior you learn.

[![Build Check](https://github.com/jonathanperis/cpnucleo/actions/workflows/build-check.yml/badge.svg)](https://github.com/jonathanperis/cpnucleo/actions/workflows/build-check.yml) [![Main Release](https://github.com/jonathanperis/cpnucleo/actions/workflows/main-release.yml/badge.svg)](https://github.com/jonathanperis/cpnucleo/actions/workflows/main-release.yml) [![CodeQL](https://github.com/jonathanperis/cpnucleo/actions/workflows/codeql.yml/badge.svg)](https://github.com/jonathanperis/cpnucleo/actions/workflows/codeql.yml)

**[Learning paths](https://jonathanperis.github.io/cpnucleo/docs/learning-lab/)** · **[Pages documentation](https://jonathanperis.github.io/cpnucleo/docs/)** · **[Live demo](https://cpnucleo.jonathanperis.tech/)**

## Purpose

Cpnucleo is a project-management sandbox for learning application and platform engineering. Its breadth is intentional: REST and gRPC, EF Core and Dapper, domain modeling, authentication, real-time updates, observability, containers, and deployment share one inspectable example.

The goal is a reliable learning baseline with explicit experiments, rather than a claim that every demonstrated technique is a complete production solution.

## Start locally

Prerequisites: Docker with Compose v2. For source development and tests, also install .NET 10 SDK and Node.js 22.14+ with Bun 1.3.11+.

```sh
git clone https://github.com/jonathanperis/cpnucleo.git
cd cpnucleo
docker compose -f compose.lab.yaml up --build -d
docker compose -f compose.lab.yaml run --rm seed
```

Open **http://localhost:5400**. The disposable lab account is `demo@cpnucleo.local` with password `LocalLearning@123`. These are local example credentials, not a production account. The login form never prefills credentials.

| Local service | Address |
|---|---|
| WebClient | http://localhost:5400 |
| WebApi | http://localhost:5100 |
| IdentityApi | http://localhost:5200 |
| PostgreSQL | localhost:15432 |
| gRPC (`full` profile) | localhost:5300; health/readiness on 5301 |
| Grafana (`observability` profile) | http://localhost:3000 |

Add optional services:

```sh
docker compose -f compose.lab.yaml --profile full --profile observability up --build -d
```

Seeding is explicit. `tiny` creates 3 projects and 30 tasks; `realistic` creates 50 projects and 500 tasks. To **replace only your disposable lab data**:

```sh
docker compose -f compose.lab.yaml run --rm seed --reset-lab --Seed:Profile=realistic
```

The existing million-row CSV importer remains an advanced, explicit load-test tool. Production deployment never invokes it automatically.

## Implementation map

| Surface | Implementation |
|---|---|
| REST | FastEndpoints; EF Core, explicit Dapper and generic Dapper/UoW examples |
| gRPC | FastEndpoints Remote Messaging, Dapper, shared contracts |
| Shared use case | `Application/Features/Projects/CreateProject` |
| Domain | Entities, factory/update behavior, repository ports and password-hasher abstraction |
| Identity | Argon2id, subject-bearing JWTs, active-account refresh with an eight-hour session boundary |
| UI | Astro static routes, native TypeScript controllers, Tailwind CSS |
| Database | PostgreSQL; EF migrations shared by both persistence strategies |
| Delivery | GitHub Actions, GHCR immutable tags, Hostinger Docker Manager |
| Observability | OpenTelemetry traces, metrics and logs; optional local Grafana LGTM |

Both transports expose 55 CRUD operations across 11 resources. Normal removal is soft deletion. Project batch removal is transactional; version-aware project updates reject stale writes. List pages are bounded to 100 rows and support search and batched relation lookups.

The Astro UI preserves CRUD forms, pagination, relation labels/search, native details dialogs, counters, login redirects, inactivity expiry, token refresh, themes, and service checks. Server-sent events combine immediate local notifications with a 15-second cross-instance refresh and client reconnection.

## Maturity and boundaries

| Status | Capability |
|---|---|
| Working baseline | Authenticated CRUD, soft deletion, PostgreSQL-backed tests, native Astro client, immutable deployments |
| Verified examples | Project concurrency conflicts, transactional batches, REST/gRPC parity, duplicate-login contention, externally written SSE updates |
| Incremental pilot | Shared Application use cases and richer domain behavior |
| Foundation/exercise | Tenant isolation: tenant context types and informational claims exist, but shared workspace records are not tenant-isolated |
| Experiment | Native AOT and Dapper.AOT; installation/build flags alone do not prove compatibility |

User administration requires an administrator on both transports. Configure `CPNUCLEO_ADMIN_LOGINS` explicitly. Other authenticated operations demonstrate a shared learning workspace, not ownership-based authorization for a multi-tenant SaaS.

## Verify

Five test projects cover architecture, application behavior, security, endpoint units, and isolated PostgreSQL-backed integration scenarios:

```sh
dotnet test cpnucleo.slnx
dotnet test tests/Architecture.Tests/
```

Integration tests create and dispose their own PostgreSQL container; they do not use your configured application database. Test counts come from the runner, not hardcoded documentation.

From `src/WebClient`:

```sh
bun install --frozen-lockfile
bun run typecheck
bun run test
bun audit
```

The frontend test command builds the static pages first, then tests native DOM interactions against that generated markup. From `docs`, run `bun install --frozen-lockfile && bun run build`. Check documentation contracts with `python3 scripts/check-docs-drift.py`.

## Deployment

Production is a **standalone** Compose configuration:

```sh
docker compose --env-file .env -f compose.prod.yaml up -d
```

Use `.env.hostinger.example` to configure production secrets, hosts and immutable image tags. Do not layer the development/base file into production: Compose preserves published ports during merging.

The release pipeline tests the exact amd64 image tags, applies additive database migrations before API startup, deploys through Hostinger, and verifies liveness and database readiness. `/healthz` checks the process; `/readyz` also checks database/schema availability. The legacy `TRIM` flag currently configures ReadyToRun/self-contained publishing, not IL trimming. Native AOT is disabled in the standard release.

## Repository layout

```text
src/Domain                 Domain behavior and ports
src/Application            Shared use-case pilot
src/Infrastructure         EF Core, Dapper, migrations, hashing and seed tools
src/WebApi                 REST
src/GrpcServer              gRPC handlers
src/GrpcServer.Contracts    Remote commands/results
src/IdentityApi             Authentication and refresh
src/WebClient              Astro and native TypeScript
tests/                     Five .NET suites; frontend tests live with source
labs/                      Reproducible learning experiments
docs/wiki/                 Published learning and technical documentation
```

## Contributing and license

Use a branch and PR; merge through rebase after CI passes. Community guidelines are maintained in [jonathanperis/.github](https://github.com/jonathanperis/.github). Licensed under [MIT](LICENSE).
