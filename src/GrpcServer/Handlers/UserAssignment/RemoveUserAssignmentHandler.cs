namespace GrpcServer.Handlers.UserAssignment;

public sealed class RemoveUserAssignmentHandler : ICommandHandler<RemoveUserAssignmentCommand, RemoveUserAssignmentResult>
{
    public async Task<RemoveUserAssignmentResult> ExecuteAsync(RemoveUserAssignmentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}