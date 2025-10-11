using WebApi.Endpoints.Workflow.UpdateWorkflow;

namespace WebApi.Integration.Tests.Endpoints.Workflow;

[Collection<WorkflowCollection>]
[Priority(4)]
public class UpdateWorkflowTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _workflowId = state.WorkflowId;

    [Fact, Priority(1)]
    public async Task Workflows_ShouldUpdateAnWorkflow()
    {
        var (rsp, err) = await app.Client.PATCHAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "Updated workflow",
            Order = 2,
            Id = _workflowId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
