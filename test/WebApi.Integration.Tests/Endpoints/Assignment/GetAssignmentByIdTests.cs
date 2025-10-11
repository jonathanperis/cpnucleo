using WebApi.Endpoints.Assignment.GetAssignmentById;

namespace WebApi.Integration.Tests.Endpoints.Assignment;

[Collection<AssignmentCollection>]
[Priority(3)]
public class GetAssignmentByIdTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _assignmentId = state.AssignmentId;

    [Fact, Priority(1)]
    public async Task Assignments_ShouldGetAnAssignment()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Id = _assignmentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
