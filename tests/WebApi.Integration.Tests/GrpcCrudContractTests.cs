using Domain.Common.Security;
using Domain.Entities;

namespace WebApi.Integration.Tests;

/// <summary>The gRPC transport honors the same CRUD and soft-delete contract as REST for every resource.</summary>
[Collection("Database")]
public class GrpcCrudContractTests(WebAppFixture app)
{
    [Theory]
    [InlineData("Organization", "Organizations")]
    [InlineData("Project", "Projects")]
    [InlineData("User", "Users")]
    [InlineData("Workflow", "Workflows")]
    [InlineData("Impediment", "Impediments")]
    [InlineData("AssignmentType", "AssignmentTypes")]
    [InlineData("UserProject", "UserProjects")]
    [InlineData("Assignment", "Assignments")]
    [InlineData("AssignmentImpediment", "AssignmentImpediments")]
    [InlineData("UserAssignment", "UserAssignments")]
    [InlineData("Appointment", "Appointments")]
    public async Task EachResource_CompletesAnIndependentCrudCycle(string resource, string table)
    {
        var graph = await app.CreateGraphAsync();
        var secondUser = User.Create("Second", $"second-{Guid.NewGuid():N}", new PasswordHash("unused-test-hash", ""));
        await using (var db = app.CreateDbContext())
        {
            db.Add(secondUser);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var id = Guid.NewGuid();
        var options = WebAppFixture.GrpcOptions();
        var created = await Remote.ExecuteAsync(Remote.Command(resource, "Create", CreateValues(resource, id, graph, graph.User.Id)), options, app);
        created.Get<bool>("Success").ShouldBeTrue(created.Get<string>("Message"));

        var fetched = await Remote.ExecuteAsync(Remote.Command(resource, "Get", new { Id = id }), options, app);
        fetched.Get<bool>("Success").ShouldBeTrue();

        var listed = await Remote.ExecuteAsync(Remote.Command(resource, "List", new { Pagination = new PaginationParams { Ids = id.ToString() } }), options, app);
        listed.Get<object>("Result").Get<int>("TotalCount").ShouldBe(1);

        var updated = await Remote.ExecuteAsync(Remote.Command(resource, "Update", UpdateValues(resource, id, graph, secondUser.Id)), options, app);
        updated.Get<bool>("Success").ShouldBeTrue(updated.Get<string>("Message"));
        var dto = (await Remote.ExecuteAsync(Remote.Command(resource, "Get", new { Id = id }), options)).Get<object>(resource);
        var (property, expected) = ChangedProperty(resource, secondUser.Id);
        dto.Get<object>(property).ShouldBe(expected, $"{resource} must persist its update over gRPC");

        var removed = await Remote.ExecuteAsync(Remote.Command(resource, "Remove", new { Ids = new List<Guid> { id, id } }), options, app);
        removed.Get<bool>("Success").ShouldBeTrue(removed.Get<string>("Message"));
        (await Remote.ExecuteAsync(Remote.Command(resource, "Get", new { Id = id }), options)).Get<bool>("Success").ShouldBeFalse();
        (await app.RowStateAsync(table, id)).ShouldBe((false, true));
    }

    private static object CreateValues(string resource, Guid id, Graph graph, Guid userId) => resource switch
    {
        "Organization" => new { Id = id, Name = "Created", Description = "Created" },
        "Project" => new { Id = id, Name = "Created", OrganizationId = graph.Organization.Id },
        "User" => new { Id = id, Name = "Created", Login = $"grpc-{id:N}", Password = "Integration@123" },
        "Workflow" => new { Id = id, Name = "Created", Order = 1 },
        "Impediment" or "AssignmentType" => new { Id = id, Name = "Created" },
        "UserProject" => new { Id = id, UserId = userId, ProjectId = graph.Project.Id },
        "Assignment" => new
        {
            Id = id, Name = "Created", Description = "Created", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1),
            AmountHours = 2, ProjectId = graph.Project.Id, WorkflowId = graph.Workflow.Id, UserId = userId, AssignmentTypeId = graph.Type.Id
        },
        "AssignmentImpediment" => new { Id = id, Description = "Created", AssignmentId = graph.Assignment.Id, ImpedimentId = graph.Impediment.Id },
        "UserAssignment" => new { Id = id, UserId = userId, AssignmentId = graph.Assignment.Id },
        "Appointment" => new { Id = id, Name = "Unused", Description = "Created", KeepDate = DateTime.UtcNow, AmountHours = 2, AssignmentId = graph.Assignment.Id, UserId = userId },
        _ => throw new ArgumentOutOfRangeException(nameof(resource))
    };

    private static object UpdateValues(string resource, Guid id, Graph graph, Guid userId) => resource switch
    {
        "Organization" => new { Id = id, Name = "Updated", Description = "Updated" },
        "Project" => new { Id = id, Name = "Updated", OrganizationId = graph.Organization.Id },
        "User" => new { Id = id, Name = "Updated" },
        "Workflow" => new { Id = id, Name = "Updated", Order = 2 },
        "Impediment" or "AssignmentType" => new { Id = id, Name = "Updated" },
        "UserProject" => new { Id = id, UserId = userId, ProjectId = graph.Project.Id },
        "Assignment" => new
        {
            Id = id, Name = "Updated", Description = "Updated", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1),
            AmountHours = 3, ProjectId = graph.Project.Id, WorkflowId = graph.Workflow.Id, UserId = userId, AssignmentTypeId = graph.Type.Id
        },
        "AssignmentImpediment" => new { Id = id, Description = "Updated", AssignmentId = graph.Assignment.Id, ImpedimentId = graph.Impediment.Id },
        "UserAssignment" => new { Id = id, UserId = userId, AssignmentId = graph.Assignment.Id },
        "Appointment" => new { Id = id, Name = "Unused", Description = "Updated", KeepDate = DateTime.UtcNow, AmountHours = 3, AssignmentId = graph.Assignment.Id, UserId = userId },
        _ => throw new ArgumentOutOfRangeException(nameof(resource))
    };

    private static (string Property, object Expected) ChangedProperty(string resource, Guid secondUserId) => resource switch
    {
        "UserProject" or "UserAssignment" => ("UserId", secondUserId),
        "Workflow" => ("Order", 2),
        "Appointment" => ("AmountHours", 3),
        "AssignmentImpediment" => ("Description", "Updated"),
        _ => ("Name", "Updated")
    };
}
