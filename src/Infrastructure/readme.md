### Install tools:
```
dotnet tool install --global dotnet-ef
```

### Add migration (from root project folder)
```
dotnet ef migrations add xxxxxx -p ./src/Infrastructure -s ./src/WebApi -c 'ApplicationDbContext'
```

### Apply migration (from root project folder) It will Generate SQL Scripts on ./docker-entrypoint-initdb.d folder
```
dotnet ef migrations script --output ./docker-entrypoint-initdb.d/001-database-dump-ddl.sql --idempotent -p ./src/Infrastructure -s ./src/WebApi -c 'ApplicationDbContext'
```

### Remove migration (from root project folder)
```
dotnet ef migrations remove -p ./src/Infrastructure -s ./src/WebApi -c 'ApplicationDbContext'
```