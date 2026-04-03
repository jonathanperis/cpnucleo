---
name: Cpnucleo Technology Versions
description: Pinned versions for .NET SDK, NuGet packages, and infrastructure components
type: reference
---

## Core Versions (as of 2026-04)

- **.NET SDK:** 10.0.102 (global.json with `latestMinor` rollForward)
- **FastEndpoints:** 7.2.0 (REST + gRPC + Swagger + Security + Kiota)
- **Entity Framework Core:** 10.0.3
- **Dapper:** 2.1.72 + Dapper.AOT 1.0.48
- **Npgsql:** 10.0.1 (driver) + 10.0.0 (EF Core provider)
- **PostgreSQL:** 16.7
- **Riok.Mapperly:** 4.3.1
- **MudBlazor:** 8.x
- **OpenTelemetry:** 1.15.x

## Test Frameworks

- **Architecture Tests:** xUnit + NetArchTest.Rules 1.3.2 + FluentAssertions 8.x
- **Unit Tests:** NUnit 4.x + FakeItEasy 9.x + Shouldly 4.x
- **Integration Tests:** xUnit v3 + FastEndpoints.Testing

## Service Ports

| Service | Internal | External |
|---------|----------|----------|
| WebApi 1 | 5000 | 5100 |
| WebApi 2 | 5000 | 5111 |
| IdentityApi | 5010 | 5200 |
| GrpcServer (gRPC) | 5020 | 5300 |
| GrpcServer (health) | 5021 | 5301 |
| WebClient | 5030 | 5400 |
| NGINX | 9999 | 9999 |
| PostgreSQL | 5432 | 5432 |
| Grafana LGTM (dev) | 3000 | 3000 |
