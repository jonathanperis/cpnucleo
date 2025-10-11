using WebApi.Endpoints.Assignment.CreateAssignment;

namespace WebApi.Integration.Tests.Endpoints.Assignment;

[Collection<AssignmentCollection>]
[Priority(1)]
public class CreateAssignmentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _assignmentId = state.AssignmentId;
    private readonly Guid _projectId = state.ProjectId;
    private readonly Guid _workflowId = state.WorkflowId;
    private readonly Guid _userId = state.UserId;
    private readonly Guid _assignmentTypeId = state.AssignmentTypeId;

    [Fact, Priority(1)]
    public async Task Assignments_ShouldCreateAnAssignment()
    {
        var (rsp, err) = await app.Client.POSTAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "New assignment",
            Description = "New assignment description",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(7),
            AmountHours = 8,
            ProjectId = _projectId,
            WorkflowId = _workflowId,
            UserId = _userId,
            AssignmentTypeId = _assignmentTypeId,
            Id = _assignmentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
