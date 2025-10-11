namespace GrpcServer.Handlers.Workflow;

public sealed class ListWorkflowsHandler : ICommandHandler<ListWorkflowsCommand, ListWorkflowsResult>
{
    public async Task<ListWorkflowsResult> ExecuteAsync(ListWorkflowsCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}