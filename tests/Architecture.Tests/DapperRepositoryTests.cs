using System.Reflection;

namespace Architecture.Tests;

public class DapperRepositoryTests
{
    public static TheoryData<Type> Entities => new(typeof(Domain.Entities.BaseEntity).Assembly.GetTypes()
        .Where(type => type.Namespace == "Domain.Entities" && !type.IsAbstract && type != typeof(Domain.Entities.Tenant)));

    [Theory]
    [MemberData(nameof(Entities))]
    public void Updates_ShouldNeverWriteIdentityOrSoftDeleteColumns(Type entity)
    {
        // An UPDATE that re-writes Active/DeletedAt from a stale read would resurrect a row removed
        // concurrently; identity columns must never change after insert.
        var assignments = (string)RepositoryType(entity)
            .GetMethod("GetUpdateAssignments", BindingFlags.NonPublic | BindingFlags.Static)!
            .Invoke(null, null)!;

        foreach (var column in new[] { "Id", "CreatedAt", "Active", "DeletedAt" })
            assignments.Should().NotContain($"\"{column}\" =", $"{entity.Name} updates must not write {column}");
        assignments.Should().Contain("\"UpdatedAt\" = GREATEST(", "versions must only move forward");
    }

    [Theory]
    [MemberData(nameof(Entities))]
    public void Columns_ShouldExcludeNavigationProperties(Type entity)
    {
        var columns = (string)RepositoryType(entity)
            .GetMethod("GetColumns", BindingFlags.NonPublic | BindingFlags.Static)!
            .Invoke(null, [false])!;

        foreach (var navigation in entity.GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(Domain.Entities.BaseEntity))))
            columns.Should().NotContain($"\"{navigation.Name}\"", "navigation objects are not database columns");
    }

    [Theory]
    [MemberData(nameof(Entities))]
    public void EveryEntity_ShouldHaveAnAccessRule(Type entity)
    {
        var act = () => Application.Common.Security.ResourceAccess.KindOf(entity);
        act.Should().NotThrow("every resource must declare how reads and writes are authorized");
    }

    private static Type RepositoryType(Type entity) =>
        typeof(Infrastructure.Repositories.DapperRepository<>).MakeGenericType(entity);
}
