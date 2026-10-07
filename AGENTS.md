# Cpnucleo — Agent Guide

Cpnucleo is a .NET 10 project-management learning laboratory. Preserve useful comparisons between REST/gRPC and EF Core/Dapper; make demonstrated guarantees executable and label unfinished experiments honestly.

## Git and delivery

- Work on `feature/*` branches and PRs targeting `main`; never push directly to main.
- Always use `gh` for GitHub operations.
- Sync main before branching/opening PRs: `git fetch origin main && git checkout main && git pull origin main`.
- Architecture tests **MUST** pass before committing. Run affected behavioral suites as well.
- CI must pass before merge. Rebase merges only: `gh pr merge --rebase`.
- Commit, push, merge and deploy only when authorized by the user.
- Preserve other task/user work. Never reset another checkout or delete its processes/artifacts.

## Commands

```sh
dotnet build cpnucleo.slnx
dotnet test cpnucleo.slnx
dotnet test tests/Architecture.Tests/
docker compose -f compose.lab.yaml up --build -d
docker compose -f compose.lab.yaml run --rm seed
docker compose --env-file .env -f compose.prod.yaml up -d
```

`global.json` requires .NET SDK 10.0.401 or newer (`rollForward: latestMinor`); a 10.0.1xx SDK cannot satisfy it. Run `bun run typecheck`, `bun run test`, and `bun audit` from `src/WebClient` with a real Node 26 on `PATH` (a Bun `node` shim breaks jsdom). The test command builds Astro and tests generated markup with jsdom. Run `bun run build` and `bun audit` from `docs`, and `python3 scripts/check-docs-drift.py` from the root.

Integration tests require Docker and own disposable PostgreSQL containers. Never point them or lab reset tools at production. Do not claim a real-browser check from jsdom evidence.

## Structure and boundaries

- `Domain`: sealed entities, factory/update behavior, repository interfaces and `IPasswordHasher`; no external package dependencies.
- `Application`: shared use cases (beginning with `Projects/CreateProject`) and the access model (`Common/Security/ResourceAccess`, `ICurrentUser`, `IAccessGuard`).
- `Infrastructure`: EF Core context, Dapper repositories/UoW, PostgreSQL migrations and integrity triggers, access enforcement, token session validation, the HTTP error envelope, Argon2id and seed tools.
- `WebApi`: FastEndpoints REST, with multiple persistence examples.
- `GrpcServer`: independent FastEndpoints Remote Messaging host using Dapper.
- `GrpcServer.Contracts`: commands/results/DTOs.
- `IdentityApi`: login and bounded refresh.
- `WebClient`: Astro static templates and native TypeScript; no client rendering framework.
- `labs`: isolated performance and outbox experiments.

WebApi and GrpcServer must not reference each other. Architecture tests load the actual target assemblies; missing assemblies must not silently pass.

## Contracts

- REST endpoint classes are named `Endpoint`; gRPC classes use `*Handler`, commands `*Command`, and DTOs `*Dto`.
- Normal CRUD soft-deletes with `Active` and `DeletedAt`. Never substitute physical deletion. Updates must not write `Id`, `CreatedAt`, `Active` or `DeletedAt`.
- PostgreSQL triggers (`RelationshipIntegrity`) keep soft-deleted relationships consistent: active rows reference only active parents (400), parents with active dependents can't be removed (409), membership links cascade. Don't bypass them in application code.
- Every batch removal is atomic, deduplicated (`BatchIds`) and capped at 100 ids on both transports. Project `expectedVersion` uses the last observed `UpdatedAt` or `CreatedAt`; omitted versions preserve legacy last-write-wins behavior.
- Pagination query keys are flat scalar fields. Page sizes are 1–100; `search` is bounded and `ids` is a comma-separated UUID string. Complex array properties break FastEndpoints nested query binding. `PaginationParams` setters never throw (binders and the gRPC MessagePack deserializer must always build it); bounds are enforced through `Problems()`/`Require()` in REST validators, gRPC handlers and repositories.
- Keep SQL values parameterized and sorting mapped to persisted canonical columns.
- Domain factories/updates own the invariants (required names/references, positive order and hours, date ordering, bounded lengths, UTC timestamps, `PasswordPolicy`) and throw `DomainException` with a client-safe message. Keep business invariants in the domain, not in one transport's validator.
- Authorization lives in `ResourceAccess` and is enforced by Dapper repositories (read filters + `IAccessGuard`) and `AccessGuardInterceptor` for EF Core: catalog writes need admin, project data is member-only, members record only their own appointments, project creators become members. Writes to rows the caller can't see are 404 on every path. Pass the active connection/transaction (`DatabaseSession`) to the guard. Without a request there is no caller: tools must use `StaticCurrentUser.System` explicitly. Add new resources to that table; tests fail otherwise.
- JWT validation uses `JwtKeys` (HS256 by default, optional RS256 with the private key only on IdentityApi) and pins algorithm, issuer, audience and lifetime. Raw `sub` is retained (`MapInboundClaims = false`). `TokenSessionValidator` rejects inactive accounts, stale security stamps and revoked admin claims; API hosts therefore need `CPNUCLEO_ADMIN_LOGINS`. User administration requires an admin claim on both transports.
- Login is timing-safe, input-bounded, locked per login after repeated failures and concurrency-capped; never log login names. Refresh checks active accounts, the security stamp and an eight-hour original-session boundary; existing ambiguous logins fail authentication rather than selecting arbitrary users.
- HTTP errors use one envelope (`statusCode`, `message`, `errors`); only `DomainException`/`AccessDeniedException` messages reach clients. Use `Send.NotFoundEnvelopeAsync` instead of FastEndpoints' empty `NotFoundAsync`.
- Tenant types/claims are a foundation, not implemented tenant isolation.
- SSE combines per-resource local notification with 15-second external-write convergence; streams close on token expiry. Preserve cancellation and reconnect behavior.

## Frontend

Every page of the WebClient and the docs site is an `.astro` page with no client UI framework; `AstroPagesTests`, the WebClient guarantee test and the docs drift check enforce it. Resource metadata generates the eleven static CRUD routes through `[resource].astro`. Preserve route paths, API envelopes and session behavior. Render API strings with `textContent`; keep native forms/dialogs and accessible labels/error states. Preserve selected relation values across search pages and abort stale requests. Static `PUBLIC_*` values are build-time configuration (the preview CSP is derived from them); safe defaults are local URLs.

## Data and operations

Use `dotnet ef ... -p ./src/Infrastructure -s ./src/WebApi -c ApplicationDbContext` for migration tooling. The production one-shot migrator applies additive migrations before API startup. Never reseed production automatically or silently rewrite duplicate legacy accounts.

`compose.prod.yaml` is standalone. Layering development ports into it is unsafe. `compose.lab.yaml` provides minimal, `full`, `observability` and explicit `seed` profiles. Local/production durability remains enabled.

`/healthz` is liveness; `/readyz` checks PostgreSQL and the newest migration. Releases test immutable amd64 images, deploy through Hostinger Docker Manager, then publish verified multi-arch manifests. OpenTelemetry exports traces, metrics and logs through OTLP. Keep EventSource and HTTP propagation enabled when changing optimization flags.

The legacy `TRIM` flag configures ReadyToRun/self-contained output, not IL trimming. Native AOT and Dapper.AOT need explicit compatibility experiments. Investigate build warnings by cause; do not rely on a frozen expected warning count or equate source checks with behavioral coverage.
