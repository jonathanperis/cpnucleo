# cpnucleo

> Project management system built with .NET 10 -- Clean Architecture, DDD, and dual REST/gRPC implementation

[![Build Check](https://github.com/jonathanperis/cpnucleo/actions/workflows/build-check.yml/badge.svg)](https://github.com/jonathanperis/cpnucleo/actions/workflows/build-check.yml) [![Main Release](https://github.com/jonathanperis/cpnucleo/actions/workflows/main-release.yml/badge.svg)](https://github.com/jonathanperis/cpnucleo/actions/workflows/main-release.yml) [![CodeQL](https://github.com/jonathanperis/cpnucleo/actions/workflows/codeql.yml/badge.svg)](https://github.com/jonathanperis/cpnucleo/actions/workflows/codeql.yml) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**[Live demo &rarr;](https://cpnucleo-webclient-dotnet.azurewebsites.net)** | **[Documentation &rarr;](https://jonathanperis.github.io/cpnucleo/)**

---

## About

Cpnucleo is a project management and task tracking system built as a .NET 10 reference implementation for Clean Architecture, Domain-Driven Design, and a CQRS-like dual data access strategy. The REST API uses FastEndpoints with EF Core while the gRPC server uses FastEndpoints Remote Messaging with Dapper, both operating against the same PostgreSQL database. Over 25 architecture tests (NetArchTest) enforce layer dependency rules at build time, ensuring the domain layer remains free of infrastructure concerns.

## Tech Stack

| Technology | Version | Purpose |
|-----------|---------|---------|
| .NET | 10.0 | Runtime and SDK |
| ASP.NET Core | 10.0 | Web framework |
| FastEndpoints | 8.1 | REST endpoint routing and gRPC messaging |
| Entity Framework Core | 10.0 | ORM for WebApi and IdentityApi |
| Dapper | 2.1 | Micro-ORM for GrpcServer |
| Dapper.AOT | 1.0 | Compile-time SQL interception |
| Npgsql | 10.0 | PostgreSQL driver |
| PostgreSQL | 16.7 | Primary database |
| Blazor Server + WebAssembly | 10.0 | Frontend rendering |
| MudBlazor | 8.x | Material Design UI components |
| Riok.Mapperly | 4.3 | Compile-time object mapping |
| OpenTelemetry | 1.15 | Distributed tracing, metrics, logging |
| Docker + Compose | Latest | Containerization and orchestration |
| NGINX | Latest | Reverse proxy and load balancing |
| GitHub Actions | -- | CI/CD pipelines |
| Azure Web Apps | -- | Production deployment target |

## Features

- Clean Architecture with strict layer separation enforced by 25+ automated NetArchTest rules
- Dual data access strategies: EF Core (REST API) and Dapper with Unit of Work (gRPC server) against the same database
- JWT authentication via a dedicated Identity API with PBKDF2-hashed credentials
- Rate limiting per IP: 50 req/min on WebApi, 10 req/min on IdentityApi
- OpenTelemetry observability with OTLP export and optional Grafana LGTM stack
- NGINX load balancing across two WebApi instances with least-connection routing
- Multi-platform Docker builds (linux/amd64 + linux/arm64/v8) with AOT, trimming, and extra optimization options
- Blazor Server + WebAssembly frontend with MudBlazor components and translation support
- Three test suites: architecture validation (xUnit + NetArchTest), unit tests (NUnit + FakeItEasy), integration tests (xUnit v3 + FastEndpoints.Testing)
- Automated CI/CD with GitHub Actions deploying to Azure Web Apps via GHCR

## Architecture

```
┌─────────────────────────────────────────────────────┐
│                 Presentation Layer                   │
│  WebApi (REST)  GrpcServer  IdentityApi  WebClient  │
│  FastEndpoints  FE.Messaging  JWT Auth    Blazor    │
│  + EF Core      + Dapper                 + MudBlazor│
└────────────────────────┬────────────────────────────┘
                         │
┌────────────────────────┴────────────────────────────┐
│               Infrastructure Layer                   │
│  ApplicationDbContext    DapperRepository<T>          │
│  EF Core Migrations     Unit of Work                 │
│                    Npgsql + PostgreSQL                │
└────────────────────────┬────────────────────────────┘
                         │
┌────────────────────────┴────────────────────────────┐
│                   Domain Layer                       │
│  11 Entities (sealed, BaseEntity, factory methods)   │
│  Repository interfaces    CryptographyManager        │
│              Zero external dependencies              │
└─────────────────────────────────────────────────────┘
```

Layer dependencies enforced by 25+ NetArchTest rules at build time.

## Getting Started

### Prerequisites

- .NET 10 SDK (10.0.102+)
- Docker
- Docker Compose v2

### Quick Start

```bash
git clone https://github.com/jonathanperis/cpnucleo.git
cd cpnucleo
docker compose up
```

| Service | URL |
|---------|-----|
| WebApi (via NGINX) | http://localhost:9999 |
| IdentityApi | http://localhost:5200 |
| GrpcServer | http://localhost:5300 |
| WebClient | http://localhost:5400 |

Development mode (build from source with Grafana LGTM observability):

```bash
docker compose -f compose.yaml -f compose.override.yaml up --build
```

Production mode (pre-built images from GHCR):

```bash
docker compose -f compose.yaml -f compose.prod.yaml up -d
```

## Project Structure

```
src/
  Domain/                      Core business logic (zero external dependencies)
  Infrastructure/              EF Core + Dapper data access implementations
  WebApi/                      REST API (FastEndpoints + EF Core)
  GrpcServer/                  gRPC command server (FastEndpoints.Messaging + Dapper)
  GrpcServer.Contracts/        Shared command/result DTOs
  IdentityApi/                 JWT authentication service
  WebApi.Client/               Auto-generated Kiota API client
  WebClient/                   Blazor Server + WebAssembly frontend

test/
  Architecture.Tests/          Clean Architecture rules (xUnit + NetArchTest)
  WebApi.Unit.Tests/           Endpoint unit tests (NUnit + FakeItEasy)
  WebApi.Integration.Tests/    End-to-end API tests (xUnit v3 + FastEndpoints.Testing)
```

## Testing

```bash
dotnet test cpnucleo.slnx                     # Run all tests
dotnet test test/Architecture.Tests/           # Architecture rules only
dotnet test test/WebApi.Unit.Tests/            # Unit tests only
```

| Suite | Framework | Coverage |
|-------|-----------|----------|
| Architecture.Tests | xUnit + NetArchTest | Layer deps, naming, sealed entities |
| WebApi.Unit.Tests | NUnit + FakeItEasy + Shouldly | Endpoint happy/negative paths |
| WebApi.Integration.Tests | xUnit v3 + FastEndpoints.Testing | Full HTTP CRUD per entity |

## CI/CD

**build-check.yml** (pull requests): builds each service, runs architecture tests, performs container health check validation.

**main-release.yml** (push to main): builds and tests with `TRIM=true` and `EXTRA_OPTIMIZE=true`, builds multi-platform Docker images, pushes to GHCR, runs container health checks, deploys to Azure Web Apps.

## Documentation

See the [Wiki](https://github.com/jonathanperis/cpnucleo/wiki) for detailed documentation on architecture, API reference, database setup, testing, and deployment.

## Contributing

Contributing guidelines, security policy, and code of conduct are maintained in the [jonathanperis/.github](https://github.com/jonathanperis/.github) repository and apply to all repositories.

## License

MIT -- see [LICENSE](LICENSE)
