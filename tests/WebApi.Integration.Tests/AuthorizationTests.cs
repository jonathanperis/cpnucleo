using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using Grpc.Core;
using GrpcServer.Contracts.Commands.Appointment;
using GrpcServer.Contracts.Commands.Assignment;
using GrpcServer.Contracts.Commands.Organization;
using GrpcServer.Contracts.Commands.Project;
using GrpcServer.Contracts.Commands.UserProject;

namespace WebApi.Integration.Tests;

/// <summary>
/// The access model is enforced identically by REST (EF Core and Dapper paths) and gRPC: shared
/// catalog data is admin-written, project data is visible and writable to members only, and
/// members only record their own hours.
/// </summary>
[Collection("Database")]
public class AuthorizationTests(WebAppFixture app)
{
    private static CancellationToken Cancellation => TestContext.Current.CancellationToken;

    [Fact]
    public async Task CatalogData_IsReadableByMembersButWritableOnlyByAdministrators()
    {
        using var member = app.CreateClient(WebAppFixture.Member);

        (await member.GetAsync("/api/organizations?pageSize=1", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        var rejected = await member.PostAsJsonAsync("/api/organization", new { id = Guid.NewGuid(), name = "Member org", description = "x" }, Cancellation);
        rejected.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        (await rejected.Content.ReadFromJsonAsync<JsonElement>(Cancellation)).GetProperty("statusCode").GetInt32().ShouldBe(403);
        (await member.PostAsJsonAsync("/api/workflow", new { id = Guid.NewGuid(), name = "Member flow", order = 1 }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.Forbidden);

        var command = new CreateOrganizationCommand { Id = Guid.NewGuid(), Name = "Member org", Description = "x" };
        (await Should.ThrowAsync<RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions(WebAppFixture.Member))))
            .StatusCode.ShouldBe(StatusCode.PermissionDenied);
    }

    [Fact]
    public async Task ProjectCreators_BecomeMembers_AndOutsidersCanNeitherSeeNorChangeTheProject()
    {
        var graph = await app.CreateGraphAsync();
        using var member = app.CreateClient(WebAppFixture.Member);
        using var outsider = app.CreateClient(WebAppFixture.Outsider);
        var projectId = Guid.NewGuid();

        (await member.PostAsJsonAsync("/api/project", new { id = projectId, name = "Member project", organizationId = graph.Organization.Id }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.OK);
        await using (var connection = app.CreateConnection())
        {
            (await connection.ExecuteScalarAsync<int>("""
                SELECT count(*) FROM "UserProjects" WHERE "UserId" = @user AND "ProjectId" = @project AND "Active"
                """, new { user = WebAppFixture.Member.Id, project = projectId })).ShouldBe(1);
        }

        (await member.GetAsync($"/api/project?id={projectId}", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await member.PatchAsJsonAsync("/api/project", new { id = projectId, name = "Renamed by member", organizationId = graph.Organization.Id }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.OK);

        (await outsider.GetAsync($"/api/project?id={projectId}", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        var listed = await outsider.GetFromJsonAsync<JsonElement>($"/api/projects?ids={projectId}", Cancellation);
        listed.GetProperty("result").GetProperty("totalCount").GetInt32().ShouldBe(0);
        (await outsider.PatchAsJsonAsync("/api/project", new { id = projectId, name = "Hijacked", organizationId = graph.Organization.Id }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.NotFound);
        using (var remove = new HttpRequestMessage(HttpMethod.Delete, "/api/project") { Content = JsonContent.Create(new { ids = new[] { projectId } }) })
            (await outsider.SendAsync(remove, Cancellation)).StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var outsiderOptions = WebAppFixture.GrpcOptions(WebAppFixture.Outsider);
        (await new GetProjectByIdCommand { Id = projectId }.RemoteExecuteAsync(outsiderOptions)).Success.ShouldBeFalse();
        (await new UpdateProjectCommand { Id = projectId, Name = "Hijacked", OrganizationId = graph.Organization.Id }.RemoteExecuteAsync(outsiderOptions))
            .Success.ShouldBeFalse();
        (await new RemoveProjectCommand { Ids = [projectId] }.RemoteExecuteAsync(outsiderOptions)).Success.ShouldBeFalse();

        await using (var connection = app.CreateConnection())
        {
            (await connection.QuerySingleAsync<(string, bool)>("""SELECT "Name", "Active" FROM "Projects" WHERE "Id" = @projectId""", new { projectId }))
                .ShouldBe(("Renamed by member", true));
        }
    }

    [Fact]
    public async Task Outsiders_CannotAddThemselvesToProjects()
    {
        var graph = await app.CreateGraphAsync();
        using var outsider = app.CreateClient(WebAppFixture.Outsider);

        (await outsider.PostAsJsonAsync("/api/userProject", new { id = Guid.NewGuid(), userId = WebAppFixture.Outsider.Id, projectId = graph.Project.Id }, Cancellation))
            .StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        var command = new CreateUserProjectCommand { Id = Guid.NewGuid(), UserId = WebAppFixture.Outsider.Id, ProjectId = graph.Project.Id };
        (await Should.ThrowAsync<RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions(WebAppFixture.Outsider))))
            .StatusCode.ShouldBe(StatusCode.PermissionDenied);
    }

    [Fact]
    public async Task Members_OnlyRecordTheirOwnHours()
    {
        var graph = await app.CreateGraphAsync(WebAppFixture.Member);
        using var member = app.CreateClient(WebAppFixture.Member);
        object Appointment(Guid userId) => new
        {
            id = Guid.NewGuid(), description = "Pairing", keepDate = DateTime.UtcNow, amountHours = 1,
            assignmentId = graph.Assignment.Id, userId
        };

        (await member.PostAsJsonAsync("/api/appointment", Appointment(WebAppFixture.Member.Id), Cancellation)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await member.PostAsJsonAsync("/api/appointment", Appointment(graph.User.Id), Cancellation)).StatusCode.ShouldBe(HttpStatusCode.Forbidden);

        var command = new CreateAppointmentCommand
        {
            Id = Guid.NewGuid(), Name = "Unused", Description = "Pairing", KeepDate = DateTime.UtcNow, AmountHours = 1,
            AssignmentId = graph.Assignment.Id, UserId = graph.User.Id
        };
        (await Should.ThrowAsync<RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions(WebAppFixture.Member))))
            .StatusCode.ShouldBe(StatusCode.PermissionDenied);
        command.Id = Guid.NewGuid();
        command.UserId = WebAppFixture.Member.Id;
        (await command.RemoteExecuteAsync(WebAppFixture.GrpcOptions(WebAppFixture.Member))).Success.ShouldBeTrue();
    }

    [Fact]
    public async Task ProjectScopedData_IsListedOnlyForMemberProjects_OnBothTransports()
    {
        var mine = await app.CreateGraphAsync(WebAppFixture.Member);
        var theirs = await app.CreateGraphAsync();
        using var member = app.CreateClient(WebAppFixture.Member);
        var ids = $"{mine.Assignment.Id},{theirs.Assignment.Id}";

        var rest = await member.GetFromJsonAsync<JsonElement>($"/api/assignments?ids={ids}", Cancellation);
        rest.GetProperty("result").GetProperty("totalCount").GetInt32().ShouldBe(1);
        rest.GetProperty("result").GetProperty("data")[0].GetProperty("id").GetGuid().ShouldBe(mine.Assignment.Id);
        (await member.GetAsync($"/api/assignment?id={theirs.Assignment.Id}", Cancellation)).StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var grpc = await new ListAssignmentsCommand { Pagination = new PaginationParams { Ids = ids } }
            .RemoteExecuteAsync(WebAppFixture.GrpcOptions(WebAppFixture.Member));
        grpc.Result.TotalCount.ShouldBe(1);

        var admin = await app.Client.GetFromJsonAsync<JsonElement>($"/api/assignments?ids={ids}", Cancellation);
        admin.GetProperty("result").GetProperty("totalCount").GetInt32().ShouldBe(2, "administrators see everything");
    }

    [Fact]
    public async Task Members_CannotMoveWorkIntoProjectsTheyDoNotBelongTo()
    {
        var mine = await app.CreateGraphAsync(WebAppFixture.Member);
        var theirs = await app.CreateGraphAsync();
        using var member = app.CreateClient(WebAppFixture.Member);
        var assignment = mine.Assignment;

        var moved = await member.PatchAsJsonAsync("/api/assignment", new
        {
            id = assignment.Id, name = assignment.Name, description = assignment.Description, startDate = assignment.StartDate,
            endDate = assignment.EndDate, amountHours = assignment.AmountHours, projectId = theirs.Project.Id,
            workflowId = assignment.WorkflowId, userId = assignment.UserId, assignmentTypeId = assignment.AssignmentTypeId
        }, Cancellation);

        moved.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        var command = new UpdateAssignmentCommand
        {
            Id = assignment.Id, Name = assignment.Name!, Description = assignment.Description!, StartDate = assignment.StartDate,
            EndDate = assignment.EndDate, AmountHours = assignment.AmountHours, ProjectId = theirs.Project.Id,
            WorkflowId = assignment.WorkflowId, UserId = assignment.UserId, AssignmentTypeId = assignment.AssignmentTypeId
        };
        (await Should.ThrowAsync<RpcException>(() => command.RemoteExecuteAsync(WebAppFixture.GrpcOptions(WebAppFixture.Member))))
            .StatusCode.ShouldBe(StatusCode.PermissionDenied);
    }
}
