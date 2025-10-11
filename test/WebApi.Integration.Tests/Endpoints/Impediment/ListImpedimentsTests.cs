using WebApi.Endpoints.Impediment.ListImpediments;

namespace WebApi.Integration.Tests.Endpoints.Impediment;

[Collection<ImpedimentCollection>]
[Priority(2)]
public class ListImpedimentsTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    [Fact, Priority(1)]
    public async Task Impediments_ShouldReturnAllImpediments()
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
