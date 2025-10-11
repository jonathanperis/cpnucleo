using WebApi.Endpoints.UserAssignment.UpdateUserAssignment;

namespace WebApi.Integration.Tests.Endpoints.UserAssignment;

[Collection<UserAssignmentCollection>]
[Priority(4)]
public class UpdateUserAssignmentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _userAssignmentId = state.UserAssignmentId;
    private readonly Guid _userId = state.UserId;
    private readonly Guid _assignmentId = state.AssignmentId;

    [Fact, Priority(1)]
    public async Task UserAssignments_ShouldUpdateAnUserAssignment()
    {
        var (rsp, err) = await app.Client.PATCHAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            UserId = _userId,
            AssignmentId = _assignmentId,
            Id = _userAssignmentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
