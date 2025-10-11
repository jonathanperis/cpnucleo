using WebApi.Endpoints.Assignment.ListAssignments;

namespace WebApi.Integration.Tests.Endpoints.Assignment;

[Collection<AssignmentCollection>]
[Priority(2)]
public class ListAssignmentsTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    [Fact, Priority(1)]
    public async Task Assignments_ShouldReturnAllAssignments()
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
