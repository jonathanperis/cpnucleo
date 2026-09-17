# Database

PostgreSQL stores one shared model. REST compares EF Core, an explicit Dapper project repository and a generic Dapper Unit of Work; gRPC uses Dapper. Both use the schema maintained by EF Core migrations.

## Startup and durability

| Configuration | Initialization | Data |
|---|---|---|
| `compose.lab.yaml` | PostgreSQL with commit timestamps enabled; one-shot `migrate` applies migrations | Explicit `seed` command; loopback port 15432 |
| `compose.prod.yaml` | Initial DDL for a fresh volume, then `migrate-cpnucleo` applies pending migrations before APIs start | Persistent volume; no host database port or automatic seed |
| `compose.yaml` + `compose.override.yaml` | Legacy init-directory mount, including the CSV import script, on a fresh volume | Legacy load-test dataset; no one-shot migration service |

Use the lab for a new local environment. The legacy topology requires an explicit migration step for an existing database; PostgreSQL entrypoint scripts run only when initializing an empty data directory. They do not upgrade an existing volume.

Lab, default and production configurations keep PostgreSQL durability enabled. Checkpoint/WAL tuning in the Compose files does not disable `fsync`, `synchronous_commit` or `full_page_writes`.

## Schema and lifecycle

| Table | Main fields | Relationships |
|---|---|---|
| Organizations | Name, Description | — |
| Projects | Name | Organization |
| Assignments | Name, Description, StartDate, EndDate, AmountHours | Project, Workflow, User, AssignmentType |
| AssignmentTypes | Name | — |
| Workflows | Name, Order | — |
| Users | Name, Login, Password (hash), Salt (legacy column) | — |
| Appointments | Description, KeepDate, AmountHours | Assignment, User |
| Impediments | Name | — |
| AssignmentImpediments | Description | Assignment, Impediment |
| UserAssignments | — | User, Assignment |
| UserProjects | — | User, Project |

Entities share `Id`, `CreatedAt`, nullable `UpdatedAt`/`DeletedAt`, and `Active`. Factories generate UUIDv7 IDs when none is supplied. EF query filters and Dapper reads exclude inactive rows. Normal removal sets `Active=false` and `DeletedAt`; it preserves rows and relationships rather than physically deleting them or automatically archiving children.

The initial migration creates `CreatedAt` and foreign-key indexes. `LoginIntegrity` adds a normalized-active-login index and a trigger that serializes new/changed active logins. It preserves legacy duplicates rather than rewriting accounts; ambiguous logins cannot authenticate. See the [migration sources](https://github.com/jonathanperis/cpnucleo/tree/main/src/Infrastructure/Migrations) for the authoritative schema.

## Connection configuration

`DB_CONNECTION_STRING` is consumed by both EF Core and Dapper. A host process using the disposable lab database can use:

```text
Host=localhost;Port=15432;Database=cpnucleo_lab;Username=learner;Password=disposable-lab-only;Maximum Pool Size=20
```

Containers use `Host=db` and internal port 5432. Pool sizes and multiplexing are configuration choices, not guarantees required by the application. Production credentials belong in deployment configuration.

## Migrations

Run from the repository root with the intended database configured:

```sh
dotnet ef migrations add DescriptiveName -p ./src/Infrastructure -s ./src/WebApi -c ApplicationDbContext
dotnet ef database update -p ./src/Infrastructure -s ./src/WebApi -c ApplicationDbContext
```

The one-shot deployment command is `WebApi --migrate-database`; it calls `MigrateAsync` and exits. Exporting an idempotent SQL script is a separate operation and does not apply it. See the [Infrastructure README](https://github.com/jonathanperis/cpnucleo/blob/main/src/Infrastructure/readme.md) for tooling and export commands.

## EF Core, Dapper and concurrency

`ApplicationDbContext` exposes eleven entity sets and active-row filters. The generic `DapperRepository<T>` provides parameterized reads/writes, bounded search/ID queries, canonical persisted-column sorting and soft deletion. Reflection metadata is cached; navigation properties are excluded from SQL column lists.

`ProjectRepository` handles project-specific operations. Project batch removal is transactional. Version-aware updates compare `COALESCE(UpdatedAt, CreatedAt)` with the client's observed timestamp; stale writes fail. Unversioned clients retain last-write-wins behavior.

`UnitOfWork` binds repositories to a connection and optional transaction with explicit begin/commit/rollback. The Dapper.AOT package is present, but generic reflection-based repositories are not proof of complete AOT compatibility.

## Conditional requests and streams

Delta middleware uses PostgreSQL commit timestamps for conditional HTTP requests. The lab starts PostgreSQL with `track_commit_timestamp=on`; the legacy/production bootstrap includes `001-track-commit-timestamp.sql`.

SSE listings additionally combine local notifications with a 15-second refresh for external writes. `/readyz` checks a PostgreSQL connection and a query against `Users`; it is not a full schema diff or business-workflow verification.

## Seed and recovery tools

Use the explicit tiny/realistic [lab profiles](../getting-started/#data-profiles). `--reset-lab` is Development-only and drops the configured database. The advanced `--run-fake-data-csv-import` command replaces demo data and is not part of production startup. The legacy `CreateFakeData=true` option generates dump files; it is not the normal lab seed path and has no environment guard of its own.

The [Learning Lab](../learning-lab/) describes disposable persistence measurements, backup/restore and outbox experiments.
