using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebApi.Integration.Tests;

/// <summary>
/// The OpenAPI documents are the published HTTP contract: routes, query keys, envelopes and statuses.
/// Each must match its committed snapshot in docs/openapi, so a contract change is a reviewed diff.
/// Run with UPDATE_OPENAPI_SNAPSHOTS=1 to rewrite the snapshots after an intended change.
/// </summary>
[Collection("Database")]
public class OpenApiSnapshotTests(WebAppFixture app)
{
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    [Theory]
    [InlineData("WebApi", "webapi")]
    [InlineData("IdentityApi", "identityapi")]
    public async Task OpenApiDocument_MatchesTheCommittedSnapshot(string host, string snapshot)
    {
        var document = JsonNode.Parse(await FetchFromStandaloneHostAsync(host))!.AsObject();
        document.Remove("servers"); // Depends on the host's address, not on the contract.
        var actual = document.ToJsonString(new JsonSerializerOptions { WriteIndented = true }).ReplaceLineEndings("\n") + "\n";

        var path = Path.Join(RepositoryRoot(), "docs", "openapi", $"{snapshot}.v1.json");
        if (Environment.GetEnvironmentVariable("UPDATE_OPENAPI_SNAPSHOTS") == "1")
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            await File.WriteAllTextAsync(path, actual, Cancellation);
            return;
        }

        File.Exists(path).ShouldBeTrue($"Missing {path}. Run the tests with UPDATE_OPENAPI_SNAPSHOTS=1 to create it.");
        (await File.ReadAllTextAsync(path, Cancellation)).ReplaceLineEndings("\n").ShouldBe(actual,
            $"The {host} OpenAPI document changed. If the change is intended, run the tests with UPDATE_OPENAPI_SNAPSHOTS=1 and commit docs/openapi.");
    }

    /// <summary>
    /// Starts the host in its own process, as a deployment does. FastEndpoints applies some settings
    /// once per process, so a document generated inside the shared multi-host test process differs.
    /// </summary>
    private async Task<string> FetchFromStandaloneHostAsync(string host)
    {
        var port = FreePort();
        // The host's own build output (same configuration as this test assembly): the test output mixes
        // every referenced host's appsettings.json.
        var configuration = new DirectoryInfo(AppContext.BaseDirectory).Parent!.Name;
        var output = Path.Join(RepositoryRoot(), "src", host, "bin", configuration, new DirectoryInfo(AppContext.BaseDirectory).Name);
        var start = new ProcessStartInfo(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet")
        {
            WorkingDirectory = output,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        start.ArgumentList.Add(Path.Join(output, $"{host}.dll"));
        start.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";
        start.Environment["ASPNETCORE_URLS"] = $"http://127.0.0.1:{port}";
        start.Environment["DB_CONNECTION_STRING"] = app.ConnectionString;
        // IdentityApi reads its stored keys from the shared test database, encrypted with this secret.
        start.Environment["Identity__KeyEncryptionSecret"] = WebAppFixture.KeyEncryptionSecret;
        start.Environment["Jwt__Issuer"] = WebAppFixture.Issuer;
        start.Environment["Jwt__MetadataAddress"] = "http://127.0.0.1:1/.well-known/openid-configuration";

        var log = new System.Collections.Concurrent.ConcurrentQueue<string>();
        using var process = new Process { StartInfo = start };
        process.OutputDataReceived += (_, line) => { if (line.Data is not null) log.Enqueue(line.Data); };
        process.ErrorDataReceived += (_, line) => { if (line.Data is not null) log.Enqueue(line.Data); };
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        try
        {
            using var client = new HttpClient { BaseAddress = new Uri($"http://127.0.0.1:{port}"), Timeout = TimeSpan.FromSeconds(10) };
            for (var attempt = 0; attempt < 120 && !process.HasExited; attempt++)
            {
                try
                {
                    var response = await client.GetAsync("/swagger/v1/swagger.json", Cancellation);
                    if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync(Cancellation);
                }
                catch (HttpRequestException)
                {
                    // Not listening yet.
                }
                await Task.Delay(250, Cancellation);
            }
            throw new InvalidOperationException($"{host} did not serve its OpenAPI document:\n{string.Join('\n', log)}");
        }
        finally
        {
            if (!process.HasExited) process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync(Cancellation);
        }
    }

    private static int FreePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }

    private static string RepositoryRoot()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
            if (File.Exists(Path.Join(directory.FullName, "cpnucleo.slnx"))) return directory.FullName;
        throw new InvalidOperationException("Repository root (cpnucleo.slnx) not found.");
    }
}
