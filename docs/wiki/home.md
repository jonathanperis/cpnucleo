# cpnucleo

Cpnucleo is a project management and task tracking system built with .NET 10, demonstrating Clean Architecture, Domain-Driven Design, and a CQRS-like dual data access strategy with REST (FastEndpoints + EF Core) and gRPC (FastEndpoints Remote Messaging + Dapper) against the same PostgreSQL database.

---

## Quick Links

| Page | Description |
|------|-------------|
| [Architecture](architecture) | Clean Architecture layers, CQRS dual implementation, DDD patterns |
| [Getting Started](getting-started) | Prerequisites, build, run with Docker Compose or locally |
| [Project Structure](project-structure) | Full tree of `src/` and `tests/` with descriptions |
| [API Reference](api-reference) | WebApi endpoints, IdentityApi auth, GrpcServer contracts |
| [WebClient CRUD](webclient-crud) | Astro/native TypeScript CRUD, edit forms, relation search, response normalization |
| [Learning Lab](learning-lab) | Guided paths, failure exercises, benchmarks and recovery |
| [Database](database) | PostgreSQL setup, EF Core, Dapper, init scripts |
| [Testing](testing) | Architecture tests, unit tests, integration tests |
| [Deployment](deployment) | Docker Compose configs, GitHub Actions CI/CD, NGINX |
| [Technologies](technologies) | Full tech stack table with versions |

---

## Key Features

- Clean Architecture boundaries validated against explicitly loaded production assemblies
- Dual data access: EF Core for the REST API, Dapper with Unit of Work for the gRPC server
- FastEndpoints for both REST endpoints and gRPC-style remote command handling
- JWT authentication via the dedicated Identity API with Argon2id-hashed credentials
- Rate limiting with fixed-window partitioning per IP (50/min WebApi, 10/min IdentityApi)
- OpenTelemetry observability with OTLP export and optional Grafana LGTM stack
- NGINX reverse proxy with least-connection load balancing across multiple WebApi instances
- Docker Compose configurations for development, default, and production environments
- AOT, Trim, and ExtraOptimize build options for production-optimized containers
- Astro and native TypeScript frontend with Tailwind CSS, IdentityApi login, prefilled edit forms and searchable relations
- Automated CI/CD with GitHub Actions deploying to Hostinger Docker Manager via GHCR

---

## Repository

[github.com/jonathanperis/cpnucleo](https://github.com/jonathanperis/cpnucleo)
