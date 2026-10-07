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
        timeout.CancelAfter(TimeSpan.FromSeconds(10));
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/projects?pageSize=1");
        request.Headers.Accept.ParseAdd("text/event-stream");
        // JWT exp has one-second resolution: a 3-second token lives at least 2 seconds.
        request.Headers.Authorization = new("Bearer", WebAppFixture.CreateToken(expiresAt: DateTime.UtcNow.AddSeconds(3)));
        var started = System.Diagnostics.Stopwatch.StartNew();
        using var response = await app.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeout.Token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync(timeout.Token);
        body.ShouldContain("data:");
        // It stayed open until expiry instead of ending after the first snapshot.
        started.Elapsed.ShouldBeGreaterThan(TimeSpan.FromSeconds(1.5));
    }

    private async Task<Project> CreateProjectAsync()
    {
        await using var context = app.CreateDbContext();
        var organization = Organization.Create("Concurrency lab", "");
        var project = Project.Create($"Lab-{Guid.NewGuid():N}", organization.Id);
        context.AddRange(organization, project);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        await using var connection = new NpgsqlConnection(app.ConnectionString);
        return (await new ProjectRepository(connection, Application.Common.Security.StaticCurrentUser.System, Infrastructure.Security.TrustedAccessGuard.Instance).GetByIdAsync(project.Id, TestContext.Current.CancellationToken))!;
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
        (await new ProjectRepository(connection, Application.Common.Security.StaticCurrentUser.System, Infrastructure.Security.TrustedAccessGuard.Instance).GetByIdAsync(project.Id, TestContext.Current.CancellationToken))!.Name.ShouldBe("First writer");
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

    [Fact]
    public async Task DapperUpdate_RacingARemoval_CannotResurrectTheRow()
    {
        var ct = TestContext.Current.CancellationToken;
        var graph = await app.CreateGraphAsync();
        await using var connection = new NpgsqlConnection(app.ConnectionString);
        using var unitOfWork = new Infrastructure.UoW.UnitOfWork(connection, Application.Common.Security.StaticCurrentUser.System,
            Infrastructure.Security.TrustedAccessGuard.Instance);
        var repository = unitOfWork.GetRepository<Organization>();

        // A handler read the row, then another request removed it before the update ran.
        var stale = (await repository.GetByIdAsync(graph.Organization.Id, ct))!;
        await using (var remover = new NpgsqlConnection(app.ConnectionString))
        {
            await remover.ExecuteAsync("""UPDATE "Assignments" SET "Active" = false, "DeletedAt" = now() WHERE "Id" = @id""", new { id = graph.Assignment.Id });
            await remover.ExecuteAsync("""UPDATE "Projects" SET "Active" = false, "DeletedAt" = now() WHERE "OrganizationId" = @id""", new { id = graph.Organization.Id });
            await remover.ExecuteAsync("""UPDATE "Organizations" SET "Active" = false, "DeletedAt" = now() WHERE "Id" = @id""", new { id = graph.Organization.Id });
        }

        Organization.Update(stale, "Late edit", "x");
        (await repository.UpdateAsync(stale, ct)).ShouldBeFalse();
        (await app.RowStateAsync("Organizations", graph.Organization.Id)).ShouldBe((false, true), "the removal wins; the stale update must not reactivate it");
    }

    [Fact]
    public async Task ProjectVersions_BehaveTheSameOverGrpc()
    {
        var project = await CreateProjectAsync();
        var options = WebAppFixture.GrpcOptions();

        var first = await new GrpcServer.Contracts.Commands.Project.UpdateProjectCommand
        {
            Id = project.Id, Name = "First writer", OrganizationId = project.OrganizationId, ExpectedVersion = project.CreatedAt
        }.RemoteExecuteAsync(options);
        first.Success.ShouldBeTrue();

        var stale = await new GrpcServer.Contracts.Commands.Project.UpdateProjectCommand
        {
            Id = project.Id, Name = "Stale writer", OrganizationId = project.OrganizationId, ExpectedVersion = project.CreatedAt
        }.RemoteExecuteAsync(options);
        stale.Success.ShouldBeFalse();
        stale.Message.ShouldContain("changed");

        // The next version is the stored UpdatedAt, read back exactly as the API returns it.
        var current = (await new GrpcServer.Contracts.Commands.Project.GetProjectByIdCommand { Id = project.Id }.RemoteExecuteAsync(options)).Project!;
        current.UpdatedAt.ShouldNotBeNull();
        (await new GrpcServer.Contracts.Commands.Project.UpdateProjectCommand
        {
            Id = project.Id, Name = "Second writer", OrganizationId = project.OrganizationId, ExpectedVersion = current.UpdatedAt
        }.RemoteExecuteAsync(options)).Success.ShouldBeTrue();
    }

    [Fact]
    public async Task ProjectEdits_WithoutAVersion_KeepLastWriteWins()
    {
        var ct = TestContext.Current.CancellationToken;
        var project = await CreateProjectAsync();

        (await app.Client.PatchAsJsonAsync("/api/project", new { project.Id, name = "First", project.OrganizationId }, ct)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await app.Client.PatchAsJsonAsync("/api/project", new { project.Id, name = "Second", project.OrganizationId }, ct)).StatusCode.ShouldBe(HttpStatusCode.OK);

        var current = await app.Client.GetFromJsonAsync<System.Text.Json.JsonElement>($"/api/project?id={project.Id}", ct);
        current.GetProperty("project").GetProperty("name").GetString().ShouldBe("Second");
        var updatedAt = current.GetProperty("project").GetProperty("updatedAt").GetDateTime();
        (await app.Client.PatchAsJsonAsync("/api/project", new { project.Id, name = "Versioned", project.OrganizationId, expectedVersion = updatedAt }, ct))
            .StatusCode.ShouldBe(HttpStatusCode.OK, "the UpdatedAt returned by the API is a valid version");
    }
}
