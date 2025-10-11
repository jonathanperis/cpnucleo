namespace GrpcServer.Handlers.UserAssignment;

public sealed class UpdateUserAssignmentHandler : ICommandHandler<UpdateUserAssignmentCommand, UpdateUserAssignmentResult>
{
    public async Task<UpdateUserAssignmentResult> ExecuteAsync(UpdateUserAssignmentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}