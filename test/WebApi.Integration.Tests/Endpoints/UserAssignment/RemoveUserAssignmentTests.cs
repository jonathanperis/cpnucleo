using WebApi.Endpoints.UserAssignment.RemoveUserAssignment;

namespace WebApi.Integration.Tests.Endpoints.UserAssignment;

[Collection<UserAssignmentCollection>]
[Priority(5)]
public class RemoveUserAssignmentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _userAssignmentId = state.UserAssignmentId;

    [Fact, Priority(1)]
    public async Task UserAssignments_ShouldDeleteAnUserAssignment()
    {
        var (rsp, err) = await app.Client.DELETEAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Ids = [_userAssignmentId]
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
