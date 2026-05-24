# Multitenancy Wide Implementation Plan

> **For Hermes:** Use subagent-driven-development skill to implement this plan task-by-task.

**Goal:** Convert Cpnucleo into a working multi-tenant application where each request operates inside an authenticated tenant boundary and tenant data cannot leak across REST, gRPC, IdentityApi, WebClient, EF Core, Dapper, seed data, tests, telemetry, or deployment.

**Architecture:** Use a shared PostgreSQL database with a `Tenants` table plus required `TenantId` discriminator columns on tenant-owned entities. Resolve tenant context at the edge, issue JWTs containing `tenant_id` and `tenant_slug`, and enforce tenant isolation in both EF Core global query filters and Dapper repository predicates. Keep `Organization` as a business entity inside a tenant; do not overload it as the SaaS tenant.

**Tech Stack:** .NET 10, FastEndpoints 8.1, EF Core 10, Dapper/Dapper.AOT, PostgreSQL 16.7, JWT bearer auth, Astro/Qwik WebClient, OpenTelemetry, xUnit/NUnit architecture/unit/integration tests.

---

## Planning Snapshot

| Area | State |
| --- | --- |
| Repo inspected | `/opt/data/github/jonathanperis/cpnucleo` |
| Branch/ref inspected | `docs/multitenancy-implementation-plan` at `6dc71bae99bc95c51a5f79eab15aefb56596dc23` |
| Working tree before plan | Clean; plan branch created from up-to-date `origin/main` |
| Prior plans consulted | None found under `docs/plans`; existing docs live in `docs/wiki/` and `docs/IMPECCABLE_OVERHAUL_PLAN.md` |
| Implementation status | Not started; this document is planning only |
| Validation status | Pending; planned gates are listed below |

### Evidence Checked

| Evidence | Finding |
| --- | --- |
| `src/Domain/Entities/BaseEntity.cs` | Common entity base has `Id`, timestamps, soft-delete `Active`, but no tenant discriminator. |
| `src/Domain/Entities/Organization.cs` | Existing `Organization` is a domain/business object and should become tenant-owned, not the tenant root itself. |
| `src/Infrastructure/Common/Context/ApplicationDbContext.cs` | EF global filters only apply `Active`; no tenant filtering exists. |
| `src/Infrastructure/Repositories/DapperRepository.cs` | Generic Dapper repository filters only `Active`; all list/get/exists/update/delete paths need tenant scoping. |
| `src/IdentityApi/Endpoints/Login/Endpoint.cs` | Login searches only by `Login` and creates a JWT without user/tenant claims. |
| `src/WebApi/Program.cs` | WebApi validates JWT issuer/audience/signing key, but has no tenant context middleware/claims requirement. |
| `src/GrpcServer/Handlers/*` | gRPC handlers mirror CRUD surface and must receive tenant context from request metadata/JWT before repository access. |
| `src/WebClient/src/lib/api/*` | WebClient central clients can carry tenant slug/login payload/header changes in one place. |

### Overall Planning Progress

This is the initial broad roadmap. The next implementation step should be a small vertical slice proving tenant context + storage isolation on one entity (`Projects`) before expanding mechanically to all 11 resources.

---

## Target Tenancy Model

### Definitions

- **Tenant:** SaaS isolation boundary. New domain entity/table: `Tenant`/`Tenants`.
- **Organization:** Existing business entity inside a tenant. A tenant can own many organizations.
- **Tenant user:** Existing `User` belongs to exactly one tenant in v1. Cross-tenant users are explicitly out of scope until there is a membership model.
- **Tenant context:** Per-request value object containing `TenantId`, `TenantSlug`, and optional `UserId`.

### V1 Decisions

1. **Shared database, row-level tenant discriminator.** Add `TenantId uuid not null` to tenant-owned tables and enforce it in application code. Database row-level security can be a later hardening phase after app behavior is stable.
2. **Slug-based tenant selection at login.** Login accepts `tenant`/`tenantSlug` plus `login` and `password`; IdentityApi finds `User` by `(TenantId, Login)` and emits tenant claims.
3. **JWT is the primary tenant source after login.** WebApi and GrpcServer reject tenant-owned operations without a valid `tenant_id` claim.
4. **Bootstrap path is explicit.** Provide a small tenant bootstrap/admin endpoint or seed process; do not silently create tenants from arbitrary request headers.
5. **Backfill existing data into a default tenant.** Existing demo data migrates into a deterministic default tenant so current demo flows keep working.

