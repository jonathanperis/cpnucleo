using WebApi.Endpoints.AssignmentType.RemoveAssignmentType;

namespace WebApi.Integration.Tests.Endpoints.AssignmentType;

[Collection<AssignmentTypeCollection>]
[Priority(5)]
public class RemoveAssignmentTypeTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _assignmentTypeId = state.AssignmentTypeId;

    [Fact, Priority(1)]
    public async Task AssignmentTypes_ShouldDeleteAnAssignmentType()
    {
        var (rsp, err) = await app.Client.DELETEAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Ids = [_assignmentTypeId]
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
