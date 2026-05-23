using WebApi.Endpoints.AssignmentImpediment.CreateAssignmentImpediment;

namespace WebApi.Integration.Tests.Endpoints.AssignmentImpediment;

[Collection<AssignmentImpedimentCollection>]
[Priority(1)]
public class CreateAssignmentImpedimentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _assignmentImpedimentId = state.AssignmentImpedimentId;
    private readonly Guid _assignmentId = state.AssignmentId;
    private readonly Guid _impedimentId = state.ImpedimentId;

    [Fact, Priority(1)]
    public async Task AssignmentImpediments_ShouldCreateAnAssignmentImpediment()
    {
        var (rsp, err) = await app.Client.POSTAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Description = "New assignment impediment description",
            AssignmentId = _assignmentId,
            ImpedimentId = _impedimentId,
            Id = _assignmentImpedimentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
