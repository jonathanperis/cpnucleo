# Architecture Tests

This project contains comprehensive architecture tests to ensure the Clean Architecture pattern is consistently followed throughout the cpnucleo solution.

## Test Categories

### 1. Layer Dependency Tests (7 tests)
These tests ensure that layers only depend on appropriate other layers according to Clean Architecture principles:

- **Domain_Should_Not_HaveDependencyOnOtherProjects**: Domain layer is the core and should have no dependencies on other projects
- **Infrastructure_Should_Not_HaveDependencyOnOtherProjects**: Infrastructure should not depend on API/UI layers
- **Infrastructure_Repositories_Should_HaveDependencyOnDomain**: Repository implementations must depend on Domain
- **GrpcServerContracts_Should_OnlyDependOnDomain**: Command contracts should only depend on Domain, not Infrastructure
- **WebApi_Should_NotDependOnGrpcServer**: WebApi and GrpcServer should be independent
- **IdentityApi_Should_NotDependOnGrpcServer**: IdentityApi and GrpcServer should be independent

### 2. Domain Layer Tests (3 tests)
These tests validate Domain layer structure and conventions:

- **Domain_Entities_Should_InheritFromBaseEntity**: All entities must inherit from BaseEntity
- **Domain_Repositories_Should_BeInterfaces**: Repository definitions in Domain must be interfaces
- **Domain_Entities_Should_BeSealed**: Entities should be sealed to prevent inheritance

### 3. Infrastructure Layer Tests (2 tests)
These tests ensure Infrastructure layer implements patterns correctly:

- **Infrastructure_Repositories_Should_ImplementDomainInterfaces**: Repository implementations must implement Domain interfaces
- **Infrastructure_DbContext_Should_BeInCorrectNamespace**: DbContext should be in Infrastructure.Common.Context

### 4. Naming Convention Tests (6 tests)
These tests enforce consistent naming patterns:

- **WebApi_Dtos_Should_HaveDtoSuffix**: All DTOs in WebApi must end with "Dto"
- **GrpcServer_Handlers_Should_HaveHandlerSuffix**: All handlers must end with "Handler"
- **GrpcServerContracts_Commands_Should_HaveCommandSuffix**: All commands must end with "Command"
- **GrpcServerContracts_Dtos_Should_HaveDtoSuffix**: All DTOs in GrpcServer.Contracts must end with "Dto"
- **WebApi_Endpoints_Should_BeNamedEndpoint**: Endpoint classes should be named "Endpoint"
- **IdentityApi_Endpoints_Should_BeNamedEndpoint**: Endpoint classes should be named "Endpoint"

### 5. Interface Implementation Tests (2 tests)
These tests validate interface naming and usage:

- **Domain_Repositories_Should_StartWithI**: Repository interfaces must start with "I"
- **Infrastructure_Should_NotContainInterfaces**: Infrastructure should implement interfaces from Domain, not define new public ones (except IApplicationDbContext)

### 6. Clean Architecture Pattern Tests (5 tests)
These tests enforce Clean Architecture principles:

- **Domain_Should_NotDependOnEntityFramework**: Domain must not depend on EF Core
- **Domain_Should_NotDependOnDapper**: Domain must not depend on Dapper
- **Domain_Should_NotDependOnNpgsql**: Domain must not depend on database drivers
- **Domain_Models_Should_BeRecordsOrClasses**: Models should be classes or records
- **WebApi_Should_NotHaveBusinessLogic**: Endpoints should not contain business logic
- **GrpcServer_Handlers_Should_HaveDependencyOnDomain**: Handlers must depend on Domain entities

## Architecture Overview

The solution follows Clean Architecture with these layers:

```
┌─────────────────────────────────────────────┐
│         Presentation Layer                  │
│  WebApi, IdentityApi, GrpcServer, WebClient│
└────────────────┬────────────────────────────┘
                 │
┌────────────────▼────────────────────────────┐
│         Application Layer                   │
│       GrpcServer.Contracts                  │
└────────────────┬────────────────────────────┘
                 │
┌────────────────▼────────────────────────────┐
│        Infrastructure Layer                 │
│   EF Core, Dapper, Repositories             │
└────────────────┬────────────────────────────┘
                 │
┌────────────────▼────────────────────────────┐
│          Domain Layer (Core)                │
│   Entities, Interfaces, Models              │
└─────────────────────────────────────────────┘
```

## Running Tests

```bash
dotnet test test/Architecture.Tests/Architecture.Tests.csproj
```

## Adding New Tests

When adding new tests, follow these guidelines:

1. Group tests by category using `#region` directives
2. Use descriptive test names that explain what is being validated
3. Include comments explaining the architectural rule being enforced
4. Use NetArchTest.Rules predicates for consistent test structure
5. Assert that `testResult.IsSuccessful.Should().BeTrue()`

## Dependencies

- **NetArchTest.Rules**: For defining architecture rules and predicates
- **FluentAssertions**: For readable assertions
- **xUnit**: Test framework
