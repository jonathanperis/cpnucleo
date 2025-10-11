namespace GrpcServer.Handlers.Assignment;

public sealed class RemoveAssignmentHandler : ICommandHandler<RemoveAssignmentCommand, RemoveAssignmentResult>
{
    public async Task<RemoveAssignmentResult> ExecuteAsync(RemoveAssignmentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}