### Out of Scope for V1

- Per-tenant databases/schemas.
- Tenant billing/subscription plans.
- Cross-tenant user memberships and tenant switching for the same login.
- Full PostgreSQL RLS policies.
- Custom domains per tenant.

---

## Tenant-Owned Tables

Add `TenantId` to these domain entities/tables:

- `Organizations`
- `Projects`
- `Users`
- `Assignments`
- `Appointments`
- `AssignmentImpediments`
- `AssignmentTypes`
- `Impediments`
- `UserAssignments`
- `UserProjects`
- `Workflows`

Create a new non-tenant-owned table:

- `Tenants`: `Id`, `Slug`, `Name`, timestamps, `Active`, unique `Slug`.

Recommended uniqueness changes:

- `Tenants.Slug`: unique globally.
- `Users`: unique `(TenantId, Login)`.
- Optional later: resource name uniqueness per tenant only where product behavior needs it.

---

## Implementation Phases

## Phase 0: Safety Net and Architecture Guardrails

### Task 0.1: Add architecture tests that fail while tenant context is missing

**Objective:** Create RED tests proving tenant-aware code is required before production changes.

**Files:**
- Modify: `tests/Architecture.Tests/ArchitectureTests.cs`
- Create: `tests/Architecture.Tests/TenantIsolationSourceTests.cs`

**Steps:**
1. Add a test asserting every non-`Tenant` entity deriving from `BaseEntity` has a `TenantId` property.
2. Add a source test asserting `ApplicationDbContext` query filters mention both `Active` and tenant context for tenant-owned entities.
3. Add a source test asserting `DapperRepository` SQL includes a tenant predicate for tenant-owned entities.
4. Run: `dotnet test tests/Architecture.Tests/Architecture.Tests.csproj`
5. Expected: FAIL because `TenantId` and tenant filtering do not exist yet.

### Task 0.2: Define tenant abstractions in Domain

**Objective:** Add minimal tenant concepts without coupling Domain to ASP.NET or EF.

**Files:**
- Create: `src/Domain/Entities/Tenant.cs`
- Create: `src/Domain/Tenancy/ITenantScoped.cs`
- Create: `src/Domain/Tenancy/TenantContext.cs`
- Create: `src/Domain/Tenancy/ITenantContextAccessor.cs`

**Design:**
```csharp
public interface ITenantScoped
{
    Guid TenantId { get; }
}

public sealed record TenantContext(Guid TenantId, string TenantSlug, Guid? UserId)
{
    public static TenantContext Empty { get; } = new(Guid.Empty, string.Empty, null);
    public bool IsResolved => TenantId != Guid.Empty;
}
```

**Verification:**
- Run: `dotnet build cpnucleo.slnx`
- Expected: PASS after references/usings are added.

### Task 0.3: Add infrastructure tenant accessor

**Objective:** Provide a scoped tenant context implementation usable by EF, Dapper, WebApi, IdentityApi, and GrpcServer.

**Files:**
- Create: `src/Infrastructure/Tenancy/TenantContextAccessor.cs`
- Modify: `src/Infrastructure/DependencyInjection.cs`

**Rules:**
- Register `ITenantContextAccessor` as scoped.
- Default state is unresolved.
- Provide `Set(TenantContext context)` only on the concrete type or an infrastructure interface; application code should read, not mutate.

**Verification:**
- Add unit tests if a suitable infrastructure test project is introduced; otherwise cover through Phase 1 architecture/integration tests.
- Run: `dotnet build cpnucleo.slnx`.

---

## Phase 1: Database Model and EF/Dapper Isolation Pilot

### Task 1.1: Add `Tenant` entity and EF mapping

**Objective:** Persist tenants and enforce unique slugs.

**Files:**
- Create: `src/Infrastructure/Common/Mappings/TenantMap.cs`
- Modify: `src/Infrastructure/Common/Context/ApplicationDbContext.cs`

**Steps:**
1. Add `DbSet<Tenant>? Tenants`.
2. Apply `TenantMap`.
3. Add `HasQueryFilter(x => x.Active)` for `Tenant` only; no tenant discriminator on tenant rows.
4. Configure `Slug` as required and unique.

**Verification:**
- Run architecture tests; expect remaining tenant-owned entity tests still fail until Task 1.2.

### Task 1.2: Add `TenantId` to tenant-owned domain entities

