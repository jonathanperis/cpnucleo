using System.Reflection;

namespace Architecture.Tests;

public class DapperRepositorySourceTests
{
    [Fact]
    public void ProjectRepository_UpdateAsync_Should_Persist_Soft_Delete_Columns()
    {
        var projectRepository = File.ReadAllText(GetRepositoryPath("src/Infrastructure/Repositories/ProjectRepository.cs"));

        projectRepository.Should().Contain("\"Active\" = @Active", "Project.Remove uses Active=false and the repository must persist it");
        projectRepository.Should().Contain("\"DeletedAt\" = @DeletedAt", "soft-deleted projects need a persisted deletion timestamp");
    }

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

    private static string GetRepositoryPath(string relativePath)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "cpnucleo.slnx")))
        {
            directory = directory.Parent;
        }

        directory.Should().NotBeNull("the test should run from inside the cpnucleo repository output tree");

        return Path.Combine(directory!.FullName, relativePath);
    }
}
