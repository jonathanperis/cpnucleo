# Testing

Cpnucleo has three test projects covering architecture validation, unit testing, and integration testing.

---

## Test Projects Overview

| Project | Framework | Focus | Key Libraries |
|---------|-----------|-------|--------------|
| Architecture.Tests | xUnit | Clean Architecture rules | NetArchTest.Rules, FluentAssertions |
| WebApi.Unit.Tests | NUnit | Endpoint unit tests | FakeItEasy, Shouldly, FastEndpoints |
| WebApi.Integration.Tests | xUnit v3 | End-to-end endpoint tests | FastEndpoints.Testing, Shouldly |

---

## Architecture Tests (`test/Architecture.Tests/`)

These tests enforce Clean Architecture dependency rules at build time using NetArchTest and FluentAssertions. They run as part of both the PR build check and the release pipeline.

### Layer Dependency Tests

| Test | Rule |
|------|------|
| `Domain_Should_Not_HaveDependencyOnOtherProjects` | Domain has no dependency on Infrastructure, WebApi, IdentityApi, GrpcServer, GrpcServer.Contracts, or WebClient |
| `Infrastructure_Should_Not_HaveDependencyOnOtherProjects` | Infrastructure has no dependency on WebApi, IdentityApi, GrpcServer, GrpcServer.Contracts, or WebClient |
| `Infrastructure_Repositories_Should_HaveDependencyOnDomain` | Repository implementations in Infrastructure depend on Domain |
| `GrpcServerContracts_Should_OnlyDependOnDomain` | GrpcServer.Contracts has no dependency on Infrastructure or presentation layers |
| `WebApi_Should_NotDependOnGrpcServer` | WebApi does not depend on GrpcServer |
| `IdentityApi_Should_NotDependOnGrpcServer` | IdentityApi does not depend on GrpcServer |

### Domain Layer Tests

| Test | Rule |
|------|------|
| `Domain_Entities_Should_InheritFromBaseEntity` | All non-abstract entities in Domain.Entities inherit from BaseEntity |
| `Domain_Repositories_Should_BeInterfaces` | All types starting with "I" in Domain.Repositories are interfaces |
| `Domain_Entities_Should_BeSealed` | All non-abstract entities are sealed |

### Infrastructure Layer Tests

| Test | Rule |
|------|------|
| `Infrastructure_Repositories_Should_ImplementDomainInterfaces` | Repository classes implement `IRepository<>` |
| `Infrastructure_DbContext_Should_BeInCorrectNamespace` | DbContext classes reside in `Infrastructure.Common.Context` |

### Naming Convention Tests

| Test | Rule |
|------|------|
| `WebApi_Dtos_Should_HaveDtoSuffix` | DTOs in WebApi.Common.Dtos end with "Dto" |
| `GrpcServer_Handlers_Should_HaveHandlerSuffix` | Handler classes end with "Handler" |
| `GrpcServerContracts_Commands_Should_HaveCommandSuffix` | Command classes end with "Command" |
| `GrpcServerContracts_Dtos_Should_HaveDtoSuffix` | DTOs in GrpcServer.Contracts end with "Dto" |
| `WebApi_Endpoints_Should_BeNamedEndpoint` | All endpoint classes are named "Endpoint" |
| `IdentityApi_Endpoints_Should_BeNamedEndpoint` | All IdentityApi endpoint classes are named "Endpoint" |

### Clean Architecture Pattern Tests

| Test | Rule |
|------|------|
| `Domain_Should_NotDependOnEntityFramework` | Domain has no dependency on Microsoft.EntityFrameworkCore |
| `Domain_Should_NotDependOnDapper` | Domain has no dependency on Dapper |
| `Domain_Should_NotDependOnNpgsql` | Domain has no dependency on Npgsql |
| `Domain_Models_Should_BeRecordsOrClasses` | Models in Domain.Models are classes or sealed |
| `Domain_Repositories_Should_StartWithI` | Repository interfaces start with "I" |
| `Infrastructure_Should_NotContainInterfaces` | Only IApplicationDbContext is an acceptable public interface in Infrastructure |
| `GrpcServer_Handlers_Should_HaveDependencyOnDomain` | gRPC handlers depend on the Domain layer |

---

## Unit Tests (`test/WebApi.Unit.Tests/`)

Unit tests for WebApi endpoints using NUnit with FakeItEasy for mocking and Shouldly for assertions.

### Structure

```
WebApi.Unit.Tests/
├── Endpoints/
│   └── ...                    # Tests organized by endpoint
├── Usings.cs
└── WebApi.Unit.Tests.csproj
```

### Key Libraries

| Library | Purpose |
|---------|---------|
| NUnit | Test framework |
| FakeItEasy | Mocking framework |
| Shouldly | Assertion library |
| FastEndpoints | Endpoint testing support |
| coverlet.collector | Code coverage |

---

## Integration Tests (`test/WebApi.Integration.Tests/`)

Integration tests that exercise the full request pipeline using FastEndpoints.Testing and xUnit v3.

### Structure

```
WebApi.Integration.Tests/
├── AssemblyInfo.cs
├── Endpoints/
│   └── ...                    # Integration tests by endpoint
├── Hosts/
│   └── ...                    # Test host/server configuration
├── Usings.cs
└── WebApi.Integration.Tests.csproj
```

### Key Libraries

| Library | Purpose |
|---------|---------|
| xUnit v3 | Test framework |
| FastEndpoints.Testing | In-memory test server for FastEndpoints |
| Shouldly | Assertion library |
| Microsoft.NET.Test.Sdk | Test SDK |

### Prerequisites

Integration tests require a running PostgreSQL database. In CI, this is provisioned via Docker Compose:

```bash
docker compose up db -d --build --force-recreate
sleep 30
```

> Integration tests are currently commented out in CI workflows and are run manually.

---

## Running Tests

### Run All Tests

```bash
dotnet test cpnucleo.slnx
```

### Run Architecture Tests Only

```bash
dotnet test test/Architecture.Tests/
```

### Run Unit Tests Only

```bash
dotnet test test/WebApi.Unit.Tests/
```

### Run Integration Tests (requires running database)

```bash
# Start the database first
docker compose up db -d
sleep 30

# Run integration tests
dotnet test test/WebApi.Integration.Tests/
```

### Run with Code Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

---

## CI Pipeline Test Execution

Architecture tests run automatically in both CI workflows:

- **build-check.yml** (PR): runs architecture tests for each service (WebApi, GrpcServer, IdentityApi, WebClient)
- **main-release.yml** (push to main): runs architecture tests before building Docker images

Unit and integration tests are configured in the workflows but currently commented out, pending further setup.