**Objective:** Make all business data tenant-scoped.

**Files:**
- Modify all files under `src/Domain/Entities/*.cs` except `BaseEntity.cs` and `Tenant.cs`.
- Modify DTOs/models under:
  - `src/WebApi/Common/Dtos/`
  - `src/GrpcServer.Contracts/Common/Dtos/`
  - `src/GrpcServer.Contracts/Commands/`
  - relevant `src/WebApi/Endpoints/**/Models.cs`

**Rules:**
- Entity create methods receive `tenantId` internally from application/endpoint context, not from anonymous client input after auth.
- Public create/update requests should not let callers choose arbitrary tenant IDs, except bootstrap/admin endpoints.
- Keep `Project.OrganizationId`; add `Project.TenantId` separately.

**TDD:**
- Add tests to `tests/Architecture.Tests/TenantIsolationSourceTests.cs` before modifying entities.
- Run architecture tests and confirm RED.
- Implement entity changes.
- Run architecture tests and confirm GREEN for entity-property checks.

### Task 1.3: Create EF migration with default tenant backfill

**Objective:** Migrate existing schema/data without breaking demo data.

**Files:**
- Create: `src/Infrastructure/Migrations/<timestamp>_AddTenancy.cs`
- Modify: `src/Infrastructure/Migrations/ApplicationDbContextModelSnapshot.cs`
- Modify seed SQL/data generation in `src/Infrastructure/Common/Helpers/FakeData.cs`

**Migration requirements:**
1. Insert deterministic default tenant, e.g. slug `default`, name `Default Tenant`.
2. Add `TenantId` nullable to all tenant-owned tables.
3. Backfill all existing rows to the default tenant.
4. Alter `TenantId` to `not null`.
5. Add indexes for common access patterns:
   - `(TenantId, Id)` on tenant-owned tables.
   - `(TenantId, Active)` where list filtering benefits.
   - unique `(TenantId, Login)` on `Users`.
6. Add foreign keys from tenant-owned tables to `Tenants(Id)`.

**Command:**
```bash
dotnet ef migrations add AddTenancy -p ./src/Infrastructure -s ./src/WebApi -c ApplicationDbContext
```

**Verification:**
- Run: `dotnet build cpnucleo.slnx`
- Run: `dotnet test tests/Architecture.Tests/Architecture.Tests.csproj`

### Task 1.4: Apply EF tenant query filters

**Objective:** Ensure EF queries automatically see only the current tenant's rows.

**Files:**
- Modify: `src/Infrastructure/Common/Context/ApplicationDbContext.cs`

**Design:**
- Inject `ITenantContextAccessor` into the context constructor.
- For each tenant-owned entity: `x.Active && x.TenantId == _tenantContext.TenantId`.
- Keep `Tenant` filtered only by `Active`.
- Provide a carefully named admin/backfill path using `IgnoreQueryFilters()` only where needed.

**TDD:**
- Add an EF InMemory or PostgreSQL-backed test that creates two tenants' projects and asserts each context sees only its tenant.
- Preferred project: create `tests/Infrastructure.Unit.Tests/` if no existing infrastructure test project fits.

### Task 1.5: Make generic Dapper repository tenant-aware

**Objective:** Ensure gRPC/Dapper paths cannot read/write/update/delete another tenant's rows.

**Files:**
- Modify: `src/Infrastructure/Repositories/DapperRepository.cs`
- Modify: `src/Infrastructure/UoW/UnitOfWork.cs`
- Modify: `src/Domain/UoW/IUnitOfWork.cs` if constructor contracts change.
- Modify: `src/Infrastructure/DependencyInjection.cs`

**Design:**
- Pass `ITenantContextAccessor` into `UnitOfWork` and `DapperRepository<T>`.
- Detect tenant-owned entity types via `typeof(ITenantScoped).IsAssignableFrom(typeof(T))`.
- `GetByIdAsync`, `ExistsAsync`, `GetAllAsync`, `UpdateAsync`, and `DeleteAsync` include `"TenantId" = @TenantId` for tenant-owned entities.
- `AddAsync` verifies entity `TenantId` matches current tenant before insert.
- Reject unresolved tenant context for tenant-owned repository operations.

**TDD:**
- Add source-level tests first in `tests/Architecture.Tests/DapperRepositorySourceTests.cs` for tenant predicates.
- Add a focused repository test if practical; otherwise cover through gRPC integration tests in Phase 4.

