# Technologies

This page maps technology to purpose. Exact package versions belong in executable manifests and lockfiles rather than a second manually maintained version inventory.

## Backend

| Technology | Role | Version source |
|---|---|---|
| .NET / ASP.NET Core / C# | Service hosts, shared application/domain code | [global.json](https://github.com/jonathanperis/cpnucleo/blob/main/global.json), project target frameworks |
| FastEndpoints / Swagger / Security | REST endpoints, OpenAPI and token issuance | [WebApi](https://github.com/jonathanperis/cpnucleo/blob/main/src/WebApi/WebApi.csproj), [IdentityApi](https://github.com/jonathanperis/cpnucleo/blob/main/src/IdentityApi/IdentityApi.csproj) |
| FastEndpoints Remote Messaging / MessagePack | HTTP/2 remote commands and serialization | [GrpcServer](https://github.com/jonathanperis/cpnucleo/blob/main/src/GrpcServer/GrpcServer.csproj), [contracts](https://github.com/jonathanperis/cpnucleo/blob/main/src/GrpcServer.Contracts/GrpcServer.Contracts.csproj) |
| EF Core / Npgsql / Dapper | Shared PostgreSQL schema, ORM and SQL comparisons | [Infrastructure](https://github.com/jonathanperis/cpnucleo/blob/main/src/Infrastructure/Infrastructure.csproj) |
| Argon2id | Password hashing | Infrastructure's `Konscious.Security.Cryptography.Argon2` reference |
| Delta | Timestamp-based conditional HTTP requests | Infrastructure |
| Mapperly / Dynamic LINQ | Generated DTO mapping and selected EF queries | WebApi / GrpcServer project files |
| OpenTelemetry | Traces, metrics and logs; ASP.NET Core, HTTP, SQL and runtime instrumentation | Service project files and telemetry extensions |

The Domain project has no external package references. The shared Application pilot begins with project creation. Dapper.AOT is an experiment dependency, not a claim that every repository is source-generated or Native AOT-compatible.

## Two static frontend projects

| Project | Stack | Authoritative versions |
|---|---|---|
| WebClient | Astro, native TypeScript, Tailwind CSS; Vitest/jsdom tests; Node static server and telemetry | [package.json](https://github.com/jonathanperis/cpnucleo/blob/main/src/WebClient/package.json), [bun.lock](https://github.com/jonathanperis/cpnucleo/blob/main/src/WebClient/bun.lock) |
| Documentation | Astro, TypeScript, Tailwind's Vite plugin, sitemap integration | [package.json](https://github.com/jonathanperis/cpnucleo/blob/main/docs/package.json), [bun.lock](https://github.com/jonathanperis/cpnucleo/blob/main/docs/bun.lock) |

The projects have independent dependency graphs. Use a real supported Node runtime for Astro and Bun for package installation/scripts. A manifest range states allowed versions; the lockfile records the resolved installation. See [Getting Started](../getting-started/) and the [docs contributor guide](https://github.com/jonathanperis/cpnucleo/blob/main/docs/README.md).

## Tests and infrastructure

- xUnit + NetArchTest + FluentAssertions: assembly architecture checks.
- NUnit + FakeItEasy + Shouldly: application, endpoint and security unit tests.
- xUnit v3 + FastEndpoints.Testing + Testcontainers.PostgreSql: isolated HTTP/gRPC/PostgreSQL contracts.
- Docker Compose: minimal lab, legacy load-balanced development example and standalone production.
- PostgreSQL, NGINX, Grafana LGTM and OpenTelemetry Collector: image tags live in the [Compose files](https://github.com/jonathanperis/cpnucleo/blob/main/compose.prod.yaml).
- GitHub Actions, GHCR and Hostinger Docker Manager: build, immutable image publication and deployment. Pages uses its own pinned reusable workflow.

## Publishing options

`AOT` enables an experimental Native AOT path; normal releases disable it. The legacy `TRIM` switch enables ReadyToRun/composite/self-contained output, not IL trimming. `EXTRA_OPTIMIZE` changes selected runtime switches and symbol settings while preserving EventSource and HTTP propagation. See [Deployment](../deployment/) for the current release gates and multi-architecture limitations.
