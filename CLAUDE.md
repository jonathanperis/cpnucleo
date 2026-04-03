# Cpnucleo — Claude Code Guide

Production-grade .NET 10 project management system demonstrating Clean Architecture, DDD, CQRS, and dual data access strategies.

**Live demo:** https://cpnucleo-webclient-dotnet.azurewebsites.net

---

## Tech Stack

| Technology | Purpose |
|-----------|---------|
| .NET 10 / C# | Runtime and language |
| FastEndpoints 7.2 | REST endpoints + gRPC handlers |
| Entity Framework Core 10 | ORM for REST API (WebApi) |
| Dapper + Dapper.AOT | Micro-ORM for gRPC (GrpcServer) |
| PostgreSQL 16.7 | Primary database |
| Npgsql 10 | PostgreSQL driver with multiplexing |
| Riok.Mapperly | Compile-time DTO mapping |
| MudBlazor | Blazor UI components |
| OpenTelemetry | Observability (OTLP export) |
| Docker + NGINX | Containerization + load balancing |
| GitHub Actions | CI/CD with Azure deployment |

---

## Build Commands

```sh
dotnet build cpnucleo.slnx                    # Build entire solution
dotnet test cpnucleo.slnx                     # Run all tests
dotnet test test/Architecture.Tests/           # Architecture tests only
dotnet test test/WebApi.Unit.Tests/            # Unit tests only
docker compose -f compose.yaml -f compose.override.yaml up --build  # Dev mode
docker compose -f compose.yaml -f compose.prod.yaml up -d           # Prod mode
```

---

## Architecture

```
Presentation Layer
├── WebApi (REST, FastEndpoints + EF Core, port 5000)
├── GrpcServer (gRPC, FastEndpoints.Messaging + Dapper, port 5020/5021)
├── IdentityApi (JWT auth, port 5010)
└── WebClient (Blazor Server + WASM, port 5030)

Infrastructure Layer
├── EF Core (ApplicationDbContext) — used by WebApi
├── Dapper (DapperRepository<T> + UnitOfWork) — used by GrpcServer
└── PostgreSQL 16.7 with Npgsql multiplexing

Domain Layer (zero external dependencies)
├── 11 Entities (Organization, Project, Assignment, User, etc.)
├── Repository interfaces (IRepository<T>, IProjectRepository)
└── CryptographyManager (PBKDF2)
```

**Clean Architecture enforced by 25+ NetArchTest rules** — Domain depends on nothing, Infrastructure depends only on Domain.

---

## Key Conventions

| Element | Pattern |
|---------|---------|
| Entities | Sealed classes inheriting `BaseEntity` with factory methods (Create, Update, Remove) |
| REST endpoints | Class named `Endpoint` with nested `Request`/`Response` |
| gRPC handlers | Class named `*Handler`, commands named `*Command` |
| DTOs | Named `*Dto` |
| Soft delete | `Active` flag + `DeletedAt` timestamp |

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
│   ├── Infrastructure/              # EF Core + Dapper implementations
│   ├── WebApi/                      # 55 REST endpoints (11 entities x 5 ops)
│   ├── GrpcServer/                  # 55 gRPC handlers
│   ├── GrpcServer.Contracts/        # Shared command/result DTOs
│   ├── IdentityApi/                 # JWT login (rate limited 10/min)
│   ├── WebClient/                   # Blazor UI with MudBlazor
│   └── WebApi.Client/               # Auto-generated Kiota client
├── test/
│   ├── Architecture.Tests/          # 25+ Clean Architecture rules (xUnit + NetArchTest)
│   ├── WebApi.Unit.Tests/           # Endpoint unit tests (NUnit + FakeItEasy)
│   └── WebApi.Integration.Tests/    # E2E tests (xUnit v3 + FastEndpoints.Testing)
└── wiki/                            # GitHub wiki documentation
```

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

- **PR:** Build + Architecture Tests + Container health check
- **Main:** Build (Release, TRIM=true) + Multi-platform Docker push (amd64/arm64) to GHCR + Health check + Azure deploy
- **Registry:** `ghcr.io/jonathanperis/cpnucleo-{service}:latest`

---

## Rate Limiting

- WebApi: 50 req/min per IP
- IdentityApi: 10 req/min per IP

---

## Database

- PostgreSQL 16.7 with commit timestamps enabled
- Connection: `Minimum Pool Size=10; Maximum Pool Size=10; Multiplexing=true`
- Dev mode disables durability (fsync=0, synchronous_commit=0) for speed
