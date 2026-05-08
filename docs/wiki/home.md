# cpnucleo

Cpnucleo is a project management and task tracking system built with .NET 10, demonstrating Clean Architecture, Domain-Driven Design, and a CQRS-like dual data access strategy with REST (FastEndpoints + EF Core) and gRPC (FastEndpoints Remote Messaging + Dapper) against the same PostgreSQL database.

---

## Quick Links

| Page | Description |
|------|-------------|
| [Architecture](architecture) | Clean Architecture layers, CQRS dual implementation, DDD patterns |
| [Getting Started](getting_started) | Prerequisites, build, run with Docker Compose or locally |
| [Project Structure](project_structure) | Full tree of `src/` and `test/` with descriptions |
| [API Reference](api_reference) | WebApi endpoints, IdentityApi auth, GrpcServer contracts |
| [Database](database) | PostgreSQL setup, EF Core, Dapper, init scripts |
| [Testing](testing) | Architecture tests, unit tests, integration tests |
| [Deployment](deployment) | Docker Compose configs, GitHub Actions CI/CD, NGINX |
| [Technologies](technologies) | Full tech stack table with versions |

---

## Key Features

- Clean Architecture with strict layer dependency enforcement validated by 25+ architecture tests
- Dual data access: EF Core for the REST API, Dapper with Unit of Work for the gRPC server
- FastEndpoints for both REST endpoints and gRPC-style remote command handling
- JWT authentication via the dedicated Identity API with PBKDF2-hashed credentials
- Rate limiting with fixed-window partitioning per IP (50/min WebApi, 10/min IdentityApi)
- OpenTelemetry observability with OTLP export and optional Grafana LGTM stack
- NGINX reverse proxy with least-connection load balancing across multiple WebApi instances
- Docker Compose configurations for development, default, and production environments
- AOT, Trim, and ExtraOptimize build options for production-optimized containers
- Blazor Server + WebAssembly frontend with MudBlazor UI components
- Automated CI/CD with GitHub Actions deploying to Azure Web Apps via GHCR

---

## Repository

[github.com/jonathanperis/cpnucleo](https://github.com/jonathanperis/cpnucleo)
