using WebApi.Endpoints.AssignmentType.UpdateAssignmentType;

namespace WebApi.Integration.Tests.Endpoints.AssignmentType;

[Collection<AssignmentTypeCollection>]
[Priority(4)]
public class UpdateAssignmentTypeTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _assignmentTypeId = state.AssignmentTypeId;

    [Fact, Priority(1)]
    public async Task AssignmentTypes_ShouldUpdateAnAssignmentType()
    {
        var (rsp, err) = await app.Client.PATCHAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "Updated assignment type",
            Id = _assignmentTypeId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
