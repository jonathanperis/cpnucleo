namespace GrpcServer.Handlers.Workflow;

public sealed class CreateWorkflowHandler : ICommandHandler<CreateWorkflowCommand, CreateWorkflowResult>
{
    public async Task<CreateWorkflowResult> ExecuteAsync(CreateWorkflowCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}