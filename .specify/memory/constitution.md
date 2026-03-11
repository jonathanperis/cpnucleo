<!--
  Sync Impact Report
  ==================
  Version change: 0.0.0 → 1.0.0 (MAJOR — initial ratification)
  
  Modified principles: N/A (initial creation)
  
  Added sections:
    - Core Principles (8 principles)
    - Technology & Architectural Constraints
    - Development Workflow & Quality Gates
    - Governance
  
  Removed sections: N/A (initial creation)
  
  Templates requiring updates:
    - .specify/templates/plan-template.md ✅ compatible (Constitution Check
      section already present; gates will be derived from principles below)
    - .specify/templates/spec-template.md ✅ compatible (requirements and
      success criteria sections align with principles)
    - .specify/templates/tasks-template.md ✅ compatible (phase structure
      supports architecture-test gate and foundational phase)
    - .specify/templates/checklist-template.md ✅ compatible (generic
      checklist structure can reference any principle)
  
  Follow-up TODOs: None
-->

# Cpnucleo Constitution

## Core Principles

### I. Clean Architecture Purity (NON-NEGOTIABLE)

The codebase MUST follow Clean Architecture with strict inward-only
dependency flow: Presentation → Infrastructure → Domain.

- The **Domain** layer MUST have ZERO external dependencies. No references
  to EF Core, Dapper, Npgsql, ASP.NET, or any framework package.
- **Infrastructure** MUST NOT depend on any presentation project (WebApi,
  IdentityApi, GrpcServer, WebClient).
- **GrpcServer.Contracts** MUST depend only on Domain.
- **WebApi** and **IdentityApi** MUST NOT depend on GrpcServer.
- All 25+ architecture tests in `test/Architecture.Tests/` MUST pass
  before any code is merged. This gate is enforced in CI and locally.

**Rationale:** Layer isolation ensures the business domain remains portable,
testable, and decoupled from infrastructure decisions. Architecture tests
make violations immediately visible.

### II. Domain-Driven Entity Design

Every domain entity MUST follow the sealed-factory pattern established in
the codebase.

- Entities MUST be **sealed classes** inheriting from `BaseEntity`.
- Construction MUST go through **static factory methods** (`Create`,
  `Update`, `Remove`) — never via public constructors with business logic.
- Entities MUST carry audit fields: `CreatedAt`, `UpdatedAt`, `DeletedAt`.
- Deletion MUST be **soft** (`Active = false`, `DeletedAt` timestamped);
  hard deletes are prohibited at the application level.
- EF Core global query filters MUST enforce `Active == true` on all
  entity queries.

**Rationale:** Sealed classes prevent fragile inheritance hierarchies.
Factory methods centralize invariant enforcement. Soft deletes preserve
audit trails and enable recovery.

### III. Dual Data-Access Strategy (CQRS)

The project implements CQRS through a dual ORM strategy. New features
MUST respect this boundary.

- **Reads** (queries) MUST use EF Core via `IApplicationDbContext`,
  leveraging LINQ, navigation properties, and change tracking.
- **Writes** (commands) MUST use Dapper via `IUnitOfWork` and
  `IRepository<T>` for transactional operations, or the specialized
  `IProjectRepository` pattern for simple cases.
- The Unit of Work MUST manage transaction lifecycle (`Begin`, `Commit`,
  `Rollback`) for all write paths requiring atomicity.
- Pagination MUST use the `PaginationParams` / `PaginatedResult<T>`
  contracts defined in Domain.

**Rationale:** EF Core provides developer productivity for reads; Dapper
delivers superior write performance. Separating concerns makes each path
optimizable independently.

### IV. FastEndpoints REPR Pattern

All HTTP endpoints MUST follow the Request-Endpoint-Response (REPR)
pattern via FastEndpoints. Traditional MVC controllers and ASP.NET
Minimal APIs are prohibited.

- Each endpoint MUST be a separate class named `Endpoint` in its own
  folder: `Endpoints/{Entity}/{Action}/Endpoint.cs`.
- Request/Response models MUST live in `Models.cs` adjacent to the
  endpoint.
- DTOs MUST end with `Dto`; commands MUST end with `Command`; handlers
  MUST end with `Handler`; endpoints MUST be named `Endpoint`.
- OpenAPI documentation MUST be configured in the `Configure()` method
  via `Summary()`.
- Object mapping MUST use Mapperly (compile-time, zero-reflection source
  generation).

**Rationale:** REPR keeps endpoints focused, discoverable, and
independently testable. Naming conventions are machine-enforced by
architecture tests.

### V. Independent Microservice Boundaries

WebApi, GrpcServer, IdentityApi, and WebClient are independent
deployment units that MUST NOT introduce runtime coupling.

- **WebApi** (REST/HTTP) and **GrpcServer** (gRPC/HTTP2) are alternative
  API surfaces — neither depends on nor calls the other.
- Both share Domain and Infrastructure as compile-time library
  references, not runtime service calls.
- Each service MUST expose a `/healthz` endpoint.
- Each service MUST define its own rate-limiting policy (WebApi: 50
  req/min; IdentityApi: 10 req/min per IP).
- New services MUST include a Dockerfile, health checks, and
  `compose.yaml` integration before merge.

**Rationale:** Independent services can be scaled, deployed, and evolved
separately. Health checks enable reliable orchestration and CI
validation.

### VI. Observability by Default

Every service MUST be observable from day one.

- OpenTelemetry MUST be configured for tracing, metrics, and logging on
  all backend services (ASP.NET, HTTP client, process, and runtime
  instrumentation).
