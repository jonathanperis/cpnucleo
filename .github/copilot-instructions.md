# Cpnucleo - AI Coding Agent Instructions

**A Production-Grade .NET 9 Microservices Reference Architecture**

This repository implements Clean Architecture with Domain-Driven Design (DDD), CQRS pattern, and microservices using .NET 9. Read this file completely before starting any work to minimize exploration time and avoid common pitfalls.

## 🎯 Project Overview

Cpnucleo is a comprehensive project management system demonstrating enterprise-grade .NET development. The business domain includes Organizations, Projects, Assignments (tasks), Users, Workflows, and Impediments (blockers).

**Key Statistics:**
- **Framework:** .NET 9.0 SDK (9.0.202+)
- **Language:** C# 13 with nullable reference types enabled
- **Lines of Code:** ~15K across 7 projects + 3 test projects
- **Architecture Style:** Clean Architecture + Microservices + CQRS
- **Data Access:** Dual strategy - EF Core 9 (reads) + Dapper 2.1 (writes)
- **Database:** PostgreSQL 16.7
- **API Framework:** FastEndpoints 7.0 (not ASP.NET Core MVC/Minimal APIs)
- **Frontend:** Blazor WebAssembly with MudBlazor

## 🏗️ Architecture & Project Structure

### Clean Architecture Layers (Dependency Flow: Outer → Inner)

```
src/
├── Domain/                    # ⭐ CORE - No external dependencies
│   ├── Entities/             # Assignment, Project, User, etc. (sealed classes)
│   ├── Repositories/         # IRepository<T>, IProjectRepository interfaces
│   ├── UoW/                  # IUnitOfWork interface
│   └── Models/               # Domain models & value objects
│
├── Infrastructure/           # Data access implementations
│   ├── Common/Context/      # ApplicationDbContext (EF Core)
│   ├── Common/Mappings/     # EF Core entity configurations
│   ├── Repositories/        # Dapper + EF Core repository implementations
│   ├── Migrations/          # EF Core migrations (managed from WebApi project)
│   └── DependencyInjection.cs
│
├── GrpcServer.Contracts/    # Command DTOs (CQRS commands)
│   └── Commands/            # CreateAssignmentCommand, UpdateProjectCommand, etc.
│
├── GrpcServer/              # ⚡ Command handlers (WRITE path via gRPC)
│   ├── Handlers/            # CQRS command handlers using Dapper
│   ├── ServiceExtensions/   # OpenTelemetry configuration
│   └── Program.cs           # Ports: 5300 (HTTP2/gRPC), 5301
│
├── WebApi/                  # 📖 Query endpoints (READ path via REST)
│   ├── Endpoints/           # FastEndpoints REPR pattern (Request-Endpoint-Response)
│   ├── Middlewares/         # ErrorHandlingMiddleware, ElapsedTimeMiddleware
│   ├── ServiceExtensions/   # Rate limiting, CORS, OpenTelemetry
│   └── Program.cs           # Ports: 5100, 5111 (load balanced)
│
├── IdentityApi/             # 🔐 Authentication service
│   ├── Endpoints/           # Login, register, token refresh
│   └── Program.cs           # Port: 5200, JWT signing
│
└── WebClient/               # 🎨 Blazor WebAssembly frontend
    ├── Components/Pages/    # Page components
    └── Program.cs           # Port: 5400

test/
├── Architecture.Tests/      # ⚠️ 25+ NetArchTest rules enforcing Clean Architecture
├── WebApi.Unit.Tests/      # Unit tests with Moq
└── WebApi.Integration.Tests/ # API integration tests with Alba
```

### Critical Architectural Rules (Enforced by Architecture.Tests)

