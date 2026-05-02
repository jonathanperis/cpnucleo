# Database

Cpnucleo uses PostgreSQL 16.7 as its primary database, accessed via two parallel data access strategies: EF Core (for the REST API) and Dapper (for the gRPC server).

---

## Database Setup

### Docker (Automatic)

The database is automatically provisioned when running with Docker Compose. The `db` service:

1. Starts PostgreSQL 16.7
2. Creates the database using credentials from `.env`
3. Runs SQL scripts from `docker-entrypoint-initdb.d/` in alphabetical order

### Docker Configuration

```yaml
db:
  image: postgres:16.7
  environment:
    POSTGRES_USER: ${POSTGRES_USER}
    POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
    POSTGRES_DB: ${POSTGRES_DB}
  volumes:
    - db_data:/var/lib/postgresql/data
    - ./docker-entrypoint-initdb.d:/docker-entrypoint-initdb.d
  command: >
    postgres
    -c checkpoint_timeout=600
    -c max_wal_size=4096
    -c synchronous_commit=0
    -c fsync=0
    -c full_page_writes=0
```

Performance flags (optimized for development speed over durability):

- `checkpoint_timeout=600` -- less frequent checkpoints
- `max_wal_size=4096` -- larger WAL before checkpoint
- `synchronous_commit=0` -- async commits
- `fsync=0` -- skip fsync (data loss risk, faster writes)
- `full_page_writes=0` -- skip full-page writes

### Manual Setup

```bash
psql -U postgres -f docker-entrypoint-initdb.d/001-track-commit-timestamp.sql
psql -U postgres -d cpnucleo -f docker-entrypoint-initdb.d/002-database-dump-ddl.sql
```

---

## Initialization Scripts

### `001-track-commit-timestamp.sql`

Enables commit timestamp tracking for the Delta middleware:

```sql
ALTER SYSTEM SET track_commit_timestamp = on;
```

This allows the Delta library to implement HTTP conditional requests based on when data was last modified.

### `002-database-dump-ddl.sql`

Contains the full DDL schema generated from EF Core migrations. Creates all tables, constraints, and indexes idempotently (using `IF NOT EXISTS` checks).

---

## Schema

### Tables

| Table | Primary Key | Key Columns | Foreign Keys |
|-------|------------|-------------|--------------|
| Organizations | Id (uuid) | Name, Description | -- |
| Projects | Id (uuid) | Name | OrganizationId -> Organizations |
| Assignments | Id (uuid) | Name, Description, StartDate, EndDate, AmountHours | ProjectId -> Projects, WorkflowId -> Workflows, UserId -> Users, AssignmentTypeId -> AssignmentTypes |
| AssignmentTypes | Id (uuid) | Name | -- |
| Workflows | Id (uuid) | Name, Order | -- |
| Users | Id (uuid) | Name, Login, Password, Salt | -- |
| Appointments | Id (uuid) | Description, KeepDate, AmountHours | AssignmentId -> Assignments, UserId -> Users |
| Impediments | Id (uuid) | Name | -- |
| AssignmentImpediments | Id (uuid) | Description | AssignmentId -> Assignments, ImpedimentId -> Impediments |
| UserAssignments | Id (uuid) | -- | UserId -> Users, AssignmentId -> Assignments |
| UserProjects | Id (uuid) | -- | UserId -> Users, ProjectId -> Projects |

### Common Columns (all tables)

| Column | Type | Description |
|--------|------|-------------|
| Id | uuid | Primary key (generated via `Guid.CreateVersion7()`) |
| CreatedAt | timestamp with time zone | Record creation time |
| UpdatedAt | timestamp with time zone (nullable) | Last update time |
| DeletedAt | timestamp with time zone (nullable) | Soft delete time |
| Active | boolean | Soft delete flag (`true` = active) |

### Indexes

All tables have indexes on:

- `CreatedAt` -- for Delta middleware timestamp queries
- Foreign key columns -- for join performance

---

## Connection Configuration

### Connection String

Configured via the `DB_CONNECTION_STRING` environment variable:

```
Host=db;Username=postgres;Password=postgres;Database=cpnucleo;Minimum Pool Size=10;Maximum Pool Size=10;Multiplexing=true
```

| Parameter | Value | Purpose |
|-----------|-------|---------|
| Host | `db` (Docker) / `localhost` (local) | Database server |
| Minimum Pool Size | 10 | Pre-allocated connections |
| Maximum Pool Size | 10 | Connection limit |
| Multiplexing | true | Npgsql multiplexing for better throughput |

---

## EF Core (WebApi + IdentityApi)

### ApplicationDbContext

The `ApplicationDbContext` implements `IApplicationDbContext` and provides DbSet properties for all 11 entities. It is registered as a scoped service.

### Migrations

EF Core migrations are maintained in `src/Infrastructure/Migrations/`. The initial migration `20250219224724_InitiaDblMigration` creates the full schema.

For production, migrations are exported as SQL and placed in `docker-entrypoint-initdb.d/` rather than running EF Core migrations at startup.

### Delta Middleware

The [Delta](https://github.com/SimonCropp/Delta) library is integrated for HTTP conditional requests:

```csharp
app.UseDelta(
    getConnection: httpContext => httpContext.RequestServices.GetRequiredService<NpgsqlConnection>());
```

This enables `If-Modified-Since` / `304 Not Modified` responses using PostgreSQL's commit timestamps.

---

## Dapper (GrpcServer)

### Generic Repository

`DapperRepository<T>` provides CRUD operations via raw SQL:

- `GetByIdAsync` -- `SELECT * FROM "Table" WHERE "Id" = @Id AND "Active" = true`
- `GetAllAsync` -- Paginated query with `OFFSET/LIMIT`, configurable sort column with SQL injection protection
- `AddAsync` -- `INSERT INTO ... RETURNING "Id"` with reflection-based column mapping
- `UpdateAsync` -- `UPDATE ... SET ... WHERE "Id" = @Id`
- `DeleteAsync` -- Hard delete (for gRPC operations)
- `ExistsAsync` -- `SELECT EXISTS(...)` check

### Performance Optimizations

- `PropertyInfo[]` cached via `Lazy<>` to avoid repeated reflection
- `HashSet<string>` for O(1) sort column validation
- `Dapper.AOT` for compile-time SQL interception

### Unit of Work

`UnitOfWork` wraps `NpgsqlConnection` and `NpgsqlTransaction`:

```csharp
public interface IUnitOfWork
{
    IRepository<T> GetRepository<T>() where T : BaseEntity;
    Task BeginTransactionAsync();
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
```

### Specialized Repository

`ProjectRepository` implements `IProjectRepository` for project-specific queries that go beyond the generic CRUD operations.

---

## Fake Data Generation

The Infrastructure layer includes a `FakeDataHelper` that uses the Bogus library to generate realistic test data. When `CreateFakeData=true` is set in configuration:

1. Bogus generates fake data for all entities
2. Outputs SQL/CSV dump files
3. Files should be placed in `docker-entrypoint-initdb.d/` for seeding
