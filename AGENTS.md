# Cpnucleo — Agent Guide

Production-grade .NET 10 project management system demonstrating Clean Architecture, DDD, CQRS, and dual data access strategies.

**Live demo:** https://cpnucleo.jonathanperis.tech/

---

## Git Workflow

- **All changes require a branch + PR strategy** — never push directly to `main`
- Branch naming: `feat/`, `fix/`, `docs/`, `chore/`, `refactor/`, `test/`
- PRs target `main`; CI must pass before merge
- **Merge strategy: rebase only** — squash and merge commits are disabled; use `gh pr merge --rebase`
- **Always use `gh` CLI** for GitHub operations (PRs, issues, checks, releases)
- **Always sync main before creating branches and opening PRs** — run `git fetch origin main && git checkout main && git pull origin main` before branching; verify main is up to date before `gh pr create`
- Architecture tests (`tests/Architecture.Tests/`) **MUST** pass before committing

---

## Tech Stack

| Technology | Purpose |
|-----------|---------|
| .NET 10 / C# | Runtime and language |
| FastEndpoints 8.1 | REST endpoints + gRPC handlers |
| Entity Framework Core 10 | ORM for REST API (WebApi) + IdentityApi |
| Dapper 2.1 + Dapper.AOT | Micro-ORM for gRPC (GrpcServer) |
| PostgreSQL 16.7 | Primary database |
| Npgsql 10 | PostgreSQL driver with multiplexing |
| Riok.Mapperly 4.3 | Compile-time DTO mapping (zero reflection) |
| MudBlazor 8.x | Blazor UI components (Material Design) |
| OpenTelemetry 1.15 | Observability (OTLP export) |
| Docker + NGINX | Containerization + load balancing |
| GitHub Actions | CI/CD with Hostinger deployment |
| Kiota | Auto-generated API client for WebClient |

---

## Build Commands

```sh
dotnet build cpnucleo.slnx                    # Build entire solution
dotnet test cpnucleo.slnx                     # Run all tests
dotnet test tests/Architecture.Tests/           # Architecture tests only (MANDATORY)
dotnet test tests/WebApi.Unit.Tests/            # Unit tests only
docker compose -f compose.yaml -f compose.override.yaml up --build  # Dev mode
docker compose -f compose.yaml -f compose.prod.yaml up -d           # Prod mode
```

### EF Core Migrations

Always use `-s ./src/WebApi` as startup project:

```sh
dotnet ef migrations add <Name> -p ./src/Infrastructure -s ./src/WebApi -c ApplicationDbContext
```

---

## Architecture

```
Presentation Layer
├── WebApi (REST, FastEndpoints + EF Core, port 5000)
├── GrpcServer (gRPC, FastEndpoints.Messaging + Dapper, port 5020/5021)
├── IdentityApi (JWT auth, port 5010)
└── WebClient (Blazor Server + WASM, port 5030)

├── Application Layer
│   └── Feature slices/use cases (pilot: Projects/CreateProject)
│
├── Infrastructure Layer
├── EF Core (ApplicationDbContext) — used by WebApi + IdentityApi
├── Dapper (DapperRepository<T> + UnitOfWork) — used by GrpcServer
└── PostgreSQL 16.7 with Npgsql multiplexing

Domain Layer (zero external dependencies)
├── 11 Entities (Organization, Project, Assignment, User, etc.)
├── Repository interfaces (IRepository<T>, IProjectRepository)
└── IPasswordHasher abstraction (Argon2id implementation in Infrastructure)
```

**Clean Architecture enforced by 27 NetArchTest rules** — Domain depends on nothing; Application contains shared feature slices; Infrastructure provides EF Core + Dapper implementations behind Domain/Application ports.

**Key dependency rule:** WebApi and GrpcServer are **independent** — neither references the other. Both depend on Domain and Infrastructure.

---

## Key Conventions

| Element | Pattern |
|---------|---------|
| Entities | Sealed classes inheriting `BaseEntity` with factory methods (Create, Update, Remove) |
| REST endpoints | Class **must** be named `Endpoint` with nested `Request`/`Response` |
| gRPC handlers | Class named `*Handler`, commands named `*Command`, results named `*Result` |
| DTOs | Named `*Dto` (in `GrpcServer.Contracts/Common/Dtos/`) |
| Soft delete | `Active` flag + `DeletedAt` timestamp (never hard-delete) |
| IDs | UUID v7 via `BaseEntity.GetNewId()` using `Guid.CreateVersion7()` |
| Mapping | Riok.Mapperly partial classes — compile-time, no reflection |
| Repository interfaces | Prefixed with `I` (e.g., `IProjectRepository`) |

---

## Project Structure

```
cpnucleo/
├── cpnucleo.slnx                    # Solution file
├── global.json                      # .NET 10.0.102
├── compose.yaml / override / prod   # Docker Compose configs
├── nginx.conf                       # NGINX reverse proxy (least_conn)
├── docker-entrypoint-initdb.d/      # PostgreSQL init scripts
├── src/
│   ├── Domain/                      # Business entities, interfaces, no deps
│   ├── Application/                 # Feature slices/use cases shared by REST + gRPC
│   ├── Infrastructure/              # EF Core + Dapper implementations
│   ├── WebApi/                      # 55 REST endpoints (11 entities x 5 ops)
│   ├── GrpcServer/                  # 55 gRPC handlers
│   ├── GrpcServer.Contracts/        # Shared command/result DTOs
│   ├── IdentityApi/                 # JWT login (rate limited 10/min)
│   ├── WebClient/                   # Blazor UI with MudBlazor
│   └── WebApi.Client/               # Auto-generated Kiota client
├── tests/
│   ├── Architecture.Tests/          # 25+ Clean Architecture rules (xUnit + NetArchTest)
│   ├── WebApi.Unit.Tests/           # Endpoint unit tests (NUnit + FakeItEasy + Shouldly)
│   └── WebApi.Integration.Tests/    # E2E tests (xUnit v3 + FastEndpoints.Testing)
├── docs/wiki/                       # Source markdown for GitHub Pages documentation
└── docs/                            # Astro GitHub Pages site
```

