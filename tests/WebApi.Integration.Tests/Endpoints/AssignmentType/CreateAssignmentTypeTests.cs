using WebApi.Endpoints.AssignmentType.CreateAssignmentType;

namespace WebApi.Integration.Tests.Endpoints.AssignmentType;

[Collection<AssignmentTypeCollection>]
[Priority(1)]
public class CreateAssignmentTypeTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _assignmentTypeId = state.AssignmentTypeId;

    [Fact, Priority(1)]
    public async Task AssignmentTypes_ShouldCreateAnAssignmentType()
    {
        var (rsp, err) = await app.Client.POSTAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "New assignment type",
            Id = _assignmentTypeId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
