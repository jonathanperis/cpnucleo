---
name: Cpnucleo Architecture Decisions
description: Clean Architecture enforcement, dual data access strategy (EF Core + Dapper), domain-driven design patterns
type: project
---

## Dual Data Access Strategy

The project intentionally implements two data access approaches against the same PostgreSQL database:
- **EF Core** (used by WebApi REST endpoints) — full ORM with migrations, ApplicationDbContext
- **Dapper + Dapper.AOT** (used by GrpcServer handlers) — micro-ORM with compile-time SQL, DapperRepository<T>, UnitOfWork

**Why:** Demonstrates that Clean Architecture allows swapping infrastructure without touching domain or presentation. Also compares ORM vs micro-ORM trade-offs in a real system.

**How to apply:** When adding new entities or endpoints, follow the existing pattern — REST endpoints use EF Core, gRPC handlers use Dapper. Never mix them within the same presentation project.

## Architecture Enforcement

25+ NetArchTest rules run at build time to enforce:
- Domain layer has zero external dependencies (no EF Core, Dapper, Npgsql)
- Infrastructure depends only on Domain
- Presentation projects cannot cross-reference each other (WebApi cannot depend on GrpcServer)
- Naming conventions are enforced (DTOs end with `Dto`, handlers end with `Handler`, etc.)

**How to apply:** Before adding new packages or cross-project references, check if it would violate an architecture test. Run `dotnet test test/Architecture.Tests/` to verify.

## Entity Lifecycle Pattern

All 11 domain entities follow the same pattern:
```csharp
public sealed class Entity : BaseEntity
{
    public static Entity Create(...) => new() { ... };
    public static void Update(Entity e, ...) { ... };
    public static void Remove(Entity e) { e.Active = false; e.DeletedAt = DateTime.UtcNow; };
}
```

Entities are always sealed, use factory methods, and implement soft delete via `Active` flag + `DeletedAt`.

## Endpoint/Handler Pattern

Each entity has 5 operations, creating 55 REST endpoints + 55 gRPC handlers = 110 total:
- Create, GetById, List (paginated), Update, Remove
- REST: Each in its own folder under `Endpoints/{Entity}/{Operation}/`
- gRPC: Each in its own folder under `Handlers/{Entity}/`
