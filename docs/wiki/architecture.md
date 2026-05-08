# Architecture

Cpnucleo follows Clean Architecture principles with strict layer separation enforced by automated architecture tests (NetArchTest). The system implements a CQRS-like dual strategy where the REST API and gRPC server use different data access technologies against the same PostgreSQL database.

---

## Layer Overview

```
+-------------------------------------------------------------+
|                    Presentation Layer                         |
|  WebApi (REST)  |  GrpcServer (gRPC)  |  IdentityApi (Auth) |
|                 |                      |  WebClient (Blazor)  |
+-------------------------------------------------------------+
|                   Infrastructure Layer                        |
|  EF Core (ApplicationDbContext)  |  Dapper (UnitOfWork)      |
|  NpgsqlConnection  |  Mappings  |  Migrations                |
+-------------------------------------------------------------+
|                     Domain Layer                              |
|  Entities  |  Repositories (Interfaces)  |  UoW (Interface)  |
|  Models  |  Common (Security)                                 |
+-------------------------------------------------------------+
```

---

## Domain Layer (`src/Domain`)

The innermost layer with zero external dependencies. Architecture tests verify that Domain does not depend on EF Core, Dapper, Npgsql, or any presentation project.

### Entities

All entities inherit from `BaseEntity`, which provides:

```csharp
public abstract class BaseEntity
{
    public Guid Id { get; protected init; }
    public DateTime CreatedAt { get; protected init; }
    public DateTime? UpdatedAt { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public bool Active { get; protected set; }
}
```

Each entity is `sealed` and uses static factory methods for creation, updates, and soft deletes:

- `Create(...)` -- initializes a new entity with `Active = true`
- `Update(...)` -- modifies fields and sets `UpdatedAt`
- `Remove(...)` -- sets `Active = false` and `DeletedAt` (soft delete)

Entities are annotated with `[Table("...")]` for Dapper's advanced repository to resolve table names.

### Domain Entities

| Entity | Key Fields | Relationships |
|--------|-----------|---------------|
| Organization | Name, Description | Parent of Projects |
| Project | Name | Belongs to Organization |
| Assignment | Name, Description, StartDate, EndDate, AmountHours | Belongs to Project, Workflow, User, AssignmentType |
| AssignmentType | Name | Referenced by Assignments |
| Workflow | Name, Order | Referenced by Assignments |
| User | Name, Login, Password, Salt | PBKDF2-encrypted credentials |
| Appointment | Description, KeepDate, AmountHours | Belongs to Assignment, User |
| Impediment | Name | Referenced by AssignmentImpediments |
| AssignmentImpediment | Description | Links Assignment to Impediment |
| UserAssignment | -- | Many-to-many: User to Assignment |
| UserProject | -- | Many-to-many: User to Project |

### Repository Interfaces

- `IRepository<T>` -- generic CRUD: `GetByIdAsync`, `GetAllAsync` (paginated), `AddAsync`, `UpdateAsync`, `DeleteAsync`, `ExistsAsync`
- `IProjectRepository` -- specialized repository for Project-specific queries
- `IUnitOfWork` -- transaction management: `BeginTransactionAsync`, `CommitAsync`, `RollbackAsync`, `GetRepository<T>`

---

## Infrastructure Layer (`src/Infrastructure`)

Implements data access with two strategies side by side:

### EF Core (used by WebApi and IdentityApi)

- `ApplicationDbContext` with `IApplicationDbContext` interface
- DbSet properties for all entities
- Migrations generated from EF Core, applied via SQL init scripts in Docker
- Delta middleware for HTTP conditional requests based on database timestamps

### Dapper (used by GrpcServer)

- `DapperRepository<T>` -- generic repository using raw SQL with reflection-based column mapping
- Caches `PropertyInfo[]` via `Lazy<>` for performance
- Supports paginated queries with configurable sort column/order and SQL injection protection
- `UnitOfWork` wraps `NpgsqlConnection` + `NpgsqlTransaction` for transactional operations
- `Dapper.AOT` enabled for compile-time SQL interception

### Dependency Injection

`DependencyInjection.AddInfrastructure()` registers:

- `IApplicationDbContext` as `ApplicationDbContext` (EF Core, scoped)
- `NpgsqlConnection` (Dapper basic, scoped)
- `IProjectRepository` as `ProjectRepository` (Dapper specialized, scoped)
- `IUnitOfWork` as `UnitOfWork` (Dapper advanced with transactions, scoped)
- Optional fake data generation via Bogus when `CreateFakeData` is configured

---

## Presentation Layer

### WebApi (`src/WebApi`)

- FastEndpoints for REST API with Swagger/OpenAPI documentation
- Uses EF Core via `IApplicationDbContext` for data access
- Rate limiting: 50 requests/minute per IP with fixed-window partitioning
- Kiota-based API client generation (C# and TypeScript)
- Middleware: `ElapsedTimeMiddleware`, `ErrorHandlingMiddleware`
- Riok.Mapperly for compile-time DTO mapping

### GrpcServer (`src/GrpcServer`)

- FastEndpoints.Messaging.Remote for gRPC-style command/handler pattern
- Uses Dapper via `IUnitOfWork` for data access
- HTTP/2 on port 5021 for gRPC transport
- Command/Result pattern via `GrpcServer.Contracts`
- All 11 entities have 5 handlers each: Create, GetById, List, Remove, Update (55 handlers total)

### IdentityApi (`src/IdentityApi`)

- JWT token generation via `FastEndpoints.Security`
- Login endpoint authenticating against User credentials in the database
- Output caching with 10-second base policy
- Rate limiting: 10 requests/minute per IP
- Swagger/OpenAPI documentation

### WebClient (`src/WebClient`)

- Blazor Server + WebAssembly hybrid rendering
- MudBlazor UI component library with translations
- Interactive server and WebAssembly render modes
- Static asset serving

---

## CQRS Dual Implementation

The system demonstrates two parallel approaches to the same domain:

| Aspect | WebApi (REST) | GrpcServer (gRPC) |
|--------|--------------|-------------------|
| Framework | FastEndpoints | FastEndpoints.Messaging.Remote |
| Data Access | EF Core + ApplicationDbContext | Dapper + UnitOfWork |
| Transport | HTTP/1.1 REST | HTTP/2 gRPC |
| Internal Port | 5000 | 5021 |
| Load Balanced | Yes (NGINX, 2 instances) | No |

Both implementations share the same Domain entities and PostgreSQL database.

---

## Dependency Rules (Enforced by Architecture Tests)

- Domain depends on nothing (no EF Core, no Dapper, no Npgsql)
- Infrastructure depends only on Domain
- GrpcServer.Contracts depends only on Domain
- WebApi does not depend on GrpcServer
- IdentityApi does not depend on GrpcServer
- All entities must inherit from `BaseEntity` and be `sealed`
- All repository interfaces must start with `I`
- All endpoints must be named `Endpoint`
- All gRPC handlers must end with `Handler`
- All commands must end with `Command`
- All DTOs must end with `Dto`
