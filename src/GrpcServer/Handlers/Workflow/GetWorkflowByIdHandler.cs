namespace GrpcServer.Handlers.Workflow;

public sealed class GetWorkflowByIdHandler : ICommandHandler<GetWorkflowByIdCommand, GetWorkflowByIdResult>
{
    public async Task<GetWorkflowByIdResult> ExecuteAsync(GetWorkflowByIdCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}