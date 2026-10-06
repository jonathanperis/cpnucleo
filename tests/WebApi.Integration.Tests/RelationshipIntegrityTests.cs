using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using Grpc.Core;
using GrpcServer.Contracts.Commands.Organization;
using GrpcServer.Contracts.Commands.Project;
using Npgsql;

namespace WebApi.Integration.Tests;

/// <summary>
/// Soft-delete aware relationships are enforced by PostgreSQL triggers, so every transport and
/// persistence style gets the same answers, including under concurrency.
/// </summary>
[Collection("Database")]
public class RelationshipIntegrityTests(WebAppFixture app)
{
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    [Fact]
    public async Task NewRows_CannotReferenceRemovedParents()
    {
        var graph = await app.CreateGraphAsync();
        await RemoveDirectlyAsync("Organizations", graph.Organization.Id, cascadeProjects: true);

        var response = await app.Client.PostAsJsonAsync("/api/project", new { id = Guid.NewGuid(), name = "Orphan", organizationId = graph.Organization.Id }, Cancellation);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation);
        body.GetProperty("errors").TryGetProperty("organizationId", out _).ShouldBeTrue(body.ToString());

        var command = new CreateProjectCommand { Id = Guid.NewGuid(), Name = "Orphan", OrganizationId = graph.Organization.Id };
        (await Should.ThrowAsync<RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions())))
            .StatusCode.ShouldBe(StatusCode.InvalidArgument);
    }

    [Fact]
    public async Task Parents_WithActiveDependents_CannotBeRemoved()
    {
        var graph = await app.CreateGraphAsync();

        using var remove = new HttpRequestMessage(HttpMethod.Delete, "/api/organization") { Content = JsonContent.Create(new { ids = new[] { graph.Organization.Id } }) };
        var response = await app.Client.SendAsync(remove, Cancellation);
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("message").GetString()!.ShouldContain("projects");

        (await Should.ThrowAsync<RpcException>(() => new RemoveOrganizationCommand { Ids = [graph.Organization.Id] }
            .RemoteExecuteAsync(WebAppFixture.GrpcOptions()))).StatusCode.ShouldBe(StatusCode.FailedPrecondition);

        (await app.RowStateAsync("Organizations", graph.Organization.Id)).ShouldBe((true, false));
    }

    [Fact]
    public async Task RemovingAParent_CascadesItsMembershipLinks()
    {
        var graph = await app.CreateGraphAsync(WebAppFixture.Member);
        await RemoveDirectlyAsync("Assignments", graph.Assignment.Id);

        (await new RemoveProjectCommand { Ids = [graph.Project.Id] }.RemoteExecuteAsync(WebAppFixture.GrpcOptions())).Success.ShouldBeTrue();

        await using var connection = app.CreateConnection();
        (await connection.ExecuteScalarAsync<int>("""
            SELECT count(*) FROM "UserProjects" WHERE "ProjectId" = @id AND ("Active" OR "DeletedAt" IS NULL)
            """, new { id = graph.Project.Id })).ShouldBe(0, "memberships follow their project");
    }

    [Fact]
    public async Task ConcurrentChildInsertAndParentRemoval_CannotBothSucceed()
    {
        var graph = await app.CreateGraphAsync();
        await RemoveDirectlyAsync("Assignments", graph.Assignment.Id);

        await using var inserter = app.CreateConnection();
        await inserter.OpenAsync(Cancellation);
        await using var transaction = await inserter.BeginTransactionAsync(Cancellation);
        await inserter.ExecuteAsync("""
            INSERT INTO "UserProjects" ("Id", "UserId", "ProjectId", "CreatedAt", "Active") VALUES (@Id, @UserId, @ProjectId, now(), true)
            """, new { Id = Guid.NewGuid(), UserId = graph.User.Id, ProjectId = graph.Project.Id }, transaction);
        var project = Domain.Entities.Assignment.Create("Racing child", "x", DateTime.UtcNow, DateTime.UtcNow, 1,
            graph.Project.Id, graph.Workflow.Id, graph.User.Id, graph.Type.Id);
        await inserter.ExecuteAsync("""
            INSERT INTO "Assignments" ("Id", "Name", "Description", "StartDate", "EndDate", "AmountHours", "ProjectId", "WorkflowId", "UserId", "AssignmentTypeId", "CreatedAt", "Active")
            VALUES (@Id, @Name, @Description, @StartDate, @EndDate, @AmountHours, @ProjectId, @WorkflowId, @UserId, @AssignmentTypeId, @CreatedAt, true)
            """, project, transaction);

        // The parent removal waits for the inserter's FOR SHARE lock, then sees the committed child.
        var removal = RemoveDirectlyAsync("Projects", graph.Project.Id);
        await WaitUntilBlockedOnALockAsync();
        removal.IsCompleted.ShouldBeFalse("the removal must wait for the in-flight child insert");
        await transaction.CommitAsync(Cancellation);

        (await Should.ThrowAsync<PostgresException>(() => removal)).SqlState.ShouldBe(PostgresErrorCodes.RestrictViolation);
        (await app.RowStateAsync("Projects", graph.Project.Id)).ShouldBe((true, false));
    }

    [Fact]
    public async Task ConcurrentParentRemovalAndChildInsert_RejectTheLateChild()
    {
        var graph = await app.CreateGraphAsync();
        await RemoveDirectlyAsync("Assignments", graph.Assignment.Id);

        await using var remover = app.CreateConnection();
        await remover.OpenAsync(Cancellation);
        await using var transaction = await remover.BeginTransactionAsync(Cancellation);
        await remover.ExecuteAsync("""UPDATE "Projects" SET "Active" = false, "DeletedAt" = now() WHERE "Id" = @id""", new { id = graph.Project.Id }, transaction);

        var insert = Task.Run(async () =>
        {
            await using var inserter = app.CreateConnection();
            await inserter.ExecuteAsync("""
                INSERT INTO "UserProjects" ("Id", "UserId", "ProjectId", "CreatedAt", "Active") VALUES (@Id, @UserId, @ProjectId, now(), true)
                """, new { Id = Guid.NewGuid(), UserId = graph.User.Id, ProjectId = graph.Project.Id });
        }, Cancellation);
        await WaitUntilBlockedOnALockAsync();
        insert.IsCompleted.ShouldBeFalse("the child insert must wait for the in-flight removal");
        await transaction.CommitAsync(Cancellation);

        (await Should.ThrowAsync<PostgresException>(() => insert)).SqlState.ShouldBe(PostgresErrorCodes.ForeignKeyViolation);
    }

    /// <summary>Waits until another session is genuinely blocked on a row lock, instead of sleeping.</summary>
    private async Task WaitUntilBlockedOnALockAsync()
    {
        await using var monitor = app.CreateConnection();
        for (var attempt = 0; attempt < 100; attempt++)
        {
            if (await monitor.ExecuteScalarAsync<bool>("""
                SELECT EXISTS (SELECT 1 FROM pg_stat_activity WHERE wait_event_type = 'Lock' AND datname = current_database())
                """)) return;
            await Task.Delay(50, Cancellation);
        }

        throw new TimeoutException("No session ever waited on a lock: the operations did not contend.");
    }

    private async Task RemoveDirectlyAsync(string table, Guid id, bool cascadeProjects = false)
    {
        await using var connection = app.CreateConnection();
        if (cascadeProjects)
        {
            await connection.ExecuteAsync("""UPDATE "Assignments" SET "Active" = false, "DeletedAt" = now() WHERE "ProjectId" IN (SELECT "Id" FROM "Projects" WHERE "OrganizationId" = @id)""", new { id });
            await connection.ExecuteAsync("""UPDATE "Projects" SET "Active" = false, "DeletedAt" = now() WHERE "OrganizationId" = @id""", new { id });
        }

        await connection.ExecuteAsync($"""UPDATE "{table}" SET "Active" = false, "DeletedAt" = now() WHERE "Id" = @id""", new { id });
    }
}
