using WebApi.Endpoints.AssignmentImpediment.UpdateAssignmentImpediment;

namespace WebApi.Integration.Tests.Endpoints.AssignmentImpediment;

[Collection<AssignmentImpedimentCollection>]
[Priority(4)]
public class UpdateAssignmentImpedimentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _assignmentImpedimentId = state.AssignmentImpedimentId;
    private readonly Guid _assignmentId = state.AssignmentId;
    private readonly Guid _impedimentId = state.ImpedimentId;

    [Fact, Priority(1)]
    public async Task AssignmentImpediments_ShouldUpdateAnAssignmentImpediment()
    {
        var (rsp, err) = await app.Client.PATCHAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Description = "Updated assignment impediment description",
            AssignmentId = _assignmentId,
            ImpedimentId = _impedimentId,
            Id = _assignmentImpedimentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
