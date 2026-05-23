using WebApi.Endpoints.Assignment.RemoveAssignment;

namespace WebApi.Integration.Tests.Endpoints.Assignment;

[Collection<AssignmentCollection>]
[Priority(5)]
public class RemoveAssignmentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _assignmentId = state.AssignmentId;

    [Fact, Priority(1)]
    public async Task Assignments_ShouldDeleteAnAssignment()
    {
        var (rsp, err) = await app.Client.DELETEAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Ids = [_assignmentId]
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
