### Install tools:
```
dotnet tool install --global dotnet-ef
```

### Add migration (from root project folder)
```
dotnet ef migrations add xxxxx -p ./src/Infrastructure -s ./src/WebApi -c 'ApplicationDbContext'
```

### Apply migration (from root project folder)
```
dotnet ef database update -p ./src/Infrastructure -s ./src/WebApi -c 'ApplicationDbContext'
```

### Remove migration (from root project folder)
```
dotnet ef migrations remove -p ./src/Infrastructure -s ./src/WebApi -c 'ApplicationDbContext'
```