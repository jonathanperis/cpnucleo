using WebApi.Endpoints.UserAssignment.CreateUserAssignment;

namespace WebApi.Integration.Tests.Endpoints.UserAssignment;

[Collection<UserAssignmentCollection>]
[Priority(1)]
public class CreateUserAssignmentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _userAssignmentId = state.UserAssignmentId;
    private readonly Guid _userId = state.UserId;
    private readonly Guid _assignmentId = state.AssignmentId;

    [Fact, Priority(1)]
    public async Task UserAssignments_ShouldCreateAnUserAssignment()
    {
        var (rsp, err) = await app.Client.POSTAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            UserId = _userId,
            AssignmentId = _assignmentId,
            Id = _userAssignmentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
