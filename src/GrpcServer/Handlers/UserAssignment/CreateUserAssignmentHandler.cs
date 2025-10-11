namespace GrpcServer.Handlers.UserAssignment;

public sealed class CreateUserAssignmentHandler : ICommandHandler<CreateUserAssignmentCommand, CreateUserAssignmentResult>
{
    public async Task<CreateUserAssignmentResult> ExecuteAsync(CreateUserAssignmentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}