- Structured logging via `ILogger<T>` MUST be used throughout; no
  `Console.Write*` in production code.
- Middleware MUST include `ErrorHandlingMiddleware` (centralized error
  responses) and `ElapsedTimeMiddleware` (request duration tracking).
- Application Insights integration MUST remain available for cloud
  deployments.

**Rationale:** Observability is a prerequisite for operating
microservices. Structured logging and distributed tracing enable
diagnosis without reproducing issues locally.

### VII. Architecture-Test-Driven Governance

Architectural decisions MUST be encoded as executable tests, not just
documented conventions.

- New architectural rules MUST be added to
  `test/Architecture.Tests/ArchitectureTests.cs` using NetArchTest.
- The architecture test suite MUST be part of every CI run and MUST pass
  on every PR before merge.
- Rule categories include: layer dependencies, domain purity, naming
  conventions, interface compliance, and sealed-entity enforcement.
- Repository interfaces MUST start with `I` and end with `Repository`.

**Rationale:** Executable architecture tests prevent drift over time and
make compliance objective rather than opinion-based.

### VIII. Production-Ready Containerization

All services MUST be container-ready with multi-stage Docker builds
supporting optimization flags.

- Dockerfiles MUST support build arguments: `AOT`, `TRIM`,
  `EXTRA_OPTIMIZE`, `BUILD_CONFIGURATION`.
- Docker Compose MUST define resource limits (`cpus`, `memory`) and
  health-check conditions for every service.
- The NGINX reverse proxy MUST load-balance WebApi instances using
  `least_conn` strategy.
- CI MUST validate containerized health checks (20 retries × 5 seconds)
  before marking a build green.
- Database initialization MUST use idempotent SQL scripts in
  `docker-entrypoint-initdb.d/`.

**Rationale:** Container-first ensures parity between development and
production. AOT and trimming options enable teams to trade build time
for startup performance as needed.

## Technology & Architectural Constraints

- **Runtime:** .NET 9.0 SDK (pinned via `global.json`, `rollForward:
  latestMajor`). C# 13 with nullable reference types enabled in all
  projects.
- **Database:** PostgreSQL 16.7. Connection pooling configured with
  min/max pool size of 10.
- **API Framework:** FastEndpoints 7.0. No MVC controllers or Minimal
  API `MapGet`/`MapPost` for business endpoints.
- **ORM:** EF Core 9 (reads) + Dapper 2.1 with Dapper.AOT (writes).
- **Frontend:** Blazor WebAssembly with MudBlazor components and
  interactive server/WASM render modes.
- **Authentication:** JWT via FastEndpoints auth; signing keys MUST NOT
  be committed — use environment variables or secret managers.
- **Serialization:** Source-generated JSON serialization preferred;
  reflection-based serialization MUST be avoided where AOT is targeted.
- **Mapping:** Mapperly source generator only. No AutoMapper or manual
  mapping code.
- **Expected warnings:** 14 nullable-reference warnings in
  `Infrastructure/Common/Helpers/FakeData.cs` (CS8601, CS8618) are
  intentional. New warnings MUST NOT be introduced elsewhere.

## Development Workflow & Quality Gates

Every change MUST pass the following gates before merge:

1. `dotnet restore` (from repository root)
2. `dotnet build` (affected project or full solution; Debug config for
   PRs, Release for main)
3. `cd test/Architecture.Tests && dotnet test` — all 25+ tests MUST
   pass
4. `cd test/WebApi.Unit.Tests && dotnet test` — unit tests MUST pass
5. Docker health-check validation via
   `curl http://localhost:9999/healthz` returning HTTP 200 (when service
   changes are involved)

**Database migrations** follow this workflow:
1. Add migration: `dotnet ef migrations add <Name> -p ./src/Infrastructure -s ./src/WebApi -c 'ApplicationDbContext'`
2. Generate idempotent SQL: `dotnet ef migrations script --output ./docker-entrypoint-initdb.d/001-database-dump-ddl.sql --idempotent -p ./src/Infrastructure -s ./src/WebApi -c 'ApplicationDbContext'`
3. SQL scripts MUST be committed alongside the migration.

**CI/CD pipelines** (GitHub Actions):
- PR validation: build + architecture tests + container health checks
- Main branch: Release build + Docker image push + Azure deployment
- Multi-platform images: `linux/amd64` and `linux/arm64/v8`

## Governance

This constitution is the authoritative source for architectural and
process decisions in Cpnucleo. It supersedes informal conventions,
comments, and undocumented practices.

- **Amendment process:** Any change to principles or constraints MUST be
  proposed, reviewed, and merged via pull request. The Sync Impact Report
  at the top of this file MUST be updated to reflect the change.
- **Versioning:** Constitution versions follow Semantic Versioning:
  - MAJOR: Principle removal, redefinition, or backward-incompatible
    governance change.
  - MINOR: New principle added, existing guidance materially expanded.
  - PATCH: Clarifications, typo fixes, non-semantic wording refinements.
- **Compliance review:** Architecture tests encode the machine-verifiable
  subset of these principles. For non-automatable principles (e.g.,
  observability configuration), PR reviewers MUST verify compliance
  before approving.
- **Complexity justification:** Any deviation from these principles MUST
  be documented in the PR description with a rationale and the simpler
  alternative that was rejected.
- **Runtime guidance:** Refer to `.github/copilot-instructions.md` for
  operational development guidance, common tasks, and troubleshooting.

**Version**: 1.0.0 | **Ratified**: 2026-03-11 | **Last Amended**: 2026-03-11