---

## Phase 2: Tenant Resolution, Authentication, and Authorization

### Task 2.1: Resolve tenant during login

**Objective:** Login must authenticate inside one tenant and issue tenant claims.

**Files:**
- Modify: `src/IdentityApi/Endpoints/Login/Models.cs`
- Modify: `src/IdentityApi/Endpoints/Login/Endpoint.cs`
- Add tests under a new `tests/IdentityApi.Unit.Tests/` or integration test project if missing.

**Behavior:**
- Request includes `TenantSlug` (or `Tenant`) plus existing `Login` and `Password`.
- Endpoint finds active tenant by slug.
- Endpoint finds active user by `TenantId` and `Login`.
- JWT includes claims:
  - `sub` or `user_id`: user id
  - `tenant_id`: tenant id
  - `tenant_slug`: tenant slug
- Invalid tenant/login/password returns the same generic auth failure shape to avoid tenant/user enumeration.

### Task 2.2: Add WebApi tenant middleware

**Objective:** Set per-request tenant context from validated JWT before FastEndpoints execute.

**Files:**
- Create: `src/WebApi/Middleware/TenantContextMiddleware.cs`
- Modify: `src/WebApi/Program.cs`
- Modify: `tests/Architecture.Tests/FastEndpointsConfigurationTests.cs`

**Rules:**
- Middleware runs after `UseAuthentication()` and before `UseAuthorization()`/`UseFastEndpoints()` or at a point where `HttpContext.User` is populated.
- Reject tenant-owned API calls with `401/403` if `tenant_id` claim is missing/invalid.
- Allow `/healthz`, Swagger in development, and login/bootstrap endpoints as anonymous exceptions.
- Add tenant tags to logs/OTel: `cpnucleo.tenant_id`, `cpnucleo.tenant_slug` where safe.

### Task 2.3: Add GrpcServer tenant context resolution

**Objective:** gRPC command handlers run with tenant context equivalent to REST.

**Files:**
- Modify: `src/GrpcServer/Program.cs`
- Create: `src/GrpcServer/Middleware` or interceptor equivalent supported by FastEndpoints.Messaging/gRPC stack.
- Modify handler tests or add source/architecture tests.

**Rules:**
- Prefer JWT bearer metadata (`authorization: Bearer ...`) if supported by the current stack.
- If gRPC auth is not wired yet, add a temporary explicit metadata contract and document it, but keep JWT as the target.
- Reject unresolved tenant before repository access.

### Task 2.4: Add tenant bootstrap/admin route

**Objective:** Provide a controlled way to create first tenants and seed tenant-scoped default data.

**Files:**
- Create: `src/IdentityApi/Endpoints/Tenant/CreateTenant/Endpoint.cs`
- Create: `src/IdentityApi/Endpoints/Tenant/CreateTenant/Models.cs`
- Add tests.

**Rules:**
- Protected by an admin secret/config or existing admin JWT claim; do not make it public anonymous in production.
- Creates tenant plus first admin user in one transaction.
- Enforces unique slug.

---

## Phase 3: Application and Endpoint Surface Expansion

### Task 3.1: Pilot `Projects` end-to-end

**Objective:** Prove one resource is tenant-safe across REST, Application, Infrastructure, Dapper, and WebClient before broad rollout.

**Files:**
- Modify: `src/Application/Features/Projects/CreateProject/CreateProjectHandler.cs`
- Modify: `src/Infrastructure/Persistence/Projects/ProjectCreateStore.cs`
- Modify: `src/WebApi/Endpoints/Project/**`
- Modify: `src/GrpcServer/Handlers/Project/**`
- Modify: `src/GrpcServer.Contracts/Commands/Project/**`
- Modify tests under `tests/Application.Unit.Tests/`, `tests/WebApi.Unit.Tests/Endpoints/ProjectEndpointsTests.cs`, and `tests/WebApi.Integration.Tests/Endpoints/Project/`.

**Rules:**
- Client requests do not submit `TenantId` for normal project CRUD.
- Application handlers receive tenant from `ITenantContextAccessor` or a request envelope, not from untrusted body fields.
- Cross-tenant `GetById`, `Update`, and `Remove` behave as not found.

**Verification:**
- `dotnet test tests/Application.Unit.Tests/Application.Unit.Tests.csproj`
- `dotnet test tests/WebApi.Unit.Tests/WebApi.Unit.Tests.csproj --filter Project`
- Integration tests if PostgreSQL is available.

