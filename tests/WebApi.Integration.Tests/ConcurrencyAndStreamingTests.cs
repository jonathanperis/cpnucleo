using System.Net.Http.Json;
using Dapper;
using Domain.Entities;
using Infrastructure.Repositories;
using Npgsql;

namespace WebApi.Integration.Tests;

[Collection("Database")]
public class ConcurrencyAndStreamingTests(WebAppFixture app)
{
    [Fact]
    public async Task Sse_ClosesWhenItsAccessTokenExpires()
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(8));
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/projects?pageSize=1");
        request.Headers.Accept.ParseAdd("text/event-stream");
        request.Headers.Authorization = new("Bearer", WebAppFixture.CreateToken(expiresAt: DateTime.UtcNow.AddSeconds(2)));
        using var response = await app.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeout.Token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync(timeout.Token);
        body.ShouldContain("data:");
    }

    private async Task<Project> CreateProjectAsync()
    {
        await using var context = app.CreateDbContext();
        var organization = Organization.Create("Concurrency lab", "");
        var project = Project.Create($"Lab-{Guid.NewGuid():N}", organization.Id);
        context.AddRange(organization, project);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        await using var connection = new NpgsqlConnection(app.ConnectionString);
        return (await new ProjectRepository(connection).GetByIdAsync(project.Id))!;
    }

    [Fact]
    public async Task StaleProjectEdit_ReturnsConflictAndPreservesTheFirstWriter()
    {
        var ct = TestContext.Current.CancellationToken;
        var project = await CreateProjectAsync();
        var body = new { project.Id, name = "First writer", project.OrganizationId, expectedVersion = project.CreatedAt };
        (await app.Client.PatchAsJsonAsync("/api/project", body, ct)).StatusCode.ShouldBe(HttpStatusCode.OK);
        var second = new { project.Id, name = "Stale writer", project.OrganizationId, expectedVersion = project.CreatedAt };
        (await app.Client.PatchAsJsonAsync("/api/project", second, ct)).StatusCode.ShouldBe(HttpStatusCode.Conflict);
        await using var connection = new NpgsqlConnection(app.ConnectionString);
        (await new ProjectRepository(connection).GetByIdAsync(project.Id))!.Name.ShouldBe("First writer");
    }

    [Fact]
    public async Task Sse_RefreshesAnExternalWriteWithoutAProcessLocalNotification()
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(25));
        var project = await CreateProjectAsync();
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/projects?ids={project.Id}");
        request.Headers.Accept.ParseAdd("text/event-stream");
        using var response = await app.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeout.Token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        using var reader = new StreamReader(await response.Content.ReadAsStreamAsync(timeout.Token));
        async Task<string> NextData()
        {
            while (await reader.ReadLineAsync(timeout.Token) is { } line)
                if (line.StartsWith("data:", StringComparison.Ordinal)) return line;
            throw new InvalidOperationException("Stream ended before a snapshot.");
        }
        (await NextData()).ShouldContain(project.Name!);
        await using var connection = new NpgsqlConnection(app.ConnectionString);
        await connection.ExecuteAsync("UPDATE \"Projects\" SET \"Name\" = 'External writer' WHERE \"Id\" = @id", new { id = project.Id });
        (await NextData()).ShouldContain("External writer");
    }
}