1. **Domain Purity:** Domain layer has ZERO dependencies on EF Core, Dapper, Npgsql, or any external libraries
2. **Dependency Direction:** Infrastructure → Domain (never Domain → Infrastructure)
3. **Sealed Entities:** All domain entities are sealed classes inheriting from `BaseEntity`
4. **Repository Pattern:** Domain defines `IRepository<T>` interfaces, Infrastructure implements them
5. **Naming Conventions:**
   - Domain entities inherit from `BaseEntity`
   - Repository interfaces start with `I` and end with `Repository`
   - DTOs end with `Dto`
   - Commands end with `Command`
   - Handlers end with `Handler`
   - FastEndpoints endpoints are named `Endpoint`

### CQRS Pattern Implementation

**Commands (Writes):** Client → WebApi → GrpcServer → Handler (Dapper) → PostgreSQL
- Use gRPC/HTTP2 for inter-service communication
- Commands handled by GrpcServer with Dapper for performance
- Example: `CreateAssignmentCommand` → `CreateAssignmentHandler`

**Queries (Reads):** Client → WebApi → EF Core DbContext → PostgreSQL
- Direct EF Core queries from WebApi endpoints
- Leverage LINQ, change tracking, and projections
- Example: `GET /api/assignment/{id}` → EF Core query

## 🔨 Build, Test & Validation Commands

### Prerequisites
- **.NET 9.0 SDK (9.0.202+)** - Verified by `global.json`
- **Docker Desktop** - For database and full-stack testing
- **dotnet-ef CLI tool** - For migrations: `dotnet tool install --global dotnet-ef`

### Essential Commands (Run from Repository Root)

#### 1. Restore Dependencies (First Step Always)
```bash
dotnet restore
# Takes ~30 seconds, restores all 10 projects
```

#### 2. Build Individual Services
```bash
# WebApi (most common)
cd src/WebApi
dotnet build -c Debug
# Build takes ~20 seconds, expects 14 warnings (nullable references in FakeData.cs)

# GrpcServer
cd src/GrpcServer
dotnet build -c Debug

# IdentityApi
cd src/IdentityApi
dotnet build -c Debug

# All services at once (from root)
dotnet build
```

#### 3. Run Architecture Tests (⚠️ ALWAYS RUN BEFORE COMMITTING)
```bash
cd test/Architecture.Tests
dotnet test
# Must pass all 25 tests - enforces Clean Architecture rules
# Validates layer dependencies, naming conventions, domain purity
# Takes ~3-5 seconds
```

#### 4. Run Unit Tests
```bash
cd test/WebApi.Unit.Tests
dotnet test
```

#### 5. Run Integration Tests (Requires Database)
```bash
# Start database first
docker compose up db -d
sleep 30  # Wait for DB initialization

cd test/WebApi.Integration.Tests
dotnet test
```

#### 6. Database Migrations (When Changing Entities)
```bash
# Add migration (from root)
dotnet ef migrations add MigrationName \
  -p ./src/Infrastructure \
  -s ./src/WebApi \
  -c 'ApplicationDbContext'

# Generate SQL script for Docker initialization
dotnet ef migrations script \
  --output ./docker-entrypoint-initdb.d/001-database-dump-ddl.sql \
  --idempotent \
  -p ./src/Infrastructure \
  -s ./src/WebApi \
  -c 'ApplicationDbContext'

# Remove last migration (if needed)
dotnet ef migrations remove \
  -p ./src/Infrastructure \
  -s ./src/WebApi \
  -c 'ApplicationDbContext'
```

#### 7. Docker Compose (Full Stack)
```bash
# Development mode (builds images locally)
docker compose up -d
# Wait 30 seconds for all services to start
# Access NGINX load balancer: http://localhost:9999/healthz

# View logs
docker compose logs -f webapi1-cpnucleo

# Production mode
docker compose -f compose.prod.yaml up -d

# Stop everything
docker compose down
```

### Build Validation Checklist
Before committing code changes, ALWAYS run:
1. ✅ `dotnet restore` (from root)
2. ✅ `dotnet build` (specific project or all)
3. ✅ `cd test/Architecture.Tests && dotnet test` (MUST PASS - enforces architectural rules)
4. ✅ `docker compose up nginx -d` (if changing services)
5. ✅ Test healthcheck: `curl http://localhost:9999/healthz` (should return 200 OK)

