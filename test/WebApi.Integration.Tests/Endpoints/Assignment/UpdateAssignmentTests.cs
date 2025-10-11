using WebApi.Endpoints.Assignment.UpdateAssignment;

namespace WebApi.Integration.Tests.Endpoints.Assignment;

[Collection<AssignmentCollection>]
[Priority(4)]
public class UpdateAssignmentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _assignmentId = state.AssignmentId;
    private readonly Guid _projectId = state.ProjectId;
    private readonly Guid _workflowId = state.WorkflowId;
    private readonly Guid _userId = state.UserId;
    private readonly Guid _assignmentTypeId = state.AssignmentTypeId;

    [Fact, Priority(1)]
    public async Task Assignments_ShouldUpdateAnAssignment()
    {
        var (rsp, err) = await app.Client.PATCHAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "Updated assignment",
            Description = "Updated assignment description",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(7),
            AmountHours = 16,
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
