using WebApi.Endpoints.UserAssignment.GetUserAssignmentById;

namespace WebApi.Integration.Tests.Endpoints.UserAssignment;

[Collection<UserAssignmentCollection>]
[Priority(3)]
public class GetUserAssignmentByIdTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _userAssignmentId = state.UserAssignmentId;

    [Fact, Priority(1)]
    public async Task UserAssignments_ShouldGetAnUserAssignment()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Id = _userAssignmentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
