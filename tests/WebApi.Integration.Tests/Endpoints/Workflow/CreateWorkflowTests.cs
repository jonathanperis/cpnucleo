using WebApi.Endpoints.Workflow.CreateWorkflow;

namespace WebApi.Integration.Tests.Endpoints.Workflow;

[Collection<WorkflowCollection>]
[Priority(1)]
public class CreateWorkflowTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _workflowId = state.WorkflowId;

    [Fact, Priority(1)]
    public async Task Workflows_ShouldCreateAnWorkflow()
    {
        var (rsp, err) = await app.Client.POSTAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "New workflow",
            Order = 1,
            Id = _workflowId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
