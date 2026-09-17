# Architecture tests

The xUnit suite combines NetArchTest/reflection checks against explicitly referenced production assemblies with targeted source/configuration checks. Missing assemblies must fail rather than silently validating an empty selection.

| Source | Scope |
|---|---|
| `ArchitectureTests.cs` | Forbidden dependencies, sealed entities, domain repository ports and naming |
| `WebApiEndpointSourceTests.cs` | Endpoint implementation conventions |
| `DapperRepositorySourceTests.cs` | SQL construction, canonical sorting and repository structure |
| `FastEndpointsConfigurationTests.cs` | Host, frontend, deployment and workflow contracts |
| `TenantFoundationTests.cs` | Tenant types/claims as a foundation, not isolated data |
| `FakeDataSeedingTests.cs` | Explicit seed/import structure and generation contracts |

Application depends on Domain; Infrastructure references both. Hosts compose use cases and persistence while WebApi and GrpcServer remain independent. Structural checks do not prove business behavior or imply that every endpoint uses Application.

Run from the repository root:

```sh
dotnet test tests/Architecture.Tests/
```

This gate must pass before commits. Runner output supplies test totals. Add rules for material boundaries, load actual target assemblies and make empty selections fail. Pair source checks with observable behavior; avoid assertions tied only to formatting or frozen warning counts.

See the [testing guide](../../docs/wiki/testing.md). Versions live in `Architecture.Tests.csproj`.
