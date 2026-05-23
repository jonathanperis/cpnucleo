using WebApi.Endpoints.AssignmentType.GetAssignmentTypeById;

namespace WebApi.Integration.Tests.Endpoints.AssignmentType;

[Collection<AssignmentTypeCollection>]
[Priority(3)]
public class GetAssignmentTypeByIdTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _assignmentTypeId = state.AssignmentTypeId;

    [Fact, Priority(1)]
    public async Task AssignmentTypes_ShouldGetAnAssignmentType()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Id = _assignmentTypeId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
