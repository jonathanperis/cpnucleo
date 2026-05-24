namespace Architecture.Tests;

public class WebApiEndpointSourceTests
{
    [Fact]
    public void WebApi_Response_Dtos_Should_Not_Use_Required_Members()
    {
        var repoRoot = LocateRepositoryRoot();
        var modelFiles = Directory.GetFiles(
            Path.Combine(repoRoot, "src", "WebApi", "Endpoints"),
            "Models.cs",
            SearchOption.AllDirectories);

        var requiredResponseMemberPattern = new System.Text.RegularExpressions.Regex(
            @"public\s+class\s+Response[\s\S]*?\{(?<body>[\s\S]*?)^\}",
            System.Text.RegularExpressions.RegexOptions.Multiline);

        var offenders = modelFiles
            .Select(path => new
            {
                Path = Path.GetRelativePath(repoRoot, path),
                Text = File.ReadAllText(path)
            })
            .SelectMany(file => requiredResponseMemberPattern
                .Matches(file.Text)
                .Cast<System.Text.RegularExpressions.Match>()
                .Where(match => match.Groups["body"].Value.Contains("public required "))
                .Select(_ => file.Path))
            .OrderBy(path => path)
            .ToArray();

        offenders.Should().BeEmpty("FastEndpoints creates response DTOs reflectively before handlers assign properties, and required response members make that instantiation fail at runtime");
    }

    [Fact]
    public void WebApi_Endpoints_Should_Not_Pass_CancellationToken_As_FindAsync_Key()
    {
        var repoRoot = LocateRepositoryRoot();
        var endpointFiles = Directory.GetFiles(
            Path.Combine(repoRoot, "src", "WebApi", "Endpoints"),
            "*.cs",
            SearchOption.AllDirectories);

        var offenders = endpointFiles
            .Select(path => new
            {
                Path = Path.GetRelativePath(repoRoot, path),
                Text = File.ReadAllText(path)
            })
            .Where(file => file.Text.Contains("FindAsync([") && file.Text.Contains(", cancellationToken]"))
            .Select(file => file.Path)
            .OrderBy(path => path)
            .ToArray();

        offenders.Should().BeEmpty("EF Core FindAsync key arrays must contain only entity key values; the cancellation token belongs only in the named cancellationToken argument");
    }

    private static string LocateRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "cpnucleo.slnx")))
        {
            directory = directory.Parent;
        }

        directory.Should().NotBeNull("the tests should run from inside the cpnucleo repository");
        return directory!.FullName;
    }
}