## 🎯 Common Development Tasks

### Adding a New Entity

**Critical Pattern:** Follow existing entity structure EXACTLY to maintain architectural consistency.

1. **Create Domain Entity** (`src/Domain/Entities/NewEntity.cs`):
   ```csharp
   public sealed class NewEntity : BaseEntity
   {
       public string? Name { get; set; }
       // Add properties
       
       public static NewEntity Create(string? name, Guid id = default)
       {
           // Factory method with validation
           return new NewEntity { Id = id, Name = name };
       }
   }
   ```

2. **Add Repository Interface** (`src/Domain/Repositories/INewEntityRepository.cs`):
   ```csharp
   public interface INewEntityRepository : IRepository<NewEntity>
   {
       // Add custom methods if needed
   }
   ```

3. **Configure EF Core Mapping** (`src/Infrastructure/Common/Mappings/NewEntityConfiguration.cs`):
   ```csharp
   public class NewEntityConfiguration : IEntityTypeConfiguration<NewEntity>
   {
       public void Configure(EntityTypeBuilder<NewEntity> builder)
       {
           builder.ToTable("NewEntities");
           builder.HasKey(x => x.Id);
           builder.Property(x => x.Name).IsRequired();
       }
   }
   ```

4. **Add to DbContext** (`src/Infrastructure/Common/Context/ApplicationDbContext.cs`):
   ```csharp
   public DbSet<NewEntity> NewEntities => Set<NewEntity>();
   ```

5. **Create & Apply Migration:**
   ```bash
   dotnet ef migrations add AddNewEntity \
     -p ./src/Infrastructure -s ./src/WebApi -c 'ApplicationDbContext'
   
   dotnet ef migrations script \
     --output ./docker-entrypoint-initdb.d/001-database-dump-ddl.sql \
     --idempotent -p ./src/Infrastructure -s ./src/WebApi -c 'ApplicationDbContext'
   ```

6. **Implement Dapper Repository** (if using Unit of Work pattern):
   Repository auto-registers via `IRepository<T>` with `DapperRepository<T>`

7. **Create Commands** in `GrpcServer.Contracts/Commands/NewEntity/`:
   - `CreateNewEntityCommand.cs`
   - `UpdateNewEntityCommand.cs`
   - `RemoveNewEntityCommand.cs`

8. **Create Handlers** in `GrpcServer/Handlers/NewEntity/`:
   - `CreateNewEntityHandler.cs` (implements `ICommandHandler<CreateNewEntityCommand, CreateNewEntityResult>`)
   - Follow existing handler patterns with Unit of Work

9. **Create WebApi Endpoints** in `WebApi/Endpoints/NewEntity/`:
   - `CreateNewEntity/Endpoint.cs`
   - `GetNewEntityById/Endpoint.cs`
   - `ListNewEntities/Endpoint.cs`
   - `UpdateNewEntity/Endpoint.cs`
   - `RemoveNewEntity/Endpoint.cs`
   - Use FastEndpoints REPR pattern (see existing endpoints)

10. **Run Architecture Tests:** `cd test/Architecture.Tests && dotnet test`

### Modifying Existing Endpoints

**Pattern:** FastEndpoints uses REPR (Request-Endpoint-Response) pattern.

Example: `src/WebApi/Endpoints/Assignment/GetAssignmentById/Endpoint.cs`
```csharp
public class Endpoint : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/assignment/{id}");
        AllowAnonymous();
        Summary(s => { /* OpenAPI documentation */ });
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var assignment = await context.Assignments.FindAsync(req.Id, ct);
        if (assignment == null)
            await SendNotFoundAsync(ct);
        else
            await SendAsync(mapper.Map(assignment), cancellation: ct);
    }
}
```

