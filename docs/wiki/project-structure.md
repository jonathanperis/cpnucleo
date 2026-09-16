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
  IdentityApi/                 Login and bounded refresh
  WebClient/
    src/pages/                 Static Astro routes, including [resource].astro
    src/components/            Native login/auth/theme templates
    src/features/crud/         CRUD template, controller and DOM tests
    src/lib/api/               Typed HTTP contracts and session helpers
    scripts/                   Static server and server telemetry
tests/
  Architecture.Tests/          Real assembly boundaries and selected source checks
  Application.Unit.Tests/     Use cases and domain contracts
  Security.Unit.Tests/        Hashing, login and session refresh
  WebApi.Unit.Tests/          Endpoint orchestration
  WebApi.Integration.Tests/   Disposable PostgreSQL and HTTP/gRPC contracts
labs/
  PersistenceLab/             Comparable EF/Dapper measurements
  OutboxLab/                  Simulated delivery, crash/retry and deduplication
scripts/                      Deployment, smoke, backup, restore and docs checks
docs/wiki/                    Published technical and learning material
docs/adr/                     Architecture decisions
```

The integration suite uses independent scenario data rather than order-dependent test classes. Frontend interaction tests read the built Astro markup. The solution includes lab projects so normal solution builds catch experiment compilation drift.
