namespace Architecture.Tests;

using System.Xml.Linq;

public class FastEndpointsConfigurationTests
{
    [Theory]
    [InlineData("src/WebApi/WebApi.csproj")]
    [InlineData("src/IdentityApi/IdentityApi.csproj")]
    [InlineData("tests/WebApi.Integration.Tests/WebApi.Integration.Tests.csproj")]
    public void FastEndpointsPackageVersions_ShouldBeAligned(string projectPath)
    {
        var project = XDocument.Load(GetRepositoryPath(projectPath));
        var fastEndpointsPackageVersions = project
            .Descendants("PackageReference")
            .Where(x => x.Attribute("Include")?.Value.StartsWith("FastEndpoints", StringComparison.Ordinal) == true)
            .Select(x => x.Attribute("Version")!.Value)
            .Distinct()
            .ToArray();

        fastEndpointsPackageVersions.Should().Equal("8.1.0");
    }

    [Fact]
    public void IdentityApi_ShouldRegisterFastEndpointsOnce()
    {
        var program = File.ReadAllText(GetRepositoryPath("src/IdentityApi/Program.cs"));
        var registrationCount = CountOccurrences(program, ".AddFastEndpoints(");

        registrationCount.Should().Be(1);
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

    private static int CountOccurrences(string value, string search)
    {
        var count = 0;
        var index = 0;

        while ((index = value.IndexOf(search, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += search.Length;
        }

        return count;
    }
}
