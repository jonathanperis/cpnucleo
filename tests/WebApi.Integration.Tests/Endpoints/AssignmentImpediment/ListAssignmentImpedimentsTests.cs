using WebApi.Endpoints.AssignmentImpediment.ListAssignmentImpediments;

namespace WebApi.Integration.Tests.Endpoints.AssignmentImpediment;

[Collection<AssignmentImpedimentCollection>]
[Priority(2)]
public class ListAssignmentImpedimentsTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    [Fact, Priority(1)]
    public async Task AssignmentImpediments_ShouldReturnAllAssignmentImpediments()
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
