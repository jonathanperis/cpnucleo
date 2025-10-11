using WebApi.Endpoints.UserProject.ListUserProjects;

namespace WebApi.Integration.Tests.Endpoints.UserProject;

[Collection<UserProjectCollection>]
[Priority(2)]
public class ListUserProjectsTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    [Fact, Priority(1)]
    public async Task UserProjects_ShouldReturnAllUserProjects()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Pagination = new PaginationParams
            {
                PageNumber = 1,
                PageSize = 10,
                SortColumn = "Id",
                SortOrder = "ASC"
            }
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
