using WebApi.Endpoints.Project.RemoveProject;

namespace WebApi.Integration.Tests.Endpoints.Project;

[Collection<ProjectCollection>]
[Priority(5)]
public class RemoveProjectTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _projectId = state.ProjectId;

    [Fact, Priority(1)]
    public async Task Projects_ShouldDeleteAnProject()
    {
        var (rsp, err) = await app.Client.DELETEAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Ids = [_projectId]
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
