using WebApi.Endpoints.Project.GetProjectById;

namespace WebApi.Integration.Tests.Endpoints.Project;

[Collection<ProjectCollection>]
[Priority(3)]
public class GetProjectByIdTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _projectId = state.ProjectId;

    [Fact, Priority(1)]
    public async Task Projects_ShouldGetAnProject()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Id = _projectId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
