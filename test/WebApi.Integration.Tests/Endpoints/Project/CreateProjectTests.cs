using WebApi.Endpoints.Project.CreateProject;

namespace WebApi.Integration.Tests.Endpoints.Project;

[Collection<ProjectCollection>]
[Priority(1)]
public class CreateProjectTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _projectId = state.ProjectId;
    private readonly Guid _organizationId = state.OrganizationId;

    [Fact, Priority(1)]
    public async Task Projects_ShouldCreateAnProject()
    {
        var (rsp, err) = await app.Client.POSTAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "New project",
            OrganizationId = _organizationId,
            Id = _projectId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
