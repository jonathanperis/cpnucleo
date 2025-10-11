using WebApi.Endpoints.UserProject.GetUserProjectById;

namespace WebApi.Integration.Tests.Endpoints.UserProject;

[Collection<UserProjectCollection>]
[Priority(3)]
public class GetUserProjectByIdTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _userProjectId = state.UserProjectId;

    [Fact, Priority(1)]
    public async Task UserProjects_ShouldGetAnUserProject()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Id = _userProjectId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
