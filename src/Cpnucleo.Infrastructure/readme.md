### Install tools:
```
dotnet tool install --global dotnet-ef
```

### Add migration (from root project folder)
```
dotnet ef migrations add xxxxx -p ./src/Cpnucleo.Infrastructure -s ./src/Cpnucleo.GRPC -c 'CpnucleoDbContext'
```

### Apply migration (from root project folder)
```
dotnet ef database update -p ./src/Cpnucleo.Infrastructure -s ./src/Cpnucleo.GRPC -c 'CpnucleoDbContext'
```

### Remove migration (from root project folder)
```
dotnet ef migrations remove -p ./src/Cpnucleo.Infrastructure -s ./src/Cpnucleo.GRPC -c 'CpnucleoDbContext'
```