### Task 3.2: Expand tenant-safe CRUD to all resources

**Objective:** Apply the proven Project pattern mechanically to all REST and gRPC resources.

**Files:**
- `src/WebApi/Endpoints/{Organization,User,Assignment,Appointment,AssignmentImpediment,AssignmentType,Impediment,UserAssignment,UserProject,Workflow}/**`
- `src/GrpcServer/Handlers/**`
- `src/GrpcServer.Contracts/Commands/**`
- `tests/WebApi.Unit.Tests/Endpoints/*EndpointsTests.cs`
- `tests/WebApi.Integration.Tests/Endpoints/**`

**Order:**
1. `Organization`, `User`, `Workflow`, `AssignmentType`, `Impediment`.
2. `Project` is already done in pilot.
3. Relationship/dependent entities: `Assignment`, `Appointment`, `AssignmentImpediment`, `UserAssignment`, `UserProject`.

**Important checks:**
- Foreign-key references must belong to the same tenant before create/update.
- Relationship rows must not connect entities from different tenants.
- Delete/remove only affects current tenant rows.

### Task 3.3: Add tenant claims to endpoint authorization tests

**Objective:** Keep existing unit tests meaningful after auth/tenant enforcement.

**Files:**
- Modify shared test helpers in `tests/WebApi.Unit.Tests/` and `tests/WebApi.Integration.Tests/Hosts/`.

**Rules:**
- Test helpers should create a default authenticated tenant principal.
- Each resource gets at least one negative cross-tenant test.

---

## Phase 4: WebClient Tenant UX

### Task 4.1: Extend login form for tenant slug

**Objective:** Let users sign into a tenant explicitly.

**Files:**
- Modify: `src/WebClient/src/routes/login/index.tsx`
- Modify: `src/WebClient/src/routes/login/login-form.ts`
- Modify: `src/WebClient/src/routes/login/login-form.test.ts`
- Modify: `src/WebClient/src/lib/api/identity-client.ts`

**Behavior:**
- Add tenant slug input with label copy like `Workspace` or `Tenant`.
- Persist last used tenant slug in `localStorage` only; do not store password/token in localStorage unless already part of current auth strategy.
- Submit `{ tenantSlug, login, password }` to IdentityApi.

**Verification:**
- Run: `bun test src/routes/login/login-form.test.ts`
- Run: `bun test && bun run typecheck && bun run build`

### Task 4.2: Display current tenant context in navigation

**Objective:** Make active tenant visible in the UI to reduce operator mistakes.

**Files:**
- Modify shared layout/navigation components under `src/WebClient/src/components/` or route files where navigation is currently defined.
- Modify auth/session helpers under `src/WebClient/src/lib/`.

**Rules:**
- Decode `tenant_slug` from JWT or retain the login response payload.
- Show current tenant/workspace in header/sidebar.
- Add logout clears tenant session state while keeping last tenant slug.

### Task 4.3: Ensure WebClient API calls carry auth only, not untrusted tenant IDs

**Objective:** Avoid client-side tenant spoofing.

**Files:**
- Modify: `src/WebClient/src/lib/api/webapi-client.ts`
- Modify: `src/WebClient/src/lib/api/types.ts`
- Modify resource metadata/tests as needed.

**Rules:**
- REST API tenant scope comes from JWT. Do not add `TenantId` to normal resource forms.
- If temporary headers are needed for dev, keep them behind explicit dev-only config and remove before production.

---

## Phase 5: Observability, Docs, and Deployment

### Task 5.1: Add tenant-safe telemetry dimensions

**Objective:** Make tenant operations observable without leaking PII or secrets.

**Files:**
- Modify existing `ConfigureOpenTelemetryOptions.cs` files in WebApi, IdentityApi, and GrpcServer.
- Modify `src/WebClient/scripts/otel.mjs` only for non-sensitive tenant slug if needed.

**Rules:**
- Prefer `tenant_slug` or hashed tenant id if dashboard readability matters.
- Never tag user password, login password payloads, JWTs, or connection strings.
- Add logs around tenant resolution failures.

### Task 5.2: Update documentation

**Objective:** Document how to operate the app as multi-tenant.

**Files:**
- Modify: `docs/wiki/architecture.md`
- Modify: `docs/wiki/database.md`
- Modify: `docs/wiki/api-reference.md`
- Modify: `docs/wiki/getting-started.md`
- Optionally add: `docs/wiki/multitenancy.md`

