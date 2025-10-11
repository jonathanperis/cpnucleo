namespace GrpcServer.Handlers.Assignment;

public sealed class CreateAssignmentHandler : ICommandHandler<CreateAssignmentCommand, CreateAssignmentResult>
{
    public async Task<CreateAssignmentResult> ExecuteAsync(CreateAssignmentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}