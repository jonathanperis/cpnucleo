namespace GrpcServer.Handlers.UserAssignment;

public sealed class GetUserAssignmentByIdHandler : ICommandHandler<GetUserAssignmentByIdCommand, GetUserAssignmentByIdResult>
{
    public async Task<GetUserAssignmentByIdResult> ExecuteAsync(GetUserAssignmentByIdCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}