**Content:**
- Tenant model and default tenant.
- Login request changes.
- Bootstrap/admin tenant creation.
- Data isolation guarantees and known V1 limits.

### Task 5.3: Deployment config and seed data

**Objective:** Ensure docker/dev/prod environments have deterministic tenant seed behavior.

**Files:**
- Modify: `.env.hostinger.example`
- Modify: `.env`
- Modify: `compose.yaml`, `compose.prod.yaml`, `compose.override.yaml` if new env vars are required.
- Modify: `docker-entrypoint-initdb.d/**` if seed CSV/SQL is checked in.

**Rules:**
- Add non-secret tenant bootstrap config only.
- Keep secrets in env examples redacted/placeholders.
- Validate public demo still works with the default tenant.

---

## Validation Gates

Run before every PR:

```bash
dotnet build cpnucleo.slnx
dotnet test tests/Architecture.Tests/Architecture.Tests.csproj
```

Run for backend behavior changes:

```bash
dotnet test tests/Application.Unit.Tests/Application.Unit.Tests.csproj
dotnet test tests/WebApi.Unit.Tests/WebApi.Unit.Tests.csproj
```

Run when PostgreSQL is available:

```bash
docker compose -f compose.yaml -f compose.override.yaml up -d postgres
dotnet test tests/WebApi.Integration.Tests/WebApi.Integration.Tests.csproj
```

Run for WebClient changes:

```bash
cd src/WebClient
bun test
bun run typecheck
bun run build
```

Manual smoke after the pilot and after full rollout:

1. Create/login tenant `alpha`; create project `Alpha Project`.
2. Create/login tenant `beta`; verify `Alpha Project` is not listed.
3. Attempt direct GET/update/delete of `alpha` ids with `beta` token; expect not found/forbidden.
4. Verify gRPC equivalent list/get commands behave the same.
5. Verify telemetry shows tenant attributes without secrets.

---

## Suggested PR Breakdown

1. **PR 1 — Tenant foundations:** Domain `Tenant`, `ITenantScoped`, tenant accessor, architecture tests.
2. **PR 2 — Schema migration:** `Tenants`, `TenantId` columns, indexes, seed/backfill, EF mappings.
3. **PR 3 — EF/Dapper enforcement pilot:** query filters and Dapper tenant predicates with `Projects` end-to-end.
4. **PR 4 — Identity/JWT tenant login:** tenant login request, tenant claims, WebApi middleware.
5. **PR 5 — Full CRUD rollout:** remaining REST/gRPC entities and tests.
6. **PR 6 — WebClient tenant UX:** tenant login field, active tenant display, form/API cleanup.
7. **PR 7 — Docs/ops/observability polish:** docs, compose/env, telemetry dimensions, smoke verification.

Each PR should be rebase-merged only after CI passes.

---

## Risks and Mitigations

| Risk | Mitigation |
| --- | --- |
| Data leak through a forgotten path | Architecture/source tests for EF filters, Dapper predicates, endpoint tenant-context usage, plus cross-tenant integration tests. |
| Client spoofs `TenantId` in request body | Do not accept `TenantId` from normal public CRUD requests; tenant comes from JWT/context. |
| Existing demo data disappears | Backfill all rows into deterministic default tenant and update demo login to include default slug. |
| Dapper generic SQL accidentally updates cross-tenant rows | Tenant predicates on all `WHERE` clauses plus affected-row tests. |
| Cross-tenant relationship links | Validate referenced foreign keys belong to current tenant before create/update. |
| Auth ambiguity with duplicate logins | Unique `(TenantId, Login)` and login endpoint requires tenant slug. |
| Tests become large and slow | Keep architecture/source/unit tests as mandatory; integration tests focus on one positive and one cross-tenant negative per resource. |

---

## Acceptance Criteria

- A user logs into a tenant by slug and receives a JWT containing tenant claims.
- WebApi and GrpcServer reject tenant-owned operations without a resolved tenant context.
- Tenant A cannot list, get, update, delete, or relate Tenant B's rows through REST or gRPC.
- Existing demo data is visible under the default tenant after migration.
- Architecture tests fail if a new tenant-owned entity omits `TenantId` or bypasses tenant filters.
- WebClient supports tenant-aware login and shows the active tenant.
- Documentation explains tenant bootstrap, login, data model, and V1 limitations.
