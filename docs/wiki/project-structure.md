# Project Structure

## Solution Overview

```
cpnucleo/
├── cpnucleo.slnx                         # Solution file
├── global.json                           # .NET SDK version (10.0.102)
├── compose.yaml                          # Docker Compose (default/base)
├── compose.override.yaml                 # Docker Compose (development overrides)
├── compose.prod.yaml                     # Docker Compose (production overrides)
├── nginx.conf                            # NGINX reverse proxy configuration
├── .env                                  # Environment variables
├── docker-entrypoint-initdb.d/           # PostgreSQL initialization scripts
├── .github/workflows/                    # CI/CD pipelines
├── src/                                  # Source code
└── tests/                                 # Test projects
```

---

## Source Projects (`src/`)

### Domain (`src/Domain/`)

The core business layer with zero external dependencies.

```
Domain/
├── Domain.csproj                         # No external NuGet packages
├── Usings.cs                             # Global usings
├── Common/
│   └── Security/
│       ├── IPasswordHasher.cs            # Password hashing abstraction
│       └── PasswordHash.cs                # Hash value object
├── Entities/
│   ├── BaseEntity.cs                     # Abstract base (Id, CreatedAt, UpdatedAt, DeletedAt, Active)
│   ├── Appointment.cs                    # Time tracking entries
│   ├── Assignment.cs                     # Tasks/work items
│   ├── AssignmentImpediment.cs           # Links assignments to impediments
│   ├── AssignmentType.cs                 # Task categorization
│   ├── Impediment.cs                     # Blockers/obstacles
│   ├── Organization.cs                   # Top-level organizational unit
│   ├── Project.cs                        # Projects within organizations
│   ├── User.cs                           # System users with encrypted credentials
│   ├── UserAssignment.cs                 # User-to-assignment mapping (many-to-many)
│   ├── UserProject.cs                    # User-to-project mapping (many-to-many)
│   └── Workflow.cs                       # Workflow stages with ordering
├── Models/
│   ├── PaginatedResult.cs                # Generic paginated response model
│   └── PaginationParams.cs              # Pagination request parameters
├── Repositories/
│   ├── IRepository.cs                    # Generic CRUD repository interface
│   └── IProjectRepository.cs            # Specialized project repository
└── UoW/
    └── IUnitOfWork.cs                    # Unit of Work interface for transactions
```

### Application (`src/Application/`)

Feature-oriented use cases shared by REST and gRPC transport adapters.

```
Application/
├── Application.csproj                    # Shared feature slices and Microsoft DI
├── DependencyInjection.cs                # Application handler registration
├── Features/
│   └── Projects/
│       ├── ProjectDetails.cs             # Transport-neutral project result model
│       └── CreateProject/
│           └── CreateProjectHandler.cs   # Shared create-project use case + store port
└── Usings.cs
```

### Infrastructure (`src/Infrastructure/`)

Data access implementations using both EF Core and Dapper.

```
Infrastructure/
├── Infrastructure.csproj                 # EF Core, Dapper, Dapper.AOT, Npgsql, Bogus, Delta
├── DependencyInjection.cs                # Service registration for all data access
├── Usings.cs
├── Common/
│   ├── Context/
│   │   ├── ApplicationDbContext.cs       # EF Core DbContext implementation
│   │   └── IApplicationDbContext.cs      # DbContext interface
│   ├── Helpers/
│   │   └── FakeData.cs                   # Bogus-based test data generator
│   └── Mappings/
│       └── ...                           # EF Core entity configurations
├── Migrations/
│   └── ...                               # EF Core database migrations
├── Repositories/
│   ├── DapperRepository.cs               # Generic Dapper CRUD repository
│   └── ProjectRepository.cs             # Specialized Dapper project repository
└── UoW/
    └── UnitOfWork.cs                     # Dapper-based Unit of Work with transactions
```

### WebApi (`src/WebApi/`)

REST API using FastEndpoints with EF Core data access.

```
WebApi/
├── WebApi.csproj                         # FastEndpoints, Swagger, Mapperly, OpenTelemetry
├── Program.cs                            # App configuration (rate limiting, health checks, Swagger)
├── AssemblyInfo.cs
├── Usings.cs
├── Dockerfile                            # Multi-stage build with AOT/Trim support
├── Common/
│   └── Dtos/                             # Data transfer objects
├── Endpoints/
│   ├── Appointment/
│   │   ├── CreateAppointment/            # POST /api/appointment
│   │   │   ├── Endpoint.cs
│   │   │   └── Models.cs                 # Request/Response models
│   │   ├── GetAppointmentById/           # GET /api/appointment/{id}
│   │   ├── ListAppointments/             # GET /api/appointment
│   │   ├── RemoveAppointment/            # DELETE /api/appointment/{id}
│   │   └── UpdateAppointment/            # PUT /api/appointment/{id}
│   ├── Assignment/                       # Same 5 CRUD endpoints
│   ├── AssignmentImpediment/
│   ├── AssignmentType/
│   ├── Impediment/
│   ├── Organization/
│   ├── Project/
│   ├── User/
│   ├── UserAssignment/
│   ├── UserProject/
│   └── Workflow/
├── Middlewares/
│   ├── ElapsedTimeMiddleware.cs          # Request timing
│   └── ErrorHandlingMiddleware.cs        # Global error handling
├── Properties/
│   └── launchSettings.json
├── ServiceExtensions/
│   └── ...                               # OpenTelemetry configuration
├── appsettings.json
├── appsettings.Development.json
└── appsettings.Testing.json
```

