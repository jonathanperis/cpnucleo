namespace GrpcServer.Handlers.Workflow;

public sealed class RemoveWorkflowHandler : ICommandHandler<RemoveWorkflowCommand, RemoveWorkflowResult>
{
    public async Task<RemoveWorkflowResult> ExecuteAsync(RemoveWorkflowCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}