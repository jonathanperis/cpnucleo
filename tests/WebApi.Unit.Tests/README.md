# WebApi unit tests

NUnit tests exercise endpoint orchestration using FastEndpoints `Factory`, FakeItEasy repository/use-case doubles, and EF Core InMemory where appropriate. `ListingChangeNotifierTests` covers local subscriber notifications.

Run from the repository root:

```sh
dotnet test tests/WebApi.Unit.Tests/
```

These tests cover selected reads, lists, writes and failure paths. They do not prove PostgreSQL constraints, transactions, HTTP binding or gRPC serialization. Test totals and pass/fail status come from the runner.

User and workflow creation are covered by the independent PostgreSQL CRUD theory in `WebApi.Integration.Tests/CrudContractTests.cs`; obsolete skipped `DbSet.Any()` mock tests have been removed. Do not mock LINQ extension methods to prove database behavior.

Read `Endpoints/ProjectEndpointsTests.cs` for repository/shared-use-case orchestration, `Endpoints/ImpedimentEndpointsTests.cs` for EF InMemory and `Common/Services/ListingChangeNotifierTests.cs` for notifications. Versions live in `WebApi.Unit.Tests.csproj`.

See the [testing guide](../../docs/wiki/testing.md) for all suites.
