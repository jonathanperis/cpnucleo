using WebApi.Endpoints.UserAssignment.ListUserAssignments;

namespace WebApi.Integration.Tests.Endpoints.UserAssignment;

[Collection<UserAssignmentCollection>]
[Priority(2)]
public class ListUserAssignmentsTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    [Fact, Priority(1)]
    public async Task UserAssignments_ShouldReturnAllUserAssignments()
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
