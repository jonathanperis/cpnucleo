using WebApi.Endpoints.AssignmentType.ListAssignmentTypes;

namespace WebApi.Integration.Tests.Endpoints.AssignmentType;

[Collection<AssignmentTypeCollection>]
[Priority(2)]
public class ListAssignmentTypesTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    [Fact, Priority(1)]
    public async Task AssignmentTypes_ShouldReturnAllAssignmentTypes()
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
