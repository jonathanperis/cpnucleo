namespace Architecture.Tests;

public class FakeDataSeedingTests
{
    [Fact]
    public void FakeDataSeed_ShouldPreserveLoginPageDemoUserAndCleanExistingData()
    {
        var fakeData = File.ReadAllText(GetRepositoryPath("src/Infrastructure/Common/Helpers/FakeData.cs"));
        var loginForm = File.ReadAllText(GetRepositoryPath("src/WebClient/src/routes/login/login-form.ts"));

        loginForm.Should().Contain("DEMO_LOGIN = 'demo@cpnucleo.local'");
        loginForm.Should().Contain("DEMO_PASSWORD = 'CpnucleoDemo2026!'");
        fakeData.Should().Contain("DefaultDemoLogin = \"demo@cpnucleo.local\"");
        fakeData.Should().Contain("DefaultDemoPassword = \"CpnucleoDemo2026!\"");
        fakeData.Should().Contain("AppendDatabaseResetAndDefaultUserSql(sb, defaultDemoPasswordHash)");
        fakeData.Should().Contain("DELETE FROM \"Users\" WHERE \"Login\" <> '{DefaultDemoLogin}'");
        fakeData.Should().Contain("WHERE NOT EXISTS (SELECT 1 FROM \"Users\" WHERE \"Login\" = '{DefaultDemoLogin}')");
    }

    [Fact]
    public void FakeDataSeed_ShouldCleanDependentTablesBeforeCopyingNewFakeRows()
    {
        var fakeData = File.ReadAllText(GetRepositoryPath("src/Infrastructure/Common/Helpers/FakeData.cs"));
        var resetIndex = fakeData.IndexOf("AppendDatabaseResetAndDefaultUserSql(sb, defaultDemoPasswordHash)", StringComparison.Ordinal);
        var firstCopyIndex = fakeData.IndexOf("COPY \"Organizations\"", StringComparison.Ordinal);

        resetIndex.Should().BeGreaterThanOrEqualTo(0);
        firstCopyIndex.Should().BeGreaterThan(resetIndex, "the database must be cleaned before fake seed rows are copied");
        fakeData.Should().Contain("TRUNCATE TABLE");
        fakeData.Should().Contain("\"Appointments\"");
        fakeData.Should().Contain("\"AssignmentImpediments\"");
        fakeData.Should().Contain("\"UserAssignments\"");
        fakeData.Should().Contain("\"Assignments\"");
        fakeData.Should().Contain("\"UserProjects\"");
        fakeData.Should().Contain("\"Projects\"");
        fakeData.Should().Contain("\"Organizations\"");
        fakeData.Should().Contain("\"Workflows\"");
        fakeData.Should().Contain("\"AssignmentTypes\"");
        fakeData.Should().Contain("\"Impediments\"");
    }

    [Fact]
    public void ProductionCsvSeeder_ShouldUseCanonicalFakeDataCountsAndOneShotContainer()
    {
        var importer = File.ReadAllText(GetRepositoryPath("src/Infrastructure/Common/Helpers/FakeDataCsvImporter.cs"));
        var program = File.ReadAllText(GetRepositoryPath("src/WebApi/Program.cs"));
        var compose = File.ReadAllText(GetRepositoryPath("compose.prod.yaml"));

        importer.Should().Contain("private const int OrganizationCount = 686");
        importer.Should().Contain("private const int ProjectCount = 1_258");
        importer.Should().Contain("private const int UserCount = 11_154");
        importer.Should().Contain("private const int AssignmentCount = 464_587");
        importer.Should().Contain("private const int UserAssignmentCount = 363_554");
        importer.Should().Contain("private const int AppointmentCount = 489_571");
        importer.Should().NotContain("faker.Random.Bool()");
        importer.Should().Contain("FROM STDIN WITH (FORMAT CSV)");
        importer.Should().Contain("__FakeDataCsvImports");
        importer.Should().Contain("TRUNCATE TABLE");
        importer.Should().Contain("DefaultDemoLogin = \"demo@cpnucleo.local\"");

        program.Should().Contain("--run-fake-data-csv-import");
        program.Should().Contain("FakeDataCsvImporter.RunAsync");

        compose.Should().Contain("seed-csv-cpnucleo:");
        compose.Should().Contain("command: [\"--run-fake-data-csv-import\"]");
        compose.Should().Contain("restart: \"no\"");
    }

    private static string GetRepositoryPath(string relativePath)
    {
        var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);
        while (currentDirectory is not null && !File.Exists(Path.Combine(currentDirectory.FullName, "cpnucleo.slnx")))
        {
            currentDirectory = currentDirectory.Parent;
        }

        currentDirectory.Should().NotBeNull("tests should run inside the repository checkout");
        return Path.Combine(currentDirectory!.FullName, relativePath);
    }
}
