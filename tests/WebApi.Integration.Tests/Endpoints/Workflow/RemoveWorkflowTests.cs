using WebApi.Endpoints.Workflow.RemoveWorkflow;

namespace WebApi.Integration.Tests.Endpoints.Workflow;

[Collection<WorkflowCollection>]
[Priority(5)]
public class RemoveWorkflowTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _workflowId = state.WorkflowId;

    [Fact, Priority(1)]
    public async Task Workflows_ShouldDeleteAnWorkflow()
    {
        var (rsp, err) = await app.Client.DELETEAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Ids = [_workflowId]
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
