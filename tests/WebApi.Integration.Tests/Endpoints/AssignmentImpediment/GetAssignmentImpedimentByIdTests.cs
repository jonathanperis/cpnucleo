using WebApi.Endpoints.AssignmentImpediment.GetAssignmentImpedimentById;

namespace WebApi.Integration.Tests.Endpoints.AssignmentImpediment;

[Collection<AssignmentImpedimentCollection>]
[Priority(3)]
public class GetAssignmentImpedimentByIdTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _assignmentImpedimentId = state.AssignmentImpedimentId;

    [Fact, Priority(1)]
    public async Task AssignmentImpediments_ShouldGetAnAssignmentImpediment()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Id = _assignmentImpedimentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
