using WebApi.Endpoints.Organization.ListOrganizations;

namespace WebApi.Integration.Tests.Endpoints.Organization;

[Collection<OrganizationCollection>]
[Priority(2)]
public class ListOrganizationsTests(WebAppFixture app) : TestBase
{
    [Fact, Priority(1)]
    public async Task Organizations_ShouldReturnAllOrganizations()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Pagination = new PaginationParams
            {
                PageNumber = 1,
                PageSize = 10,
                SortColumn = "Name",
                SortOrder = "ASC"
            }
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
