using WebApi.Endpoints.Project.UpdateProject;

namespace WebApi.Integration.Tests.Endpoints.Project;

[Collection<ProjectCollection>]
[Priority(4)]
public class UpdateProjectTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _projectId = state.ProjectId;
    private readonly Guid _organizationId = state.OrganizationId;

    [Fact, Priority(1)]
    public async Task Projects_ShouldUpdateAnProject()
    {
        var (rsp, err) = await app.Client.PATCHAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "Updated project",
            OrganizationId = _organizationId,
            Id = _projectId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