**Key Points:**
- Each endpoint is a separate class in its own folder
- Dependency injection via constructor
- Configuration in `Configure()` method
- Logic in `HandleAsync()` method
- Use Mapperly-generated mappers (compile-time, zero-reflection)

### Working with Docker Builds

**Docker Build Arguments:**
```bash
docker build \
  --build-arg BUILD_CONFIGURATION=Release \
  --build-arg AOT=false \
  --build-arg TRIM=false \
  --build-arg EXTRA_OPTIMIZE=false \
  -t cpnucleo-webapi:latest \
  -f src/WebApi/Dockerfile \
  ./src
```

**Options:**
- `AOT=true`: Native Ahead-of-Time compilation (faster startup, smaller size)
- `TRIM=true`: Assembly trimming (removes unused code)
- `EXTRA_OPTIMIZE=true`: Single-file publish (cannot debug)
- `BUILD_CONFIGURATION`: Debug or Release

**Port Mapping:**
- WebApi internal: 5000 → external: 5100, 5111
- IdentityApi internal: 5010 → external: 5200
- GrpcServer internal: 5020, 5021 → external: 5300, 5301
- WebClient internal: 5030 → external: 5400
- PostgreSQL: 5432 → 5432
- NGINX: 9999 → 9999

## 🚨 Common Issues & Workarounds

### Issue 1: Database Connection Failures
**Symptom:** `Npgsql.NpgsqlException: Connection refused`
**Solution:**
```bash
docker compose ps  # Check if db is running
docker compose logs db  # Check database logs
docker compose restart db
# Wait 30 seconds for initialization
```

### Issue 2: Port Already in Use
**Symptom:** `System.IO.IOException: Failed to bind to address`
**Solution:**
```bash
# Linux/macOS
lsof -i :5100
kill -9 <PID>

# Windows
netstat -ano | findstr :5100
taskkill /PID <PID> /F

# Or change ports in compose.yaml
```

### Issue 3: EF Core Migration Errors
**Symptom:** Migration fails to generate or apply
**Solution:**
1. Ensure `-s ./src/WebApi` is specified (startup project must have connection string)
2. Check `appsettings.json` has valid `DB_CONNECTION_STRING`
3. Delete `bin/` and `obj/` folders if stale
4. Run `dotnet restore` before migration commands

### Issue 4: Architecture Tests Failing
**Symptom:** NetArchTest rules fail after code changes
**Solutions:**
- **Layer dependency violation:** Check you're not referencing Infrastructure from Domain
- **Naming convention:** Ensure DTOs end with `Dto`, Commands with `Command`, etc.
- **Domain purity:** Remove any EF Core/Dapper references from Domain layer
- **Sealed entities:** All domain entities must be sealed classes

### Issue 5: Docker Healthcheck Timeout (CI/CD)
**Symptom:** `curl http://localhost:9999/healthz` returns non-200 status
**Solution:**
```bash
# In CI/CD, increase wait time to 30 seconds
docker compose up nginx -d --build --force-recreate
sleep 30

# Retry logic (see .github/workflows/build-check-webapi.yml)
for i in {1..20}; do
  STATUS=$(curl -s -o /dev/null -w "%{http_code}" http://localhost:9999/healthz)
  if [ "$STATUS" -eq 200 ]; then exit 0; fi
  sleep 5
done
exit 1
```

### Issue 6: Nullable Reference Warnings
**Expected:** 14 warnings in `Infrastructure/Common/Helpers/FakeData.cs` (CS8601, CS8618)
- These are intentional for fake data generation
- Do NOT suppress warnings project-wide
- Only suppress in FakeData.cs if absolutely necessary

## 📝 Code Style & Conventions

### Project-Wide Conventions
- **Nullable Reference Types:** Enabled in all projects (`<Nullable>enable</Nullable>`)
- **Implicit Usings:** Enabled with global usings in `Usings.cs` files
- **Sealed Classes:** Domain entities are sealed (prevents inheritance)
- **Record Types:** Use records for DTOs and commands (immutability)
- **Source Generators:** Mapperly for object mapping (zero-reflection)

