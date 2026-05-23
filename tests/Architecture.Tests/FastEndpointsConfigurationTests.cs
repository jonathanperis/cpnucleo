namespace Architecture.Tests;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Linq;

public class FastEndpointsConfigurationTests
{
    private static readonly string[] HttpVerbRouteMethods = ["Get", "Post", "Put", "Delete", "Patch"];

    [Fact]
    public void FastEndpointsPackageVersions_ShouldBeAligned()
    {
        var repositoryRoot = GetRepositoryPath(".");
        var fastEndpointsPackageVersions = Directory
            .EnumerateFiles(repositoryRoot, "*.csproj", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
                && !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            .SelectMany(projectPath => XDocument
                .Load(projectPath)
                .Descendants("PackageReference")
                .Where(x => x.Attribute("Include")?.Value.StartsWith("FastEndpoints", StringComparison.Ordinal) == true)
                .Select(x => new
                {
                    ProjectPath = Path.GetRelativePath(repositoryRoot, projectPath),
                    Version = x.Attribute("Version")?.Value
                }))
            .ToArray();

        fastEndpointsPackageVersions.Should().NotBeEmpty();
        fastEndpointsPackageVersions
            .Select(x => x.Version)
            .Distinct()
            .Should()
            .ContainSingle("all FastEndpoints packages should use the same reviewed version: {0}", string.Join(", ", fastEndpointsPackageVersions.Select(x => $"{x.ProjectPath}: {x.Version}")))
            .Which.Should().Be("8.1.0");
    }

    [Fact]
    public void IdentityApi_ShouldRegisterFastEndpointsOnce()
    {
        var program = File.ReadAllText(GetRepositoryPath("src/IdentityApi/Program.cs"));
        var registrationCount = CountInvocationExpressions(program, "AddFastEndpoints");

        registrationCount.Should().Be(1);
    }

    [Theory]
    [InlineData("src/WebApi/Program.cs")]
    [InlineData("src/IdentityApi/Program.cs")]
    public void ApiProjects_ShouldUseGlobalFastEndpointsApiRoutePrefix(string programPath)
    {
        var program = File.ReadAllText(GetRepositoryPath(programPath));

        program.Should().Contain("UseFastEndpoints(c => c.Endpoints.RoutePrefix = \"api\")");
    }

    [Theory]
    [InlineData("src/WebApi/Endpoints")]
    [InlineData("src/IdentityApi/Endpoints")]
    public void FastEndpoints_ShouldNotHardCodeApiRoutePrefix(string endpointsPath)
    {
        var endpointFiles = Directory.GetFiles(GetRepositoryPath(endpointsPath), "*.cs", SearchOption.AllDirectories);
        var filesWithHardCodedPrefix = endpointFiles
            .Where(path => HttpVerbRouteMethods.Any(method => File.ReadAllText(path).Contains($"{method}(\"/api", StringComparison.Ordinal)))
            .Select(path => Path.GetRelativePath(GetRepositoryPath("."), path))
            .ToArray();

        filesWithHardCodedPrefix.Should().BeEmpty();
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

    private static int CountInvocationExpressions(string value, string methodName)
    {
        var root = CSharpSyntaxTree.ParseText(value).GetRoot();

        return root
            .DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .Count(invocation => invocation.Expression switch
            {
                IdentifierNameSyntax identifier => identifier.Identifier.ValueText == methodName,
                MemberAccessExpressionSyntax memberAccess => memberAccess.Name.Identifier.ValueText == methodName,
                _ => false
            });
    }
}
