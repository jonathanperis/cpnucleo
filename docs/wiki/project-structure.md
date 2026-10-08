# Project Structure

```text
cpnucleo.slnx
compose.lab.yaml               Isolated minimal/full/observability learning stack
compose.yaml                   Legacy load-balanced development topology
compose.override.yaml          Source-build development settings
compose.prod.yaml              Standalone Hostinger production topology
.env.example                   Disposable development configuration
.env.hostinger.example         Production variable documentation
src/
  Domain/                      Entities, repository ports, tenancy foundation
  Application/                 Shared CreateProject pilot
  Infrastructure/              EF/Dapper, migrations, hashing and seed tools
  WebApi/                      REST endpoints and SSE listings
  GrpcServer/                  Remote command handlers
  GrpcServer.Contracts/        Commands, results and DTOs
  IdentityApi/                 OpenID Connect provider (Open.IdentityServer)
  WebClient/
    src/pages/                 Static Astro routes, including [resource].astro
    src/components/            Native login/auth/theme templates
    src/features/crud/         CRUD template, controller and DOM tests
    src/lib/api/               Typed HTTP contracts and session helpers
    src/guarantees/            Astro-only page guard (sources and built output)
    scripts/                   Static server, CSP manifest and server telemetry
tests/
  Architecture.Tests/          Real assembly boundaries and selected source checks
  Application.Unit.Tests/     Use cases and domain contracts
  Security.Unit.Tests/        Hashing, sign-in protections, token validation, clients
  WebApi.Unit.Tests/          Endpoint orchestration
  WebApi.Integration.Tests/   Disposable PostgreSQL and HTTP/gRPC contracts
labs/
  PersistenceLab/             Comparable EF/Dapper measurements
  OutboxLab/                  Simulated delivery, crash/retry and deduplication
  JobQueueLab/                FastEndpoints job queues on PostgreSQL, compared with the outbox
  GrpcStreamingLab/           gRPC server streaming driven by LISTEN/NOTIFY
  OpenApiLab/                 FastEndpoints.OpenApi compared with the NSwag contract
  ApiReferenceLab/            Scalar API reference and its CSP needs
  NativeAotLab/               Native AOT publish and probe of WebApi (manual, not in CI)
scripts/                      Deployment, smoke, backup, restore and docs checks
docs/wiki/                    Published technical and learning material
docs/adr/                     Architecture decisions
```

The integration suite uses independent scenario data rather than order-dependent test classes. Frontend interaction tests read the built Astro markup. The solution includes lab projects so normal solution builds catch experiment compilation drift.
