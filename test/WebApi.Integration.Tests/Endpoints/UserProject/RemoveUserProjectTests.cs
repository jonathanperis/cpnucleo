using WebApi.Endpoints.UserProject.RemoveUserProject;

namespace WebApi.Integration.Tests.Endpoints.UserProject;

[Collection<UserProjectCollection>]
[Priority(5)]
public class RemoveUserProjectTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _userProjectId = state.UserProjectId;

    [Fact, Priority(1)]
    public async Task UserProjects_ShouldDeleteAnUserProject()
    {
        var (rsp, err) = await app.Client.DELETEAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Ids = [_userProjectId]
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
