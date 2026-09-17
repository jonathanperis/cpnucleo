---
name: Cpnucleo Architecture Decisions
description: Clean Architecture enforcement, dual data access strategy (EF Core + Dapper), domain-driven design patterns
type: project
---

## Dual Data Access Strategy

The project intentionally implements two data access approaches against the same PostgreSQL database:
- **EF Core** — selected REST operations, Identity and shared migrations via ApplicationDbContext
- **Dapper** — REST repository/UoW examples and gRPC handlers; Dapper.AOT compatibility remains experimental

**Why:** Demonstrates that Clean Architecture allows swapping infrastructure without touching domain or presentation. Also compares ORM vs micro-ORM trade-offs in a real system.

**How to apply:** Preserve useful comparisons. Inspect the endpoint's existing strategy and share domain/use-case rules where appropriate; REST intentionally mixes EF Core and Dapper.

## Architecture Enforcement

Architecture tests run explicitly through `dotnet test` and inspect real target assemblies:
- Domain layer has zero external dependencies (no EF Core, Dapper, Npgsql)
- Infrastructure depends on Application and Domain, not transport hosts
- Presentation projects cannot cross-reference each other (WebApi cannot depend on GrpcServer)
- Naming conventions are enforced (DTOs end with `Dto`, handlers end with `Handler`, etc.)

**How to apply:** Before adding new packages or cross-project references, check if it would violate an architecture test. Run `dotnet test tests/Architecture.Tests/` to verify.

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
