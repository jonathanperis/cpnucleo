namespace GrpcServer.Handlers.UserAssignment;

public sealed class ListUserAssignmentsHandler : ICommandHandler<ListUserAssignmentsCommand, ListUserAssignmentsResult>
{
    public async Task<ListUserAssignmentsResult> ExecuteAsync(ListUserAssignmentsCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}