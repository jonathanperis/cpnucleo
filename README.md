# Cpnucleo

<div align="center">

**A Production-Grade .NET 10 Microservices Reference Architecture**

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blue)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)](https://www.docker.com/)

*A comprehensive sample project demonstrating enterprise-grade best practices, architectural patterns, and modern .NET development techniques.*

[Getting Started](#getting-started) • [Architecture](#architecture) • [Technologies](#technologies) • [Documentation](#documentation)

</div>

---

## 📋 Table of Contents

- [Overview](#overview)
- [Key Features](#key-features)
- [Architecture](#architecture)
  - [Clean Architecture](#clean-architecture)
  - [Microservices Architecture](#microservices-architecture)
  - [Domain-Driven Design](#domain-driven-design)
  - [CQRS Pattern](#cqrs-pattern)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Design Patterns & Best Practices](#design-patterns--best-practices)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
- [Building & Running](#building--running)
  - [Local Development](#local-development)
  - [Docker Deployment](#docker-deployment)
  - [Production Deployment](#production-deployment)
- [API Documentation](#api-documentation)
- [Testing Strategy](#testing-strategy)
- [Performance Optimizations](#performance-optimizations)
- [Observability](#observability)
- [Security](#security)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)

---

## 🎯 Overview

**Cpnucleo** is a comprehensive reference implementation that showcases how to build production-ready microservices using .NET 10. This project serves as both a learning resource and a template for developing enterprise-grade applications following industry best practices.

### Who Is This For?

- **Beginners**: Learn modern .NET development patterns and architectural principles
- **Intermediate Developers**: Study production-ready code organization and best practices
- **Architects**: Reference implementation for microservices and clean architecture
- **Teams**: Template for establishing coding standards and project structure

### Business Domain

The project implements a project management system with the following core entities:
- **Organizations**: Top-level entities managing multiple projects
- **Projects**: Work containers with assignments and workflows
- **Assignments**: Tasks with time tracking, workflows, and impediments
- **Assignment Types**: Categories for classifying assignments
- **Users**: Team members with role-based access
- **Workflows**: Configurable status flows for assignments
- **Impediments**: Issues blocking assignment progress
- **Appointments**: Scheduled events and meetings
- **Junction Entities**: UserAssignment, UserProject, AssignmentImpediment (many-to-many relationships)

---

## ✨ Key Features

### Architectural Excellence
- ✅ **Clean Architecture** - Clear separation of concerns with dependency inversion
- ✅ **Domain-Driven Design (DDD)** - Rich domain models with business logic encapsulation
- ✅ **CQRS Pattern** - Command-Query segregation via gRPC messaging
- ✅ **Microservices** - Independent, scalable services with clear boundaries
- ✅ **Repository Pattern** - Both EF Core and Dapper implementations
- ✅ **Unit of Work** - Transaction management with Dapper

### Modern .NET Features
- ✅ **.NET 10** - Latest framework with minimal APIs and improved performance
- ✅ **Native AOT Compilation** - Ahead-of-time compilation for faster startup
- ✅ **Trimming & Single File Publish** - Optimized deployment sizes
- ✅ **Source Generators** - Compile-time code generation (Mapperly, FastEndpoints)
- ✅ **C# 14** - Modern language features with nullable reference types

### API Development
- ✅ **FastEndpoints** - High-performance, convention-based endpoint routing
- ✅ **gRPC Communication** - Inter-service messaging with HTTP/2
- ✅ **OpenAPI/Swagger** - Auto-generated API documentation
- ✅ **API Client Generation** - Auto-generated C# and TypeScript clients
- ✅ **Rate Limiting** - IP-based request throttling
- ✅ **JWT Authentication** - Secure token-based authentication

### Data Access
- ✅ **Entity Framework Core 10** - Modern ORM with advanced features
- ✅ **Dapper** - High-performance micro-ORM for critical paths
- ✅ **Dapper AOT** - Ahead-of-time compiled queries for maximum performance
- ✅ **PostgreSQL** - Production-ready relational database
- ✅ **Delta Library** - Real-time data synchronization
- ✅ **Code-First Migrations** - Version-controlled database schema

### Frontend
- ✅ **Blazor Web App** - .NET 10 interactive rendering with server and WASM support
- ✅ **MudBlazor** - Material Design component library
- ✅ **Interactive Server Rendering** - SignalR-based UI with fast initial load

### Infrastructure & DevOps
- ✅ **Docker & Docker Compose** - Containerized deployment
- ✅ **NGINX Load Balancer** - Reverse proxy and load balancing
- ✅ **GitHub Actions** - Automated CI/CD pipelines
- ✅ **Multi-Stage Builds** - Optimized Docker images
- ✅ **Health Checks** - Service monitoring and readiness probes

### Testing
- ✅ **Architecture Tests** - NetArchTest rules enforcing architectural boundaries
- ✅ **Unit Tests** - Business logic testing with NUnit
- ✅ **Integration Tests** - API testing with Alba and xUnit v3
- ✅ **Test Isolation** - Mocked dependencies with FakeItEasy

### Observability
- ✅ **OpenTelemetry** - Distributed tracing, metrics, and logging via OTLP
- ✅ **Structured Logging** - Contextual logging with Microsoft.Extensions.Logging
- ✅ **Performance Counters** - Runtime and process metrics
- ✅ **Health Checks** - Service monitoring and readiness probes

---

## 🏗️ Architecture

### Clean Architecture

Cpnucleo implements Clean Architecture (also known as Onion Architecture or Hexagonal Architecture) to achieve:

- **Independence of Frameworks**: Business logic doesn't depend on external libraries
- **Testability**: Core business logic can be tested without UI, database, or external services
- **Independence of UI**: UI can change without affecting business rules
- **Independence of Database**: Business logic doesn't care about data storage details
- **Independence of External Agency**: Business rules don't know about the outside world

#### Layer Structure

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                        │
│        WebApi (REST) | IdentityApi | WebClient               │
│   • FastEndpoints     • JWT Auth    • Blazor Web App        │
│   • Swagger/OpenAPI   • Rate Limit  • MudBlazor              │
└─────────────────┬───────────────────────────────────────────┘
                  │ References
┌─────────────────▼───────────────────────────────────────────┐
│           Alternative gRPC Implementation (Independent)      │
│           GrpcServer | GrpcServer.Contracts                  │
│   • gRPC/HTTP2       • Command DTOs (shared with clients)   │
│   • FastEndpoints.Messaging  • CQRS Commands                │
└─────────────────┬───────────────────────────────────────────┘
                  │ Both Reference
┌─────────────────▼───────────────────────────────────────────┐
│                  Infrastructure Layer                        │
│                    Infrastructure                            │
│   • EF Core DbContext    • Dapper Repositories              │
│   • Database Migrations  • External Service Integrations    │
│   • Unit of Work         • Dependency Injection Setup       │
└─────────────────┬───────────────────────────────────────────┘
                  │ References
┌─────────────────▼───────────────────────────────────────────┐
│                     Domain Layer (Core)                      │
│                         Domain                               │
│   • Entities (Assignment, Project, User, etc.)              │
│   • Repository Interfaces (IRepository<T>, IUnitOfWork)     │
│   • Domain Models & Value Objects                           │
│   • Business Rules & Domain Logic                           │
└─────────────────────────────────────────────────────────────┘
```

**Dependency Rules**:
- Domain has no dependencies (pure business logic)
- Infrastructure depends on Domain (implements interfaces)
- WebApi, IdentityApi, and WebClient depend on Infrastructure and Domain
- GrpcServer (independent) depends on GrpcServer.Contracts, Infrastructure, and Domain
- **Important**: WebApi does NOT depend on GrpcServer - they are independent services

**Architecture Tests**: The `Architecture.Tests` project enforces these rules using NetArchTest, with 25+ tests validating:
- Layer dependencies
- Naming conventions
- Interface implementations
- Domain purity (no ORM dependencies)

### Microservices Architecture

The solution is decomposed into independently deployable services:

#### Service Catalog

| Service | Port | Purpose | Technology |
|---------|------|---------|------------|
| **WebApi** | 5100, 5111 | RESTful API with CRUD operations | FastEndpoints + EF Core |
| **IdentityApi** | 5200 | Authentication & JWT token issuance | FastEndpoints.Security |
| **GrpcServer** | 5300, 5301 | Alternative gRPC-based API implementation sample | FastEndpoints.Messaging + Dapper |
| **WebClient** | 5400 | Blazor Web App with interactive server rendering | Blazor + MudBlazor |
| **Database** | 5432 | PostgreSQL data store | PostgreSQL 16.7 |
| **NGINX** | 9999 | Load balancer & reverse proxy | NGINX |

#### Communication Patterns

```
┌──────────┐    HTTP/REST    ┌─────────────┐
│ Client   │ ──────────────► │   NGINX     │
│ (Web/App)│                 │ (Port 9999) │
└──────────┘                 └──────┬──────┘
                                    │ Load Balancing
                    ┌───────────────┼───────────────┐
                    ▼               ▼               ▼
              ┌──────────┐    ┌──────────┐   ┌─────────────┐
              │ WebApi1  │    │ WebApi2  │   │ IdentityApi │
              │(Port 5100)    │(Port 5111)   │ (Port 5200) │
              └────┬─────┘    └────┬─────┘   └──────┬──────┘
                   │               │                 │
                   │               │                 │
                   ▼               ▼                 ▼
              ┌──────────────────────────────────────┐
              │    Infrastructure + Domain Layers    │
              └────────┬─────────────────────────────┘
                       │
                       ▼
              ┌──────────────────────────────────────┐
              │         PostgreSQL (Port 5432)       │
              └──────────────────────────────────────┘

Alternative gRPC-based implementation (independent):
┌──────────┐    gRPC/HTTP2   ┌─────────────────┐
│ gRPC     │ ──────────────► │   GrpcServer    │
│ Client   │                 │  (Port 5300)    │
└──────────┘                 └────────┬────────┘
                                      │
                                      ▼
              ┌──────────────────────────────────────┐
              │    Infrastructure + Domain Layers    │
              └────────┬─────────────────────────────┘
                       │ Dapper
                       ▼
              ┌──────────────────────────────────────┐
              │         PostgreSQL (Port 5432)       │
              └──────────────────────────────────────┘
```

**Key Design Decisions**:
- **WebApi**: RESTful implementation using FastEndpoints + EF Core for read operations
- **GrpcServer**: Independent gRPC-based implementation sample using FastEndpoints.Messaging + Dapper for write operations
- **Independence**: GrpcServer is NOT called by WebApi - it's an alternative implementation pattern demonstrating gRPC
- **Shared Layers**: Both services share Infrastructure and Domain layers
- **Scalability**: WebApi instances can scale horizontally behind NGINX

### Domain-Driven Design

The Domain layer implements DDD tactical patterns:

#### Rich Domain Models

```csharp
// Entities encapsulate business logic
public sealed class Assignment : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AmountHours { get; set; }

    // Factory method enforces business rules
    public static Assignment Create(
        string? name,
        string? description,
        DateTime startDate,
        DateTime endDate,
        int amountHours,
        Guid projectId,
        Guid workflowId,
        Guid userId,
        Guid assignmentTypeId,
        Guid id = default)
    {
        return new Assignment
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Active = true,
            // ... property assignments
        };
    }
}
```

#### Repository Pattern

```csharp
// Domain defines interface (port)
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}

// Infrastructure provides implementation (adapter)
public class DapperRepository<T> : IRepository<T> where T : BaseEntity
{
    // Dapper implementation with raw SQL
}
```

#### Unit of Work Pattern

```csharp
public interface IUnitOfWork
{
    IRepository<T> GetRepository<T>() where T : BaseEntity;
    Task BeginTransactionAsync();
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
```

### CQRS Pattern

The architecture demonstrates CQRS (Command Query Responsibility Segregation) with two independent implementation approaches:

#### WebApi Approach (RESTful)
```
Client → WebApi → Infrastructure (EF Core/Dapper) → Database
         (Queries & Commands)
```

**Example Flow**:
1. Client sends HTTP request to WebApi endpoint (GET, POST, PUT, DELETE)
2. WebApi processes directly using Infrastructure layer
3. Queries use EF Core, Commands may use Dapper via Unit of Work
4. Returns data with projections and filtering

#### GrpcServer Approach (gRPC - Independent Sample)
```
gRPC Client → GrpcServer → Infrastructure (Dapper) → Database
              (Commands via gRPC Contracts)
```

**Example Flow**:
1. gRPC client sends command to GrpcServer via gRPC/HTTP2
2. GrpcServer's handler processes using Dapper (transactional)
3. Returns result back to the gRPC client

**Important**: GrpcServer is an **alternative implementation pattern**, not a dependency of WebApi. It demonstrates how to build the same functionality using gRPC instead of REST.

**Benefits of Both Patterns**:
- **Performance**: Optimized data access for each operation type
- **Flexibility**: Choose REST (WebApi) or gRPC (GrpcServer) based on client needs
- **Independence**: Services can scale and evolve separately
- **Learning**: Side-by-side comparison of REST vs gRPC implementation patterns

---

## 🛠️ Technology Stack

### Core Framework
- **.NET 10.0** - Latest framework
- **C# 14** - Modern language features
- **ASP.NET Core 10** - Web framework

### Web & API
- **FastEndpoints 7.2** - High-performance endpoint routing
  - Convention-based routing
  - Built-in validation
  - OpenAPI generation
  - Client code generation (C#, TypeScript)
- **FastEndpoints.Messaging.Remote** - gRPC integration
- **FastEndpoints.Security** - JWT authentication

### Data Access
- **Entity Framework Core 10.0** - Primary ORM
  - Code-first migrations
  - LINQ queries
  - Change tracking
- **Dapper 2.1** - Micro-ORM for performance-critical operations
- **Dapper.AOT 1.0** - Ahead-of-time compiled SQL
- **Npgsql 10.0** - PostgreSQL driver
- **Delta 8.0** - Real-time data sync library

### Frontend
- **Blazor Web App** - .NET 10 interactive rendering framework
- **MudBlazor 8.15** - Material Design UI components
- **MudBlazor.Translations** - Internationalization support

### Database
- **PostgreSQL 16.7** - Production database
  - ACID compliance
  - JSON support
  - Full-text search capabilities

### Mapping & Code Generation
- **Riok.Mapperly 4.3** - Source generator for object mapping
  - Zero-reflection mapping
  - Compile-time type safety
  - AOT-compatible

### Observability
- **OpenTelemetry 1.15** - Distributed tracing & metrics
  - OTLP exporter
  - Console exporter (development)
  - ASP.NET Core instrumentation
  - HTTP client instrumentation
  - Process & runtime metrics

### Testing
- **xUnit v3 3.2.2** - Test framework (integration tests)
- **NUnit 4.4.0** - Test framework (unit tests)
- **FakeItEasy 9.0.1** - Mocking framework
- **Shouldly 4.3.0** - Assertion library
- **FluentAssertions 8.8.0** - Assertion library (architecture tests)
- **NetArchTest.Rules 1.3.2** - Architecture testing
- **FastEndpoints.Testing 7.2.0** - API integration testing

### DevOps & Infrastructure
- **Docker** - Containerization
- **Docker Compose** - Multi-container orchestration
- **NGINX** - Reverse proxy & load balancing
- **GitHub Actions** - CI/CD pipelines

### Development Tools
- **Microsoft.EntityFrameworkCore.Design** - Migration tooling
- **FastEndpoints.Generator** - Source generators
- **System.Linq.Dynamic.Core** - Dynamic LINQ queries

---

## 📁 Project Structure

```
cpnucleo/
├── src/
│   ├── Domain/                          # Core business logic (no dependencies)
│   │   ├── Entities/                    # Domain entities (Assignment, Project, User, etc.)
│   │   ├── Repositories/                # Repository interfaces (IRepository<T>)
│   │   ├── UoW/                        # Unit of Work interface
│   │   ├── Models/                     # Domain models & value objects
│   │   └── Common/                     # Shared domain primitives
│   │
│   ├── Infrastructure/                  # External concerns implementation
│   │   ├── Common/
│   │   │   ├── Context/               # EF Core DbContext (ApplicationDbContext)
│   │   │   ├── Mappings/              # EF Core entity configurations
│   │   │   └── Helpers/               # Fake data generation
│   │   ├── Repositories/               # Dapper & EF Core repositories
│   │   ├── UoW/                       # Unit of Work implementation
│   │   ├── Migrations/                # EF Core migrations
│   │   └── DependencyInjection.cs     # Service registration
│   │
│   ├── GrpcServer.Contracts/           # Shared contracts for gRPC communication
│   │   ├── Commands/                   # Command DTOs (Create*, Update*, Remove*)
│   │   │   ├── Appointment/
│   │   │   ├── Assignment/
│   │   │   ├── Project/
│   │   │   └── ... (all entities)
│   │   └── Common/                     # Shared DTOs and base classes
│   │
│   ├── GrpcServer/                     # Command handler service (gRPC/HTTP2)
│   │   ├── Handlers/                   # Command handlers (CQRS pattern)
│   │   │   ├── Appointment/
│   │   │   ├── Assignment/
│   │   │   └── ... (all entities)
│   │   ├── Common/                    # Shared gRPC infrastructure
│   │   ├── ServiceExtensions/         # OpenTelemetry configuration
│   │   ├── Program.cs                 # Application entry point
│   │   └── Dockerfile                 # Container definition
│   │
│   ├── WebApi/                         # RESTful API service (FastEndpoints)
│   │   ├── Endpoints/                  # REST endpoints (CRUD operations)
│   │   │   ├── Appointment/
│   │   │   │   ├── CreateAppointment/
│   │   │   │   ├── GetAppointmentById/
│   │   │   │   ├── ListAppointments/
│   │   │   │   ├── UpdateAppointment/
│   │   │   │   └── RemoveAppointment/
│   │   │   └── ... (all entities)
│   │   ├── Common/                    # Shared API infrastructure
│   │   ├── Middlewares/               # Custom middleware
│   │   │   ├── ErrorHandlingMiddleware.cs
│   │   │   └── ElapsedTimeMiddleware.cs
│   │   ├── ServiceExtensions/         # Configuration extensions
│   │   ├── Program.cs                 # Application entry point
│   │   └── Dockerfile                 # Container definition
│   │
│   ├── IdentityApi/                    # Authentication service
│   │   ├── Endpoints/                  # Auth endpoints (login, register, refresh)
│   │   ├── Common/                    # JWT configuration
│   │   ├── Middlewares/               # Auth middleware
│   │   ├── Program.cs                 # Application entry point
│   │   └── Dockerfile                 # Container definition
│   │
│   ├── WebApi.Client/                   # Auto-generated Kiota API client (C#)
│   │
│   └── WebClient/                      # Blazor Web App frontend
│       ├── Components/                 # Blazor components
│       │   ├── Pages/                 # Page components
│       │   └── Layout/               # Layout components
│       ├── ServiceExtensions/         # Client service registration
│       ├── wwwroot/                   # Static assets
│       ├── Program.cs                 # Application entry point
│       └── Dockerfile                 # Container definition
│
├── test/
│   ├── Architecture.Tests/             # Architecture validation tests
│   │   ├── ArchitectureTests.cs       # 25+ NetArchTest rules
│   │   └── README.md                  # Architecture documentation
│   │
│   ├── WebApi.Unit.Tests/             # Unit tests for WebApi
│   │
│   └── WebApi.Integration.Tests/      # Integration tests with Alba
│
├── docker-entrypoint-initdb.d/        # Database initialization scripts
│   └── 001-database-dump-ddl.sql      # EF Core generated schema
│
├── .github/
│   └── workflows/                      # CI/CD pipelines
│       ├── build-check.yml            # Build & architecture validation
│       └── main-release.yml           # Production releases
│
├── compose.yaml                        # Development docker-compose
├── compose.prod.yaml                   # Production docker-compose
├── compose.override.yaml               # Local overrides
├── nginx.conf                          # NGINX configuration
├── .env                                # Environment variables
├── global.json                         # .NET SDK version
└── cpnucleo.slnx                      # Solution file
```

### Key Files

- **Program.cs** (in each service): Application bootstrapping and middleware pipeline
- **DependencyInjection.cs**: Service registration and IoC configuration
- **Dockerfile**: Multi-stage builds with AOT/trimming support
- **appsettings.json**: Configuration per environment

---

## 🎨 Design Patterns & Best Practices

### Architectural Patterns

#### 1. Clean Architecture
- **Dependency Inversion**: High-level modules don't depend on low-level modules
- **Ports & Adapters**: Domain defines interfaces, infrastructure implements them
- **Separation of Concerns**: Each layer has a single responsibility

#### 2. Repository Pattern
**Two Implementations**:

```csharp
// Basic: Direct Dapper usage for simple queries
public class ProjectRepository : IProjectRepository
{
    private readonly NpgsqlConnection _connection;
    // Simple CRUD with Dapper
}

// Advanced: Generic repository with Unit of Work
public class DapperRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly NpgsqlConnection _connection;
    private readonly NpgsqlTransaction _transaction;
    // Transactional operations
}
```

#### 3. Unit of Work
Ensures atomic transactions across multiple repositories:

```csharp
await unitOfWork.BeginTransactionAsync();
try
{
    var assignmentRepo = unitOfWork.GetRepository<Assignment>();
    await assignmentRepo.AddAsync(assignment);
    
    var userAssignmentRepo = unitOfWork.GetRepository<UserAssignment>();
    await userAssignmentRepo.AddAsync(userAssignment);
    
    await unitOfWork.CommitAsync();
}
catch
{
    await unitOfWork.RollbackAsync();
    throw;
}
```

#### 4. Command Pattern (CQRS)
Commands encapsulate all information needed to perform an action:

```csharp
public record CreateAssignmentCommand
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public DateTime StartDate { get; init; }
    // ... other properties
}

public class CreateAssignmentHandler : ICommandHandler<CreateAssignmentCommand, CreateAssignmentResult>
{
    public async Task<CreateAssignmentResult> ExecuteAsync(
        CreateAssignmentCommand command, 
        CancellationToken cancellationToken)
    {
        // Handle command
    }
}
```

#### 5. Factory Pattern
Domain entities use factory methods for creation:

```csharp
public static Assignment Create(
    string? name,
    DateTime startDate,
    DateTime endDate,
    Guid projectId,
    Guid workflowId,
    Guid userId,
    Guid assignmentTypeId,
    Guid id = default)
{
    // Validate business rules
    // Create entity
    return new Assignment { /* ... */ };
}
```

### Code Quality Practices

#### 1. Nullable Reference Types
All projects enable `<Nullable>enable</Nullable>` for compile-time null safety:

```csharp
public string? Name { get; set; }  // Can be null
public string Email { get; set; }  // Never null (compiler enforces)
```

#### 2. Implicit Usings
Global usings reduce boilerplate:

```csharp
// Usings.cs
global using System;
global using System.Threading;
global using Domain.Entities;
global using Microsoft.Extensions.Logging;
```

#### 3. Source Generators
- **Mapperly**: Zero-allocation object mapping
- **FastEndpoints.Generator**: Compile-time endpoint discovery

#### 4. Sealed Classes
Entities are sealed to prevent inheritance and enable compiler optimizations:

```csharp
public sealed class Assignment : BaseEntity { }
```

#### 5. Record Types
DTOs and commands use records for immutability:

```csharp
public record CreateAssignmentCommand
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
}
```

### API Design Patterns

#### 1. FastEndpoints Convention
Each endpoint is a separate class following REPR pattern (Request-Endpoint-Response):

```csharp
public class Endpoint : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/assignment");
        AllowAnonymous();
        Summary(s => { /* OpenAPI docs */ });
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        // Handle request
    }
}
```

#### 2. Error Handling
Centralized error handling middleware:

```csharp
public class ErrorHandlingMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // Log and return structured error response
        }
    }
}
```

#### 3. Rate Limiting
IP-based request throttling:

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        httpContext => RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 50,
                Window = TimeSpan.FromMinutes(1)
            }));
});
```

### Database Patterns

#### 1. Code-First Migrations
Schema managed through EF Core migrations:

```bash
# Add migration
dotnet ef migrations add MigrationName \
  -p ./src/Infrastructure \
  -s ./src/WebApi \
  -c 'ApplicationDbContext'

# Generate SQL script
dotnet ef migrations script \
  --output ./docker-entrypoint-initdb.d/001-database-dump-ddl.sql \
  --idempotent
```

#### 2. Fluent API Configuration
Entity configurations separate from entity classes:

```csharp
public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.ToTable("Assignments");
        builder.HasKey(x => x.Id);
        // ... other configurations
    }
}
```

#### 3. Connection Pooling
Optimized connection management:

```env
DB_CONNECTION_STRING=Host=db;Username=postgres;Password=***;
Database=cpnucleo;Minimum Pool Size=10;Maximum Pool Size=10;Multiplexing=true
```

---

## 🚀 Getting Started

### Prerequisites

#### Required
- **.NET 10.0 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/10.0))
- **Docker Desktop** ([Download](https://www.docker.com/products/docker-desktop))
- **Git** ([Download](https://git-scm.com/downloads))

#### Optional (for local development)
- **PostgreSQL 16+** ([Download](https://www.postgresql.org/download/))
- **Visual Studio 2022** or **JetBrains Rider** or **VS Code**
- **dotnet-ef CLI tool**: `dotnet tool install --global dotnet-ef`

### Installation

#### 1. Clone the Repository

```bash
git clone https://github.com/jonathanperis/cpnucleo.git
cd cpnucleo
```

#### 2. Verify SDK Version

```bash
dotnet --version
# Should be 10.0.102 or higher
```

#### 3. Restore Dependencies

```bash
dotnet restore
```

### Configuration

#### Environment Variables

Copy and configure `.env` file:

```env
# Database Configuration
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
POSTGRES_DB=cpnucleo

# Connection String
DB_CONNECTION_STRING=Host=db;Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD};Database=${POSTGRES_DB};Minimum Pool Size=10;Maximum Pool Size=10;Multiplexing=true

# OpenTelemetry (optional)
OTEL_EXPORTER_OTLP_ENDPOINT=http://otel-lgtm:4317
OTEL_METRIC_EXPORT_INTERVAL=5000
```

#### Application Settings

Each service has its own `appsettings.json`:

**WebApi** (`src/WebApi/appsettings.json`):
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "DB_CONNECTION_STRING": ""
}
```

---

## 🔨 Building & Running

### Local Development

#### Option 1: Run Individual Services

**Start Database**:
```bash
docker compose up db -d
```

**Run WebApi**:
```bash
cd src/WebApi
dotnet run
# Access: http://localhost:5000
# Swagger: http://localhost:5000/swagger
```

**Run GrpcServer**:
```bash
cd src/GrpcServer
dotnet run
# Access: http://localhost:5020
```

**Run IdentityApi**:
```bash
cd src/IdentityApi
dotnet run
# Access: http://localhost:5010
# Swagger: http://localhost:5010/swagger
```

**Run WebClient**:
```bash
cd src/WebClient
dotnet run
# Access: http://localhost:5030
```

#### Option 2: Use Docker Compose (Recommended)

**Development Environment**:
```bash
# Start all services
docker compose up -d

# View logs
docker compose logs -f

# Stop services
docker compose down
```

**Service URLs**:
- WebApi (via NGINX): http://localhost:9999
- WebApi 1: http://localhost:5100
- WebApi 2: http://localhost:5111
- IdentityApi: http://localhost:5200
- GrpcServer: http://localhost:5300
- WebClient: http://localhost:5400
- PostgreSQL: localhost:5432

### Docker Deployment

#### Build Options

The Dockerfiles support multiple build configurations:

**Build Arguments**:
- `AOT=true`: Enable Native AOT compilation
- `TRIM=true`: Enable assembly trimming
- `EXTRA_OPTIMIZE=true`: Maximum optimization (smaller size, no debugging)
- `BUILD_CONFIGURATION`: Debug or Release

**Standard Build**:
```bash
docker build \
  --build-arg BUILD_CONFIGURATION=Release \
  --build-arg AOT=false \
  --build-arg TRIM=false \
  -t cpnucleo-webapi:latest \
  -f src/WebApi/Dockerfile \
  ./src
```

**Optimized Build (AOT + Trimming)**:
```bash
docker build \
  --build-arg BUILD_CONFIGURATION=Release \
  --build-arg AOT=true \
  --build-arg TRIM=true \
  --build-arg EXTRA_OPTIMIZE=true \
  -t cpnucleo-webapi:optimized \
  -f src/WebApi/Dockerfile \
  ./src
```

**Benefits of AOT**:
- ⚡ Faster startup (no JIT compilation)
- 📦 Smaller deployment size (trimmed unused code)
- 🔒 Better security (no IL to reverse engineer)
- 💰 Lower cloud costs (faster scaling)

#### Using Docker Compose

**Development** (`compose.yaml`):
```bash
docker compose up -d
```

**Production** (`compose.prod.yaml`):
```bash
docker compose -f compose.prod.yaml up -d
```

**Production features**:
- Resource limits (CPU/Memory)
- Auto-restart policies
- JSON logging with rotation
- Health checks
- Optimized PostgreSQL settings

### Production Deployment

#### Kubernetes (Example)

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: cpnucleo-webapi
spec:
  replicas: 3
  selector:
    matchLabels:
      app: cpnucleo-webapi
  template:
    metadata:
      labels:
        app: cpnucleo-webapi
    spec:
      containers:
      - name: webapi
        image: ghcr.io/jonathanperis/cpnucleo-web-api:latest
        ports:
        - containerPort: 5000
        env:
        - name: DB_CONNECTION_STRING
          valueFrom:
            secretKeyRef:
              name: cpnucleo-secrets
              key: db-connection-string
        livenessProbe:
          httpGet:
            path: /healthz
            port: 5000
          initialDelaySeconds: 30
          periodSeconds: 10
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
```

#### Azure Container Apps

```bash
# Create resource group
az group create --name cpnucleo-rg --location eastus

# Create container app environment
az containerapp env create \
  --name cpnucleo-env \
  --resource-group cpnucleo-rg \
  --location eastus

# Deploy WebApi
az containerapp create \
  --name cpnucleo-webapi \
  --resource-group cpnucleo-rg \
  --environment cpnucleo-env \
  --image ghcr.io/jonathanperis/cpnucleo-web-api:latest \
  --target-port 5000 \
  --ingress external \
  --min-replicas 2 \
  --max-replicas 10
```

---

## 📚 API Documentation

### OpenAPI/Swagger

Interactive API documentation is available when running in Development mode:

- **WebApi**: http://localhost:5100/swagger
- **IdentityApi**: http://localhost:5200/swagger

### Auto-Generated Clients

The project generates API clients automatically:

**C# Client**:
```bash
# Generated at: wwwroot/ApiClients/CSharp/
# Usage:
var client = new Cpnucleo.WebApi.Client("http://localhost:5100");
var assignments = await client.GetAssignmentsAsync();
```

**TypeScript Client**:
```bash
# Generated at: wwwroot/ApiClients/Typescript/
# Usage:
import { CpnucleoWebApiClient } from './cpnucleo-webapi-client';
const client = new CpnucleoWebApiClient('http://localhost:5100');
const assignments = await client.getAssignments();
```

---

## 🧪 Testing Strategy

### Test Pyramid

```
       ╱╲
      ╱  ╲        Architecture Tests (25+)
     ╱────╲       Enforce architectural rules
    ╱      ╲      
   ╱────────╲     Integration Tests
  ╱          ╲    API end-to-end testing
 ╱────────────╲   
╱──────────────╲  Unit Tests
                   Business logic & handlers
```

### Running Tests

**All Tests**:
```bash
dotnet test
```

**Specific Project**:
```bash
# Architecture tests
dotnet test test/Architecture.Tests/

# Unit tests
dotnet test test/WebApi.Unit.Tests/

# Integration tests
dotnet test test/WebApi.Integration.Tests/
```

**With Coverage**:
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Architecture Tests

Located in `test/Architecture.Tests/`, these tests enforce Clean Architecture principles with 25+ tests covering:

1. **Layer Dependencies**: Domain doesn't depend on Infrastructure/UI
2. **Domain Purity**: Domain has no ORM dependencies
3. **Naming Conventions**: Entities end with proper suffixes
4. **Repository Pattern**: Repositories implement domain interfaces
5. **Sealed Classes**: Entities are sealed
6. **Interface Naming**: Repositories start with "I"

---

## ⚡ Performance Optimizations

### Native AOT Compilation

All services support Ahead-of-Time compilation:

**Benefits**:
- **Startup Time**: 70% faster (no JIT compilation)
- **Memory Usage**: 50% reduction (trimmed assemblies)
- **Deployment Size**: 80% smaller (single file, trimmed)
- **Cold Start**: Sub-second in cloud environments

### Dapper Performance

GrpcServer uses Dapper for write operations:

**Why Dapper?**
- 🚀 **Speed**: 10-50x faster than EF Core for simple operations
- 💾 **Memory**: Minimal allocations
- 🎯 **Control**: Full SQL control for optimization
- ⚡ **AOT**: Dapper.AOT provides compile-time query generation

### NGINX Load Balancing

Production uses NGINX for:
- **Load Distribution**: Least-connection algorithm
- **Caching**: Static content caching
- **Compression**: gzip compression (level 5)
- **Connection Pooling**: Keep-alive disabled for stateless APIs

---

## 📊 Observability

### OpenTelemetry Integration

All services instrument telemetry:

**Metrics Collected**:
- HTTP request duration
- Request count & error rates
- Database query performance
- Process CPU & memory usage
- .NET runtime metrics (GC, thread pool)

**Configuration**:
```csharp
builder.ConfigureOpenTelemetry();

// Sends to OTLP collector
OTEL_EXPORTER_OTLP_ENDPOINT=http://otel-lgtm:4317
```

### Application Monitoring

For production monitoring, integrate with your preferred APM solution:

```csharp
// OpenTelemetry supports multiple exporters
// Configure via OTEL_EXPORTER_OTLP_ENDPOINT environment variable
builder.ConfigureOpenTelemetry();
```

OpenTelemetry data can be exported to:
- **Grafana/Loki/Tempo** (development stack included in compose.override.yaml)
- **Azure Monitor / Application Insights** (via OTLP exporter)
- **Datadog, New Relic, Jaeger** (via OTLP protocol)

### Structured Logging

All services use `ILogger<T>`:

```csharp
Logger.LogInformation(
    "Service started processing request with payload Name: {Name}, Id: {AssignmentId}", 
    request.Name, 
    request.Id);
```

### Health Checks

All services expose `/healthz` endpoint:

```csharp
builder.Services.AddHealthChecks();
app.UseHealthChecks("/healthz");
```

---

## 🔒 Security

### Authentication & Authorization

**JWT Bearer Tokens**:
```csharp
builder.Services
    .AddAuthenticationJwtBearer(s => 
        s.SigningKey = "ForTheLoveOfGodStoreAndLoadThisSecurely")
    .AddAuthorization();
```

⚠️ **Production**: Store signing keys in Azure Key Vault or AWS Secrets Manager

**Token Configuration**:
- Issuer: `https://identity.peris-studio.dev`
- Audience: `https://peris-studio.dev`
- Expiration: 24 hours

### Rate Limiting

IP-based throttling prevents abuse:

**WebApi**:
- 50 requests per minute
- Queue: 10 requests
- Response: 429 Too Many Requests

**IdentityApi**:
- 10 requests per minute (stricter for auth)
- Queue: 3 requests

---

## 🔧 Troubleshooting

### Common Issues

#### 1. Database Connection Fails

**Symptom**: `Npgsql.NpgsqlException: Connection refused`

**Solution**:
```bash
# Check if database is running
docker compose ps

# View database logs
docker compose logs db

# Restart database
docker compose restart db
```

#### 2. Port Already in Use

**Symptom**: `System.IO.IOException: Failed to bind to address`

**Solution**:
```bash
# Find process using port (Linux/macOS)
lsof -i :5100

# Kill the process or change port in compose.yaml
```

#### 3. NGINX 502 Bad Gateway

**Symptom**: NGINX returns 502 when accessing http://localhost:9999

**Solution**:
```bash
# Check if WebApi services are healthy
docker compose ps

# View WebApi logs
docker compose logs webapi1-cpnucleo

# Restart services
docker compose restart webapi1-cpnucleo nginx
```

### FAQ

**Q: Can I run services without Docker?**  
A: Yes, but you need PostgreSQL running locally. Update connection strings in `appsettings.json`.

**Q: Why two ORMs (EF Core + Dapper)?**  
A: Strategic choice - EF Core for read-heavy operations (LINQ, change tracking), Dapper for write-heavy operations (performance, control).

**Q: Can I use SQL Server instead of PostgreSQL?**  
A: Yes, change Npgsql packages to `Microsoft.EntityFrameworkCore.SqlServer` and update connection strings.

**Q: Is this production-ready?**  
A: The architecture and patterns are production-ready. However, you should:
- Change JWT signing keys
- Configure proper SSL/TLS
- Adjust PostgreSQL settings for durability
- Set up proper monitoring
- Implement backup strategies

---

## 🤝 Contributing

Contributions are welcome! This project follows standard open-source practices.

### How to Contribute

1. **Fork** the repository
2. **Create a feature branch**: `git checkout -b feature/amazing-feature`
3. **Make your changes** following the project's coding standards
4. **Write/update tests** for your changes
5. **Run all tests**: `dotnet test`
6. **Commit your changes**: `git commit -m 'Add amazing feature'`
7. **Push to your fork**: `git push origin feature/amazing-feature`
8. **Open a Pull Request** with a detailed description

### Coding Standards

- Follow existing code style and conventions
- All public APIs must have XML documentation
- Maintain test coverage above 80%
- Architecture tests must pass
- Use meaningful commit messages
- Update documentation for significant changes

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

```
MIT License

Copyright (c) 2019 Jonathan Peris

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
```

---

## 📞 Contact & Support

- **Author**: Jonathan Peris
- **GitHub**: [@jonathanperis](https://github.com/jonathanperis)
- **Bluesky**: [@jperis.bsky.social](https://bsky.app/profile/jperis.bsky.social)
- **Issues**: [GitHub Issues](https://github.com/jonathanperis/cpnucleo/issues)

---

## 🎓 Learning Resources

### Recommended Reading

**Clean Architecture**:
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Microsoft - Clean Architecture with ASP.NET Core](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture)

**Domain-Driven Design**:
- [Domain-Driven Design by Eric Evans](https://domainlanguage.com/ddd/)
- [DDD Reference by Eric Evans](https://www.domainlanguage.com/ddd/reference/)

**Microservices**:
- [.NET Microservices: Architecture for Containerized .NET Applications](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/)
- [Building Microservices by Sam Newman](https://samnewman.io/books/building_microservices_2nd_edition/)

**.NET Performance**:
- [.NET Performance Blog](https://devblogs.microsoft.com/dotnet/category/performance/)
- [Native AOT Deployment](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/)

---

## 🙏 Acknowledgments

This project leverages the amazing work of the .NET community:

- **FastEndpoints** - For excellent API framework
- **Dapper** - For blazing-fast data access
- **MudBlazor** - For beautiful UI components
- **NetArchTest** - For architecture testing
- **PostgreSQL** - For reliable data storage
- **.NET Foundation** - For the incredible .NET platform

Special thanks to all contributors and the open-source community for their continuous support and improvements.

---

<div align="center">

**Built with ❤️ using .NET 10**

[⬆ Back to Top](#cpnucleo)

</div>
