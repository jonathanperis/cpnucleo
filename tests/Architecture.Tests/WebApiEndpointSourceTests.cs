namespace Architecture.Tests;

public class WebApiEndpointSourceTests
{
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