### GrpcServer (`src/GrpcServer/`)

gRPC command server using FastEndpoints Remote Messaging with Dapper data access.

```
GrpcServer/
├── GrpcServer.csproj                     # FastEndpoints.Messaging.Remote, Mapperly, OpenTelemetry
├── Program.cs                            # HTTP/2 on port 5020, HTTP/1 health on 5021, handler registration (55 handlers)
├── Usings.cs
├── Dockerfile
├── Common/
│   └── Dtos/                             # Data transfer objects
├── Handlers/
│   ├── Appointment/
│   │   ├── CreateAppointmentHandler.cs
│   │   ├── GetAppointmentByIdHandler.cs
│   │   ├── ListAppointmentsHandler.cs
│   │   ├── RemoveAppointmentHandler.cs
│   │   └── UpdateAppointmentHandler.cs
│   ├── Assignment/                       # Same 5 handlers per entity
│   ├── AssignmentImpediment/
│   ├── AssignmentType/
│   ├── Impediment/
│   ├── Organization/
│   ├── Project/
│   ├── User/
│   ├── UserAssignment/
│   ├── UserProject/
│   └── Workflow/
├── Properties/
│   └── launchSettings.json
├── ServiceExtensions/
│   └── ...                               # OpenTelemetry configuration
├── appsettings.json
└── appsettings.Development.json
```

### GrpcServer.Contracts (`src/GrpcServer.Contracts/`)

Shared command/result contracts between gRPC client and server.

```
GrpcServer.Contracts/
├── GrpcServer.Contracts.csproj           # FastEndpoints.Messaging.Core, Domain reference
├── Usings.cs
├── Common/
│   └── Dtos/                             # Shared DTOs
└── Commands/
    ├── Appointment/                      # CreateAppointmentCommand, GetAppointmentByIdCommand, etc.
    ├── Assignment/
    ├── AssignmentImpediment/
    ├── AssignmentType/
    ├── Impediment/
    ├── Organization/
    ├── Project/
    ├── User/
    ├── UserAssignment/
    ├── UserProject/
    └── Workflow/
```

### IdentityApi (`src/IdentityApi/`)

JWT authentication service.

```
IdentityApi/
├── IdentityApi.csproj                    # FastEndpoints, FastEndpoints.Security, Swagger, OpenTelemetry
├── Program.cs                            # JWT config, rate limiting (10/min), output caching
├── Usings.cs
├── Dockerfile
├── Endpoints/
│   └── Login/
│       ├── Endpoint.cs                   # POST /api/login
│       └── Models.cs                     # Request (Login, Password) / Response (Token)
├── Middlewares/
│   ├── ElapsedTimeMiddleware.cs
│   └── ErrorHandlingMiddleware.cs
├── Properties/
│   └── launchSettings.json
├── ServiceExtensions/
│   └── ...                               # OpenTelemetry configuration
├── appsettings.json
└── appsettings.Development.json
```

### WebClient (`src/WebClient/`)

Qwik + Qwik City + Tailwind frontend served from the existing WebClient container slot.

```
WebClient/
├── package.json                           # Qwik, Qwik City, Tailwind, Vite scripts
├── src/root.tsx                           # Qwik City application root
├── Usings.cs
├── Dockerfile
├── Components/
│   └── ...                               # Qwik routes, API metadata, and CRUD components
├── Properties/
│   └── launchSettings.json
├── ServiceExtensions/
│   └── ...                               # OpenTelemetry configuration
├── wwwroot/
│   └── ...                               # Static assets
├── appsettings.json
└── appsettings.Development.json
```

---

## Test Projects (`tests/`)

### Architecture.Tests (`tests/Architecture.Tests/`)

Validates Clean Architecture dependency rules using NetArchTest.

```
Architecture.Tests/
├── Architecture.Tests.csproj             # xUnit, NetArchTest.Rules, FluentAssertions
├── ArchitectureTests.cs                  # 27 architecture validation tests
├── Usings.cs
└── README.md
```

### WebApi.Unit.Tests (`tests/WebApi.Unit.Tests/`)

Unit tests for WebApi endpoints.

```
WebApi.Unit.Tests/
├── WebApi.Unit.Tests.csproj              # NUnit, FakeItEasy, Shouldly, FastEndpoints
├── Endpoints/
│   └── ...                               # Endpoint unit tests
├── Usings.cs
└── README.md
```

### WebApi.Integration.Tests (`tests/WebApi.Integration.Tests/`)

Integration tests using FastEndpoints.Testing host fixtures; run manually when a database-backed integration pass is needed.

```
WebApi.Integration.Tests/
├── WebApi.Integration.Tests.csproj       # xUnit v3, FastEndpoints.Testing, Shouldly
├── AssemblyInfo.cs
├── Endpoints/
│   └── ...                               # Endpoint integration tests
├── Hosts/
│   └── ...                               # Test host configuration
└── Usings.cs
```

---

## Configuration Files

| File | Purpose |
|------|---------|
| `compose.yaml` | Base Docker Compose with all services, PostgreSQL, NGINX |
| `compose.override.yaml` | Development overrides: build from source, Grafana LGTM |
| `compose.prod.yaml` | Production: resource limits/reservations, restart policies, logging |
| `nginx.conf` | NGINX reverse proxy with least-conn load balancing |
| `.env` | Database credentials, connection string, OTEL config |
| `global.json` | .NET SDK version pinning |
| `docker-entrypoint-initdb.d/` | SQL scripts run on PostgreSQL container startup |
