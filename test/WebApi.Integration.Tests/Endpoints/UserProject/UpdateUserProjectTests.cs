using WebApi.Endpoints.UserProject.UpdateUserProject;

namespace WebApi.Integration.Tests.Endpoints.UserProject;

[Collection<UserProjectCollection>]
[Priority(4)]
public class UpdateUserProjectTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _userProjectId = state.UserProjectId;
    private readonly Guid _userId = state.UserId;
    private readonly Guid _projectId = state.ProjectId;

    [Fact, Priority(1)]
    public async Task UserProjects_ShouldUpdateAnUserProject()
    {
        var (rsp, err) = await app.Client.PATCHAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            UserId = _userId,
            ProjectId = _projectId,
            Id = _userProjectId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
