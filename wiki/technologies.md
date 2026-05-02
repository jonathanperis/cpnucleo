# Technologies

## Runtime & Framework

| Technology | Version | Purpose |
|-----------|---------|---------|
| .NET | 10.0 | Runtime and SDK |
| ASP.NET Core | 10.0 | Web framework |
| C# | Latest (via LangVersion) | Programming language |

## Web Frameworks & API

| Technology | Version | Purpose |
|-----------|---------|---------|
| FastEndpoints | 8.1.0 | REST endpoint framework (WebApi, IdentityApi) |
| FastEndpoints.Swagger | 7.2.0 | OpenAPI/Swagger documentation |
| FastEndpoints.Security | 8.1.0 | JWT token generation and validation (IdentityApi) |
| FastEndpoints.Messaging.Remote | 8.1.0 | gRPC-style remote command handling (GrpcServer) |
| FastEndpoints.Messaging.Core | 8.1.0 | Shared command/result contracts (GrpcServer.Contracts) |
| FastEndpoints.Generator | 8.1.0 | Source generator for endpoint discovery |
| FastEndpoints.ClientGen.Kiota | 8.1.0 | API client generation (C#, TypeScript) |
| FastEndpoints.Testing | 7.2.0 | Integration test support |

## Data Access

| Technology | Version | Purpose |
|-----------|---------|---------|
| Entity Framework Core | 10.0.3 | ORM for WebApi and IdentityApi |
| EF Core Design | 10.0.3 | Migration tooling |
| Npgsql | 10.0.1 | PostgreSQL .NET driver |
| Npgsql.EntityFrameworkCore.PostgreSQL | 10.0.0 | EF Core PostgreSQL provider |
| Dapper | 2.1.72 | Micro-ORM for GrpcServer |
| Dapper.AOT | 1.0.48 | Compile-time SQL interception |
| Delta | 9.0.0 | HTTP conditional requests via DB timestamps |

## Database

| Technology | Version | Purpose |
|-----------|---------|---------|
| PostgreSQL | 16.7 | Primary database |

## Authentication

| Technology | Version | Purpose |
|-----------|---------|---------|
| Microsoft.AspNetCore.Authentication.JwtBearer | 10.0.3 | JWT Bearer authentication middleware |

## Mapping

| Technology | Version | Purpose |
|-----------|---------|---------|
| Riok.Mapperly | 4.3.1 | Compile-time object mapping (source generator) |

## Querying

| Technology | Version | Purpose |
|-----------|---------|---------|
| System.Linq.Dynamic.Core | 1.7.1 | Dynamic LINQ queries |

## Observability & Monitoring

| Technology | Version | Purpose |
|-----------|---------|---------|
| OpenTelemetry.Exporter.Console | 1.15.1 | Console telemetry export |
| OpenTelemetry.Exporter.OpenTelemetryProtocol | 1.15.0 | OTLP telemetry export |
| OpenTelemetry.Extensions.Hosting | 1.15.0 | Host integration |
| OpenTelemetry.Instrumentation.AspNetCore | 1.15.0 | ASP.NET Core instrumentation |
| OpenTelemetry.Instrumentation.Http | 1.15.0 | HTTP client instrumentation |
| OpenTelemetry.Instrumentation.Process | 1.12.0-beta.1 | Process metrics |
| OpenTelemetry.Instrumentation.Runtime | 1.15.0 | .NET runtime metrics |
| Grafana LGTM | Latest | Observability stack (dev only, via Docker) |

## Frontend

| Technology | Version | Purpose |
|-----------|---------|---------|
| Blazor Server | 10.0 | Server-side interactive rendering |
| Blazor WebAssembly | 10.0 | Client-side interactive rendering |
| MudBlazor | 8.15.0 | Material Design UI component library |
| MudBlazor.Translations | 2.7.0 | MudBlazor localization support |

## Testing

| Technology | Version | Purpose |
|-----------|---------|---------|
| xUnit | 2.9.3 | Test framework (Architecture.Tests) |
| xUnit v3 | 3.2.2 | Test framework (Integration.Tests) |
| NUnit | 4.4.0 | Test framework (Unit.Tests) |
| NetArchTest.Rules | 1.3.2 | Architecture rule validation |
| FluentAssertions | 8.8.0 | Fluent assertion library |
| FakeItEasy | 9.0.1 | Mocking framework |
| Shouldly | 4.3.0 | Assertion library |
| Bogus | 35.6.5 | Fake data generation |
| coverlet.collector | 8.0.1 | Code coverage collection |
| Microsoft.NET.Test.Sdk | 18.0.1 | .NET test infrastructure |

## Infrastructure & DevOps

| Technology | Version | Purpose |
|-----------|---------|---------|
| Docker | Latest | Containerization |
| Docker Compose | v2 | Multi-container orchestration |
| NGINX | Latest | Reverse proxy and load balancer |
| GitHub Actions | -- | CI/CD pipelines |
| Azure Web Apps | -- | Cloud hosting (production deployment target) |
| GHCR | -- | Container image registry |

## Build Optimization

| Feature | Description |
|---------|-------------|
| PublishAot | Native AOT compilation (optional) |
| PublishReadyToRun | ReadyToRun pre-compilation |
| PublishReadyToRunComposite | Composite R2R for better startup |
| InvariantGlobalization | Reduce binary size by removing culture data |
| TrimmerRemoveSymbols | Strip debug symbols |
| Multi-platform | linux/amd64 and linux/arm64/v8 |
