using WebApi.Endpoints.UserProject.CreateUserProject;

namespace WebApi.Integration.Tests.Endpoints.UserProject;

[Collection<UserProjectCollection>]
[Priority(1)]
public class CreateUserProjectTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _userProjectId = state.UserProjectId;
    private readonly Guid _userId = state.UserId;
    private readonly Guid _projectId = state.ProjectId;

    [Fact, Priority(1)]
    public async Task UserProjects_ShouldCreateAnUserProject()
    {
        var (rsp, err) = await app.Client.POSTAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            UserId = _userId,
            ProjectId = _projectId,
            Id = _userProjectId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