### FastEndpoints Conventions
- Each endpoint is a separate class in its own folder
- Folder structure: `Endpoints/{Entity}/{Action}/Endpoint.cs`
- Models defined in `Models.cs` next to endpoint
- OpenAPI documentation in `Configure()` method
- Authentication/authorization configured per endpoint

### Repository Pattern
- **Simple:** Direct Dapper usage with `IProjectRepository` (see `ProjectRepository.cs`)
- **Advanced:** Generic `IRepository<T>` with Unit of Work for transactional operations
- Both patterns coexist - use Unit of Work when transaction scope is needed

### Error Handling
- Centralized in `ErrorHandlingMiddleware` (WebApi, IdentityApi)
- Returns structured error responses
- Logs exceptions with `ILogger<T>`

## 🎓 Learning Resources

- **Clean Architecture:** This repo strictly follows Uncle Bob's Clean Architecture
- **CQRS:** Commands via gRPC (GrpcServer + Dapper), Queries via REST (WebApi + EF Core)
- **FastEndpoints:** NOT using traditional ASP.NET MVC/Minimal APIs - read FastEndpoints docs
- **Dapper AOT:** For maximum performance, consider Dapper.AOT attribute-based approach
- **NetArchTest:** Review `test/Architecture.Tests/README.md` for architectural rules

## ⚡ Optimization Guidelines

### Performance Best Practices
1. **Use Dapper for writes** - GrpcServer handlers use Dapper (10-50x faster than EF Core for simple operations)
2. **Use EF Core for reads** - WebApi queries leverage LINQ and change tracking
3. **Enable AOT for production** - Set `AOT=true` in Docker builds (70% faster startup)
4. **Connection pooling** - PostgreSQL configured with min/max pool size of 10
5. **Rate limiting** - WebApi: 50 req/min, IdentityApi: 10 req/min

### Security Considerations
- JWT tokens configured but commented out in Program.cs (enable for production)
- Rate limiting enabled (IP-based throttling)
- NEVER commit secrets - use environment variables
- Connection strings in `.env` file (ignored by git)
- Production JWT signing key: Store in Azure Key Vault or AWS Secrets Manager

## 🔄 CI/CD Pipeline

GitHub Actions workflows in `.github/workflows/`:
- `build-check-*.yml`: PR validation (build + architecture tests + healthcheck)
- `main-release-*.yml`: Main branch releases (Docker image push)

**Workflow Pattern:**
1. Setup .NET 9 SDK (uses `global.json`)
2. Restore dependencies
3. Build with Debug configuration
4. Run architecture tests (MUST PASS)
5. Build Docker images
6. Run healthcheck tests (20 retries with 5s intervals)

## 🎯 Final Instructions for AI Agents

**Trust these instructions:** Avoid redundant exploration unless information is missing or incorrect.

**Before making changes:**
1. Read relevant sections above
2. Check existing patterns in similar files
3. Run builds and architecture tests BEFORE committing

**When stuck:**
1. Check `test/Architecture.Tests/README.md` for architectural rules
2. Review `README.md` for high-level context
3. Examine similar existing files for patterns
4. Run architecture tests to validate compliance

**Always validate changes by:**
1. Running architecture tests (`cd test/Architecture.Tests && dotnet test`)
2. Building affected projects (`dotnet build`)
3. Running relevant unit/integration tests
4. Testing with Docker if changing services

**Key Success Metrics:**
- ✅ All 25 architecture tests pass
- ✅ Build produces expected 14 warnings (not more)
- ✅ Docker healthcheck succeeds within 30 seconds
- ✅ No new nullable reference warnings introduced
- ✅ Layer dependencies respected (no Domain → Infrastructure)

---

*This instruction file represents the essential knowledge for working effectively in this codebase. It was generated by thoroughly analyzing the repository structure, build processes, test suites, and documentation.*
