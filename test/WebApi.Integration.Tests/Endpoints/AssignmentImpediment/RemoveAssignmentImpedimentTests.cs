using WebApi.Endpoints.AssignmentImpediment.RemoveAssignmentImpediment;

namespace WebApi.Integration.Tests.Endpoints.AssignmentImpediment;

[Collection<AssignmentImpedimentCollection>]
[Priority(5)]
public class RemoveAssignmentImpedimentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _assignmentImpedimentId = state.AssignmentImpedimentId;

    [Fact, Priority(1)]
    public async Task AssignmentImpediments_ShouldDeleteAnAssignmentImpediment()
    {
        var (rsp, err) = await app.Client.DELETEAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Ids = [_assignmentImpedimentId]
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