---

## Testing

| Suite | Framework | Tests | Purpose |
|-------|-----------|-------|---------|
| Architecture.Tests | xUnit + NetArchTest + FluentAssertions | 27 | Layer deps, naming, sealed entities |
| Application.Unit.Tests | NUnit + FakeItEasy + Shouldly | 4 | Shared Application use cases |
| WebApi.Unit.Tests | NUnit + FakeItEasy + Shouldly | 49 | Endpoint happy/negative paths |
| WebApi.Integration.Tests | xUnit v3 + FastEndpoints.Testing | 50+ | Full HTTP CRUD per entity |

**CI note:** Unit and integration tests are currently commented out in `build-check.yml` — only architecture tests run in CI.

### Build Warnings

Exactly 14 expected warnings from `src/Infrastructure/Common/Helpers/FakeData.cs` (Bogus usage). More than 14 = investigate.

---

## Docker Services

| Service | Port | CPU | RAM |
|---------|------|-----|-----|
| WebApi (x2) | 5100/5111 | 0.4 | 100MB |
| IdentityApi | 5200 | — | — |
| GrpcServer | 5300 (gRPC) / 5301 (health) | — | — |
| WebClient | 5400 | — | — |
| NGINX | 9999 | — | — |
| PostgreSQL | 5432 | — | — |
| Grafana LGTM (dev) | 3000 | — | — |

---

## CI/CD

- **PR (`build-check.yml`):** Build + Architecture Tests + Code Coverage (Codecov) + Container health check
- **Main (`main-release.yml`):** release pipeline builds/tests images, deploys Hostinger, and publishes multi-arch manifests
- **Security (`codeql.yml`):** CodeQL analysis on push/PR/weekly schedule
- **Docs (`deploy.yml`):** Deploys the Astro docs site to GitHub Pages on main
- **Registry:** `ghcr.io/jonathanperis/cpnucleo-{web-api|grpc-server|identity-api|web-client}`

### Main Release Pipeline Flow

```
setup-build-test (x4)
        │
        ▼
build-push-amd64 (x4) ──────────────────────────────┐
        │                                            │
        ├──→ container-test (x4) ─────────┐          ├──→ build-push-arm64 (x4)
        │                                 │          │
        ├──→ deploy-hostinger ───────────┤          ▼
        │                                 ▼      merge-manifest (x4)
```

| Job | Description |
|-----|-------------|
| `setup-build-test` (x4) | Restore + build Release + Architecture Tests |
| `build-push-amd64` (x4) | Build linux/amd64, push as `:latest` and `:sha-<commit>-amd64` |
| `container-test` (x4) | Docker Compose health check per service |
| `deploy-hostinger` | Deploy immutable amd64 GHCR images to Hostinger Docker Manager |
| `build-push-arm64` (x4) | Build linux/arm64/v8, push as `:latest-arm64` and `:sha-<commit>-arm64` |
| `merge-manifest` (x4) | Merge amd64 + arm64 into multi-arch `:latest` and `:sha-<commit>` manifests |

### Hostinger Deployment

Production deploys through Hostinger Docker Manager using `scripts/deploy-hostinger-docker-manager.sh`. The workflow requires Hostinger project secrets, immutable amd64 GHCR image tags, and public route smoke-test URLs for WebClient, WebApi, IdentityApi, and gRPC health.

---

## Rate Limiting

- WebApi: 50 req/min per IP (queue: 10)
- IdentityApi: 10 req/min per IP (queue: 5)

---

## Database

- PostgreSQL 16.7 with commit timestamps enabled
- Connection: `Minimum Pool Size=10; Maximum Pool Size=10; Multiplexing=true`
- Dev mode disables durability (fsync=0, synchronous_commit=0) for speed
- Schema init: `docker-entrypoint-initdb.d/` (DDL + CSV seed data)

---

## Observability

- OpenTelemetry tracing, metrics, and logging via OTLP export
- Dev: Grafana LGTM container (port 3000 UI, 4317 gRPC, 4318 HTTP)
- Service names: `WebApi-Cpnucleo`, `Identity-Cpnucleo`, `GrpcServer-Cpnucleo`, `WebClient-Cpnucleo`
- Instrumentation: ASP.NET Core, HttpClient, Process, Runtime
- **OpenTelemetry:** services export traces, metrics, and logs through OTLP; Hostinger production points `OTEL_EXPORTER_OTLP_ENDPOINT` at the local `otel-collector`, which forwards to Grafana LGTM.

---

## Known Architecture Notes

- All WebApi endpoints currently use `AllowAnonymous()` — auth is configured but not enforced
- JWT validation code exists in WebApi but is commented out (reference implementation mode)
- IdentityApi verifies Argon2id password hashes through the Domain-owned `IPasswordHasher` abstraction
- WebApi and GrpcServer have parallel CRUD logic — future improvement: extract shared application services
