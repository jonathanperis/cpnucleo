# WebApi Unit Tests

This directory contains unit tests for the WebApi endpoints using FastEndpoints testing framework.

## Test Coverage

### Fully Tested Endpoints (Repository-based)
- **Project** - 9 tests (GetById, Create, Update, Delete, List) ✅
- **Organization** - 6 tests (GetById, Create, Update, Delete, List) ✅

### Partially Tested Endpoints (EF Core-based)
- **User** - 6 tests (GetById, List work; Create, Update, Delete need EF Core DbSet mock fixes)
- **Workflow** - 6 tests (GetById, List work; Create, Update, Delete need EF Core DbSet mock fixes)

## Testing Patterns

### Repository-Based Endpoints
These endpoints use either `IProjectRepository` or `IUnitOfWork` with `IRepository<T>`. They're straightforward to test with FakeItEasy mocks.

Example:
```csharp
var fakeRepository = A.Fake<IProjectRepository>();
A.CallTo(() => fakeRepository.GetByIdAsync(id)).Returns(Task.FromResult<Project?>(project));
var ep = Factory.Create<Endpoint>(fakeRepository);
```

### EF Core-Based Endpoints
These endpoints use `IApplicationDbContext` directly. Mocking DbSet<T> requires more complex setup and is prone to issues with FakeItEasy due to how EF Core DbSet works.

Example (partial - needs refinement):
```csharp
var fakeDbContext = A.Fake<IApplicationDbContext>();
var fakeDbSet = A.Fake<DbSet<Entity>>();
A.CallTo(() => fakeDbContext.Entities).Returns(fakeDbSet);
// Additional setup needed for DbSet operations
```

## Test Statistics

- **Total Test Files**: 4
- **Total Tests Written**: 27
- **Currently Passing**: 21
- **Currently Failing**: 6 (EF Core-based CUD operations)

## Dependencies

- FastEndpoints 7.0.1
- FakeItEasy 8.3.0
- Shouldly 4.2.1
- NUnit 4.2.2

## Running Tests

```bash
dotnet test test/WebApi.Unit.Tests/WebApi.Unit.Tests.csproj
```

## Next Steps

To achieve full test coverage:
1. Fix EF Core DbSet mocking for User and Workflow Create/Update/Delete operations
2. Add tests for remaining endpoints: Assignment, AssignmentType, Impediment, Appointment, UserProject, UserAssignment, AssignmentImpediment
3. Consider using integration tests for EF Core-based endpoints as an alternative to complex mocking
