using WebApi.Endpoints.Workflow.ListWorkflows;

namespace WebApi.Integration.Tests.Endpoints.Workflow;

[Collection<WorkflowCollection>]
[Priority(2)]
public class ListWorkflowsTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    [Fact, Priority(1)]
    public async Task Workflows_ShouldReturnAllWorkflows()
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
