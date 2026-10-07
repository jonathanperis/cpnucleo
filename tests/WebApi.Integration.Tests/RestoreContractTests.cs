using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using Domain.Common.Security;
using Domain.Entities;

namespace WebApi.Integration.Tests;

/// <summary>
/// Restore undoes a soft delete over REST: atomic batches of removed rows the caller may see and
/// change, with the same access rules and relationship triggers as every other write.
/// </summary>
[Collection("Database")]
public class RestoreContractTests(WebAppFixture app)
{
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    [Theory]
    [InlineData("organization", "Organizations")]
    [InlineData("project", "Projects")]
    [InlineData("user", "Users")]
    [InlineData("workflow", "Workflows")]
    [InlineData("impediment", "Impediments")]
    [InlineData("assignmentType", "AssignmentTypes")]
    [InlineData("userProject", "UserProjects")]
    [InlineData("assignment", "Assignments")]
    [InlineData("assignmentImpediment", "AssignmentImpediments")]
    [InlineData("userAssignment", "UserAssignments")]
    [InlineData("appointment", "Appointments")]
    public async Task EachResource_RestoresWhatItRemoved(string singular, string table)
    {
        var graph = await app.CreateGraphAsync();
        var id = Guid.NewGuid();
        (await app.Client.PostAsJsonAsync($"/api/{singular}", Payload(graph, id), Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        var version = await UpdatedAtAsync(table, id);
        (await RemoveAsync(singular, id)).StatusCode.ShouldBe(HttpStatusCode.OK);

        var response = await app.Client.PostAsJsonAsync($"/api/{singular}/restore", new { ids = new[] { id, id } }, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("success").GetBoolean().ShouldBeTrue();
        (await app.RowStateAsync(table, id)).ShouldBe((true, false), "restore sets Active and clears DeletedAt");
        (await app.Client.GetAsync($"/api/{singular}?id={id}", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await UpdatedAtAsync(table, id)).ShouldBe(version, "restoring is not an edit");
    }

    [Theory]
    [InlineData("organization", "Organizations")]
    [InlineData("project", "Projects")]
    [InlineData("workflow", "Workflows")]
    public async Task ABatch_ContainingAnActiveOrUnknownId_RestoresNothing(string singular, string table)
    {
        var graph = await app.CreateGraphAsync();
        var removed = Guid.NewGuid();
        var active = Guid.NewGuid();
        (await app.Client.PostAsJsonAsync($"/api/{singular}", Payload(graph, removed), Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await app.Client.PostAsJsonAsync($"/api/{singular}", Payload(graph, active), Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await RemoveAsync(singular, removed)).StatusCode.ShouldBe(HttpStatusCode.OK);

        foreach (var batch in new[] { new[] { removed, Guid.NewGuid() }, new[] { removed, active } })
        {
            var response = await app.Client.PostAsJsonAsync($"/api/{singular}/restore", new { ids = batch }, Cancellation);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("statusCode").GetInt32().ShouldBe(404);
        }

        (await app.RowStateAsync(table, removed)).ShouldBe((false, true), "the batch is atomic");
        (await app.RowStateAsync(table, active)).ShouldBe((true, false));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public async Task Batches_AreBoundedLikeRemovals(int count)
    {
        var response = await app.Client.PostAsJsonAsync("/api/workflow/restore", new { ids = Enumerable.Range(0, count).Select(_ => Guid.NewGuid()).ToArray() }, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("errors").TryGetProperty("ids", out _).ShouldBeTrue();
    }

    [Fact]
    public async Task RestoringAChildOfARemovedParent_IsRejectedByTheRelationshipTriggers()
    {
        var graph = await app.CreateGraphAsync();
        var appointment = Guid.NewGuid();
        (await app.Client.PostAsJsonAsync("/api/appointment", Payload(graph, appointment), Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await RemoveAsync("appointment", appointment)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await RemoveAsync("assignment", graph.Assignment.Id)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await RemoveAsync("project", graph.Project.Id)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await RemoveAsync("organization", graph.Organization.Id)).StatusCode.ShouldBe(HttpStatusCode.OK);

        // EF Core path: the appointment's assignment is removed.
        var child = await app.Client.PostAsJsonAsync("/api/appointment/restore", new { ids = new[] { appointment } }, Cancellation);
        child.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        (await child.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("errors").TryGetProperty("assignmentId", out _).ShouldBeTrue();
        (await app.RowStateAsync("Appointments", appointment)).ShouldBe((false, true));

        // Dapper path: the project's organization is removed.
        var project = await app.Client.PostAsJsonAsync("/api/project/restore", new { ids = new[] { graph.Project.Id } }, Cancellation);
        project.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        (await project.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("errors").TryGetProperty("organizationId", out _).ShouldBeTrue();

        // Restoring parents first, in one batch per resource, brings the whole chain back.
        foreach (var (singular, id) in new[] { ("organization", graph.Organization.Id), ("project", graph.Project.Id), ("assignment", graph.Assignment.Id), ("appointment", appointment) })
            (await app.Client.PostAsJsonAsync($"/api/{singular}/restore", new { ids = new[] { id } }, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK, singular);
        (await app.RowStateAsync("Appointments", appointment)).ShouldBe((true, false));
    }

    [Fact]
    public async Task RestoringAUser_WhoseLoginWasReused_IsAConflict()
    {
        var login = $"reused-{Guid.NewGuid():N}";
        var original = Guid.NewGuid();
        (await app.Client.PostAsJsonAsync("/api/user", new { id = original, name = "Original", login, password = "Integration@123" }, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await RemoveAsync("user", original)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await app.Client.PostAsJsonAsync("/api/user", new { id = Guid.NewGuid(), name = "Successor", login, password = "Integration@123" }, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);

        var response = await app.Client.PostAsJsonAsync("/api/user/restore", new { ids = new[] { original } }, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        (await response.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("message").GetString().ShouldBe("This login is already in use.");
        (await app.RowStateAsync("Users", original)).ShouldBe((false, true));
    }

    [Fact]
    public async Task RestoringAParent_BringsBackTheMembershipsRemovedWithIt()
    {
        var graph = await app.CreateGraphAsync();
        var user = User.Create("Linked", $"linked-{Guid.NewGuid():N}", new PasswordHash("unused-test-hash", ""));
        var membership = UserProject.Create(user.Id, graph.Project.Id);
        var earlier = UserAssignment.Create(user.Id, graph.Assignment.Id);
        var cascaded = UserAssignment.Create(user.Id, graph.Assignment.Id);
        await using (var db = app.CreateDbContext())
        {
            db.AddRange(user, membership, earlier, cascaded);
            await db.SaveChangesAsync(Cancellation);
        }

        (await RemoveAsync("userAssignment", earlier.Id)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await RemoveAsync("user", user.Id)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await app.RowStateAsync("UserProjects", membership.Id)).ShouldBe((false, true), "memberships follow their user");

        (await app.Client.PostAsJsonAsync("/api/user/restore", new { ids = new[] { user.Id } }, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);

        (await app.RowStateAsync("UserProjects", membership.Id)).ShouldBe((true, false));
        (await app.RowStateAsync("UserAssignments", cascaded.Id)).ShouldBe((true, false));
        (await app.RowStateAsync("UserAssignments", earlier.Id)).ShouldBe((false, true), "links removed on their own stay removed");
    }

    [Fact]
    public async Task Members_CanUndoRemovingTheirProject_AndOutsidersCannot()
    {
        var graph = await app.CreateGraphAsync();
        using var member = app.CreateClient(WebAppFixture.Member);
        using var outsider = app.CreateClient(WebAppFixture.Outsider);
        var projectId = Guid.NewGuid();
        (await member.PostAsJsonAsync("/api/project", new { id = projectId, name = "Undo me", organizationId = graph.Organization.Id }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.OK);
        using (var remove = new HttpRequestMessage(HttpMethod.Delete, "/api/project") { Content = JsonContent.Create(new { ids = new[] { projectId } }) })
            (await member.SendAsync(remove, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await member.GetAsync($"/api/project?id={projectId}", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.NotFound, "the membership was removed with the project");

        (await outsider.PostAsJsonAsync("/api/project/restore", new { ids = new[] { projectId } }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.NotFound, "a removed project of someone else looks missing");
        (await app.RowStateAsync("Projects", projectId)).ShouldBe((false, true));

        (await member.PostAsJsonAsync("/api/project/restore", new { ids = new[] { projectId } }, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await member.GetAsync($"/api/project?id={projectId}", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK, "the membership came back with the project");
    }

    [Fact]
    public async Task NonMembers_CannotRestoreProjectData()
    {
        var graph = await app.CreateGraphAsync(WebAppFixture.Member);
        var assignmentImpediment = AssignmentImpediment.Create("Blocked", graph.Assignment.Id, graph.Impediment.Id);
        var appointment = Appointment.Create("Hours", DateTime.UtcNow, 1, graph.Assignment.Id, WebAppFixture.Member.Id);
        await using (var db = app.CreateDbContext())
        {
            db.AddRange(assignmentImpediment, appointment);
            await db.SaveChangesAsync(Cancellation);
        }

        (await RemoveAsync("assignmentImpediment", assignmentImpediment.Id)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await RemoveAsync("appointment", appointment.Id)).StatusCode.ShouldBe(HttpStatusCode.OK);
        using var outsider = app.CreateClient(WebAppFixture.Outsider);
        using var member = app.CreateClient(WebAppFixture.Member);

        (await outsider.PostAsJsonAsync("/api/assignmentImpediment/restore", new { ids = new[] { assignmentImpediment.Id } }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (await outsider.PostAsJsonAsync("/api/appointment/restore", new { ids = new[] { appointment.Id } }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (await app.RowStateAsync("AssignmentImpediments", assignmentImpediment.Id)).ShouldBe((false, true));
        (await app.RowStateAsync("Appointments", appointment.Id)).ShouldBe((false, true));

        (await member.PostAsJsonAsync("/api/assignmentImpediment/restore", new { ids = new[] { assignmentImpediment.Id } }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.OK);
        (await member.PostAsJsonAsync("/api/appointment/restore", new { ids = new[] { appointment.Id } }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.OK, "members restore their own hours");
    }

    [Theory]
    [InlineData("organization", "Organizations")]
    [InlineData("workflow", "Workflows")]
    public async Task Members_CannotRestoreCatalogData(string singular, string table)
    {
        var graph = await app.CreateGraphAsync();
        var id = Guid.NewGuid();
        (await app.Client.PostAsJsonAsync($"/api/{singular}", Payload(graph, id), Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await RemoveAsync(singular, id)).StatusCode.ShouldBe(HttpStatusCode.OK);
        using var member = app.CreateClient(WebAppFixture.Member);

        var response = await member.PostAsJsonAsync($"/api/{singular}/restore", new { ids = new[] { id } }, Cancellation);

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        (await app.RowStateAsync(table, id)).ShouldBe((false, true));
        (await member.PostAsJsonAsync("/api/user/restore", new { ids = new[] { Guid.NewGuid() } }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.Forbidden, "user administration requires an administrator");
    }

    private async Task<DateTime?> UpdatedAtAsync(string table, Guid id)
    {
        await using var connection = app.CreateConnection();
        return await connection.QuerySingleAsync<DateTime?>($"""SELECT "UpdatedAt" FROM "{table}" WHERE "Id" = @id""", new { id });
    }

    private async Task<HttpResponseMessage> RemoveAsync(string singular, Guid id)
    {
        using var remove = new HttpRequestMessage(HttpMethod.Delete, $"/api/{singular}") { Content = JsonContent.Create(new { ids = new[] { id } }) };
        return await app.Client.SendAsync(remove, Cancellation);
    }

    private static Dictionary<string, object> Payload(Graph graph, Guid id) => new()
    {
        ["id"] = id, ["name"] = "Restorable", ["description"] = "Restorable", ["order"] = 1,
        ["login"] = $"restore-{id:N}", ["password"] = "Integration@123", ["amountHours"] = 2,
        ["startDate"] = DateTime.UtcNow, ["endDate"] = DateTime.UtcNow.AddDays(1), ["keepDate"] = DateTime.UtcNow,
        ["organizationId"] = graph.Organization.Id, ["projectId"] = graph.Project.Id, ["userId"] = graph.User.Id,
        ["workflowId"] = graph.Workflow.Id, ["assignmentTypeId"] = graph.Type.Id, ["impedimentId"] = graph.Impediment.Id,
        ["assignmentId"] = graph.Assignment.Id
    };
}
