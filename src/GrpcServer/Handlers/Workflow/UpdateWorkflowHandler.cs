namespace GrpcServer.Handlers.Workflow;

public sealed class UpdateWorkflowHandler : ICommandHandler<UpdateWorkflowCommand, UpdateWorkflowResult>
{
    public async Task<UpdateWorkflowResult> ExecuteAsync(UpdateWorkflowCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}