using System.Reflection;

namespace Architecture.Tests;

public class DapperRepositorySourceTests
{
    [Fact]
    public void DapperRepository_Should_Not_Treat_Navigation_Properties_As_Table_Columns()
    {
        var repositoryType = typeof(Infrastructure.Repositories.DapperRepository<Domain.Entities.Project>);
        var getColumns = repositoryType.GetMethod("GetColumns", BindingFlags.NonPublic | BindingFlags.Static);

        getColumns.Should().NotBeNull("the Dapper repository column selection helper should exist");
        var columns = (string)getColumns!.Invoke(null, [false])!;

        columns.Should().Contain("\"OrganizationId\"");
        columns.Should().NotContain("\"Organization\"", "navigation objects are not database columns and break project seeding through the public API");
    }
}
