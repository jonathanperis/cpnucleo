# Infrastructure and migrations

Infrastructure contains EF Core, Dapper repositories/Unit of Work, hashing, migrations and seed tools. Both transports share the PostgreSQL schema.

Install `dotnet-ef` matching the EF Core major version in `Infrastructure.csproj`. From the repository root, configure `DB_CONNECTION_STRING` for the intended database:

```sh
dotnet ef migrations add DescriptiveName -p ./src/Infrastructure -s ./src/WebApi -c ApplicationDbContext
dotnet ef database update -p ./src/Infrastructure -s ./src/WebApi -c ApplicationDbContext
```

To **export**, rather than apply, idempotent SQL:

```sh
dotnet ef migrations script --output ./docker-entrypoint-initdb.d/002-database-dump-ddl.sql --idempotent -p ./src/Infrastructure -s ./src/WebApi -c ApplicationDbContext
```

Review the generated diff. `001-track-commit-timestamp.sql` is a separate server setting. Production embeds bootstrap DDL in `compose.prod.yaml`; its one-shot `--migrate-database` service applies later migrations to existing volumes.

Remove only an unapplied development migration:

```sh
dotnet ef migrations remove -p ./src/Infrastructure -s ./src/WebApi -c ApplicationDbContext
```

See [Database](../../docs/wiki/database.md) and [Getting Started](../../docs/wiki/getting-started.md) for connections and explicit lab seeding. Production never invokes the CSV importer automatically.
