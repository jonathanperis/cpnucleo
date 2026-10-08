# Database

PostgreSQL stores one shared model. REST compares EF Core, an explicit Dapper project repository and a generic Dapper Unit of Work; gRPC uses Dapper. Both use the schema maintained by EF Core migrations.

## Startup and durability

| Configuration | Initialization | Data |
|---|---|---|
| `compose.lab.yaml` | PostgreSQL with commit timestamps enabled; one-shot `migrate` applies migrations | Explicit `seed` command; loopback port 15432 |
| `compose.prod.yaml` | Initial DDL for a fresh volume, then `migrate-cpnucleo` applies pending migrations before APIs start | Persistent volume; no host database port or automatic seed |
| `compose.yaml` + `compose.override.yaml` | Legacy init-directory mount, including the CSV import script, on a fresh volume, then `migrate-cpnucleo` applies pending migrations before APIs start | Legacy load-test dataset; no automatic seed |

Use the lab for a new local environment. PostgreSQL entrypoint scripts run only when initializing an empty data directory and do not upgrade an existing volume; every Compose topology therefore runs the one-shot `--migrate-database` service before the APIs start.

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

Entities share `Id`, `CreatedAt`, nullable `UpdatedAt`/`DeletedAt`, and `Active`. Factories generate UUIDv7 IDs when none is supplied. EF query filters and Dapper reads exclude inactive rows. Normal removal sets `Active=false` and `DeletedAt`; rows are never physically deleted.

The initial migration creates `CreatedAt` and foreign-key indexes. `LoginIntegrity` adds a normalized-active-login index and a trigger that serializes new/changed active logins. It preserves legacy duplicates rather than rewriting accounts; ambiguous logins cannot authenticate.

`RelationshipIntegrity` adds soft-delete aware relationship triggers. An active row may only reference active parents (SQLSTATE 23503, reported as HTTP 400 with the offending field). A parent with active dependent data can't be deactivated (SQLSTATE 23001, HTTP 409). `UserProjects` and `UserAssignments` rows are deactivated together with their user, project or assignment. Child checks take a `FOR SHARE` lock on the parent and deactivation runs under the parent's row lock, so a concurrent insert and removal can't both succeed. Legacy rows that already reference removed parents are only re-checked when their references change or they are reactivated.

`RelationshipRestore` adds the reverse of the cascade: when a user, project or assignment is reactivated (the REST restore endpoints), the `UserProjects` and `UserAssignments` rows that were deactivated together with it (same `DeletedAt`) are reactivated too, as long as their other parent is active. It runs after the parent row changes, so the links' own active-parent checks see the restored parent. Restoring a row whose parent is still removed fails with SQLSTATE 23503 like any other write.

`ListAndSearchIndexes` adds partial indexes on active rows (listing, counting, relationship checks and membership lookups) and `pg_trgm` GIN indexes for the bounded contains-search. The extension is trusted, so the database owner can create it. See the [migration sources](https://github.com/jonathanperis/cpnucleo/tree/main/src/Infrastructure/Migrations) for the authoritative schema.

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

`ApplicationDbContext` exposes eleven entity sets and active-row filters; request-scoped contexts authorize every save through `AccessGuardInterceptor`. The generic `DapperRepository<T>` provides parameterized reads/writes restricted to what the caller may see, bounded search (LIKE wildcards are escaped) and ID queries, canonical persisted-column sorting (credential columns are never sort keys), atomic batch soft deletion and updates that leave lifecycle columns alone. Reflection metadata is cached; navigation properties are excluded from SQL column lists.

`ProjectRepository` handles project-specific operations with the same visibility and write rules. Project batch removal is transactional. Version-aware updates compare `COALESCE(UpdatedAt, CreatedAt)` with the client's observed timestamp; stale writes fail. Unversioned clients retain last-write-wins behavior.

`UnitOfWork` binds repositories to a connection with explicit begin/commit/rollback. Repositories read the transaction when each command runs, so one obtained before `BeginTransactionAsync` still enlists. The Dapper.AOT package is present, but generic reflection-based repositories are not proof of complete AOT compatibility.

## Conditional requests and streams

Delta middleware uses PostgreSQL commit timestamps for conditional HTTP requests. The lab starts PostgreSQL with `track_commit_timestamp=on`; the legacy/production bootstrap includes `001-track-commit-timestamp.sql`.

SSE listings additionally combine per-resource local notifications with a 15-second refresh for external writes; Delta skips SSE requests. `/readyz` checks that PostgreSQL answers and that `__EFMigrationsHistory` contains the newest migration the build ships, so an instance never reports ready on a database the migrator hasn't upgraded. It is not a full schema diff or business-workflow verification.

## Seed and recovery tools

Use the explicit tiny/realistic [lab profiles](../getting-started/#data-profiles). `--reset-lab` is Development-only and drops the configured database. The advanced `--run-fake-data-csv-import` command truncates every table and loads the large demo dataset; it refuses to run in Production. The legacy CSV dump generator for the `compose.yaml` init directory runs only through the Development-only `--generate-legacy-fake-data` command; it is not the normal lab seed path.

Bogus only supplies the volume, relations and dates of that dataset. `DemoWorkspaceNames.sql` (embedded in Infrastructure) then turns it into a coherent workspace: industry companies and their products, a Backlog → Done board, feature/bug/chore tasks with matching time entries and blocker notes, `first.last@company.example` logins, and task schedules whose board column follows their dates. The importer runs it inside its transaction, the legacy init directory runs an identical copy (`004-demo-workspace-names.sql`), the data migration `20261008000100_DemoWorkspaceNames` applies it once to databases seeded earlier (including production, through the one-shot migrator), and [`scripts/apply-demo-workspace-names.sh`](../deployment/#renaming-existing-demo-data) applies it by hand. It only rewrites rows that still carry generated text (Bogus Hacker names and phrases, blank legacy notes, `learner-NNNNNN` logins), never writes Ids, relations, `Active`, `DeletedAt`, `CreatedAt` or `UpdatedAt`, refuses to repeat names when its catalogs are too small, and a second run changes nothing. Logins of the older legacy CSV dataset are Bogus user names and stay unchanged.

The [Learning Lab](../learning-lab/) describes disposable persistence measurements, backup/restore and outbox experiments.
