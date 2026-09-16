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

Run `bun run typecheck`, `bun run test`, and `bun audit` from `src/WebClient`. The test command builds Astro and tests generated markup with jsdom. Run `bun run build` and `bun audit` from `docs`, and `python3 scripts/check-docs-drift.py` from the root.

Integration tests require Docker and own disposable PostgreSQL containers. Never point them or lab reset tools at production. Do not claim a real-browser check from jsdom evidence.

## Structure and boundaries

- `Domain`: sealed entities, factory/update behavior, repository interfaces and `IPasswordHasher`; no external package dependencies.
- `Application`: shared use cases, currently beginning with `Projects/CreateProject`.
- `Infrastructure`: EF Core context, Dapper repositories/UoW, PostgreSQL migrations, Argon2id and seed tools.
- `WebApi`: FastEndpoints REST, with multiple persistence examples.
- `GrpcServer`: independent FastEndpoints Remote Messaging host using Dapper.
- `GrpcServer.Contracts`: commands/results/DTOs.
- `IdentityApi`: login and bounded refresh.
- `WebClient`: Astro static templates and native TypeScript; no client rendering framework.
- `labs`: isolated performance and outbox experiments.

WebApi and GrpcServer must not reference each other. Architecture tests load the actual target assemblies; missing assemblies must not silently pass.

## Contracts

- REST endpoint classes are named `Endpoint`; gRPC classes use `*Handler`, commands `*Command`, and DTOs `*Dto`.
- Normal CRUD soft-deletes with `Active` and `DeletedAt`. Never substitute physical deletion.
- Project batch removal is atomic. Project `expectedVersion` uses the last observed `UpdatedAt` or `CreatedAt`; omitted versions preserve legacy last-write-wins behavior.
- Pagination query keys are flat scalar fields. Page sizes are 1–100; `search` is bounded and `ids` is a comma-separated UUID string. Complex array properties break FastEndpoints nested query binding.
- Keep SQL values parameterized and sorting mapped to persisted canonical columns.
- Assignment factories/updates enforce date ordering and positive hours. Keep business invariants shared across transports.
- JWT validation is enabled. Raw `sub` is retained (`MapInboundClaims = false`). User administration requires an admin claim on both transports.
- Refresh checks active accounts and an eight-hour original-session boundary; existing ambiguous logins fail authentication rather than selecting arbitrary users.
- Tenant types/claims are a foundation, not implemented tenant isolation.
- SSE combines local notification with 15-second external-write convergence; streams close on token expiry. Preserve cancellation and reconnect behavior.

## Frontend

Resource metadata generates the eleven static CRUD routes through `[resource].astro`. Preserve route paths, API envelopes and session behavior. Render API strings with `textContent`; keep native forms/dialogs and accessible labels/error states. Preserve selected relation values across search pages and abort stale requests. Static `PUBLIC_*` values are build-time configuration; safe defaults are local URLs.

## Data and operations

Use `dotnet ef ... -p ./src/Infrastructure -s ./src/WebApi -c ApplicationDbContext` for migration tooling. The production one-shot migrator applies additive migrations before API startup. Never reseed production automatically or silently rewrite duplicate legacy accounts.

`compose.prod.yaml` is standalone. Layering development ports into it is unsafe. `compose.lab.yaml` provides minimal, `full`, `observability` and explicit `seed` profiles. Local/production durability remains enabled.

`/healthz` is liveness; `/readyz` checks PostgreSQL/schema. Releases test immutable amd64 images, deploy through Hostinger Docker Manager, then publish verified multi-arch manifests. OpenTelemetry exports traces, metrics and logs through OTLP. Keep EventSource and HTTP propagation enabled when changing optimization flags.

The legacy `TRIM` flag configures ReadyToRun/self-contained output, not IL trimming. Native AOT and Dapper.AOT need explicit compatibility experiments. Investigate build warnings by cause; do not rely on a frozen expected warning count or equate source checks with behavioral coverage.
