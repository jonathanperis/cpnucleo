using WebApi.Endpoints.Workflow.GetWorkflowById;

namespace WebApi.Integration.Tests.Endpoints.Workflow;

[Collection<WorkflowCollection>]
[Priority(3)]
public class GetWorkflowByIdTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _workflowId = state.WorkflowId;

    [Fact, Priority(1)]
    public async Task Workflows_ShouldGetAnWorkflow()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Id = _workflowId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
