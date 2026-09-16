using System.Net.Http.Json;
using Dapper;
using Domain.Entities;
using GrpcServer.Contracts.Commands.Project;
using Infrastructure.Repositories;
using Npgsql;

namespace WebApi.Integration.Tests;

[Collection("Database")]
public class PersistenceParityTests(WebAppFixture app)
{
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    [Fact]
    public async Task FailedExplicitSeed_RollsBackItsReset()
    {
        var organization = Organization.Create("Preserved after seed failure", "");
        await using (var db = app.CreateDbContext())
        {
            db.Add(organization);
            await db.SaveChangesAsync(Cancellation);
        }
        var previous = Environment.GetEnvironmentVariable("CPNUCLEO_DEMO_PASSWORD");
        try
        {
            Environment.SetEnvironmentVariable("CPNUCLEO_DEMO_PASSWORD", null);
            await Should.ThrowAsync<InvalidOperationException>(() => Infrastructure.Common.Helpers.FakeDataCsvImporter.RunAsync(
                app.ConnectionString, Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance, Cancellation));
        }
        finally { Environment.SetEnvironmentVariable("CPNUCLEO_DEMO_PASSWORD", previous); }
        await using var connection = new NpgsqlConnection(app.ConnectionString);
        (await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM \"Organizations\" WHERE \"Id\" = @id", new { id = organization.Id })).ShouldBe(1);
    }
    [Fact]
    public async Task RestCreate_GrpcReadAndRemove_PreserveRowsAndRelationships()
    {
        await using var connection = new NpgsqlConnection(app.ConnectionString);
        var organization = Organization.Create("Parity organization", "");
        await using (var context = app.CreateDbContext())
        {
            context.Organizations!.Add(organization);
            await context.SaveChangesAsync(Cancellation);
        }
        var id = Guid.NewGuid();
        var response = await app.Client.PostAsJsonAsync("/api/project", new { id, name = "Parity project", organizationId = organization.Id }, Cancellation);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        (await new GetProjectByIdCommand { Id = id }.RemoteExecuteAsync(WebAppFixture.GrpcOptions())).Project!.Id.ShouldBe(id);
        (await new RemoveProjectCommand { Ids = [id] }.RemoteExecuteAsync(WebAppFixture.GrpcOptions())).Success.ShouldBeTrue();

        (await app.Client.GetAsync($"/api/project?id={id}", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (await connection.QuerySingleAsync<bool>("SELECT NOT \"Active\" AND \"DeletedAt\" IS NOT NULL FROM \"Projects\" WHERE \"Id\" = @id", new { id })).ShouldBeTrue();
        (await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM \"Organizations\" WHERE \"Id\" = @id", new { id = organization.Id })).ShouldBe(1);
    }

    [Fact]
    public async Task MixedValidityBatch_RollsBackAndCanonicalSortingAcceptsOnlyColumns()
    {
        await using var connection = new NpgsqlConnection(app.ConnectionString);
        var organization = Organization.Create("Batch organization", "");
        var project = Project.Create("Batch project", organization.Id);
        await using (var context = app.CreateDbContext())
        {
            context.Organizations!.Add(organization);
            context.Projects!.Add(project);
            await context.SaveChangesAsync(Cancellation);
        }

        using var request = new HttpRequestMessage(HttpMethod.Delete, "/api/project")
        {
            Content = JsonContent.Create(new { ids = new[] { project.Id, Guid.NewGuid() } })
        };
        (await app.Client.SendAsync(request, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        var repository = new ProjectRepository(connection);
        (await repository.GetByIdAsync(project.Id)).ShouldNotBeNull();
        foreach (var sort in new[] { "name", "Organization", "Name\"; DELETE" })
            (await repository.GetAllAsync(new PaginationParams { SortColumn = sort }, Cancellation)).Data.ShouldNotBeEmpty();
        var filteredResponse = await app.Client.GetAsync($"/api/projects?ids={project.Id},{Guid.NewGuid()}&search=Batch&pageSize=100", Cancellation);
        filteredResponse.StatusCode.ShouldBe(HttpStatusCode.OK, app.FailureDetails);
        var filtered = await filteredResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>(Cancellation);
        filtered.GetProperty("result").GetProperty("totalCount").GetInt32().ShouldBe(1);
        filtered.GetProperty("result").GetProperty("data")[0].GetProperty("id").GetGuid().ShouldBe(project.Id);
        var empty = await app.Client.GetFromJsonAsync<System.Text.Json.JsonElement>("/api/projects?search=never-a-matching-project-name", Cancellation);
        empty.GetProperty("result").GetProperty("totalCount").GetInt32().ShouldBe(0);
        (await app.Client.GetAsync("/api/projects?pageSize=101", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task LoginConstraint_SerializesConcurrentRegistrationsAcrossConnections()
    {
        var login = $"integrity-{Guid.NewGuid():N}";
        async Task<bool> Insert(string value)
        {
            await using var connection = new NpgsqlConnection(app.ConnectionString);
            try
            {
                await connection.ExecuteAsync("""
                    INSERT INTO "Users" ("Id", "Name", "Login", "CreatedAt", "Active") VALUES (@id, 'Integrity', @login, now(), true)
                    """, new { id = Guid.NewGuid(), login = value });
                return true;
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") { return false; }
        }
        var results = await Task.WhenAll(Insert(login), Insert($" {login.ToUpperInvariant()} "));
        results.Count(success => success).ShouldBe(1);
    }

    [Fact]
    public async Task UserAdministration_RejectsNonAdminAndAnonymousRequests()
    {
        using var client = app.CreateClient();
        (await client.GetAsync("/api/users?pageSize=10", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        client.DefaultRequestHeaders.Authorization = new("Bearer", WebAppFixture.CreateToken(admin: false));
        (await client.GetAsync("/api/users?pageSize=10", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        (await client.GetAsync("/api/projects?pageSize=10", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        var command = new GrpcServer.Contracts.Commands.User.ListUsersCommand { Pagination = new PaginationParams() };
        var rejected = await Should.ThrowAsync<Grpc.Core.RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions(admin: false)));
        rejected.StatusCode.ShouldBe(Grpc.Core.StatusCode.PermissionDenied);
        (await command.RemoteExecuteAsync(WebAppFixture.GrpcOptions())).Success.ShouldBeTrue();
    }

    [Fact]
    public async Task InvalidDomainInput_IsRejectedAcrossTransportsWithoutPersisting()
    {
        var id = Guid.NewGuid();
        var command = new GrpcServer.Contracts.Commands.Assignment.CreateAssignmentCommand
        {
            Id = id, Name = "Invalid range", Description = "Invalid", StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(-1), AmountHours = 1, ProjectId = Guid.NewGuid(),
            WorkflowId = Guid.NewGuid(), UserId = Guid.NewGuid(), AssignmentTypeId = Guid.NewGuid()
        };
        var error = await Should.ThrowAsync<Grpc.Core.RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions()));
        error.StatusCode.ShouldBe(Grpc.Core.StatusCode.InvalidArgument);
        (await app.Client.PostAsJsonAsync("/api/assignment", command, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        await using var connection = new NpgsqlConnection(app.ConnectionString);
        (await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM \"Assignments\" WHERE \"Id\" = @id", new { id })).ShouldBe(0);
    }
}
