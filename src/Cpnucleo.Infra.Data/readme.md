### Install tools:
```
dotnet tool install --global dotnet-ef
```

### Add migration
```
dotnet ef migrations add xxxxx 
```

### Apply migration
```
dotnet ef database update
```

### Remove migration
```
dotnet ef migrations remove
```