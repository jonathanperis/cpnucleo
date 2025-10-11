namespace GrpcServer.Handlers.Assignment;

public sealed class ListAssignmentsHandler : ICommandHandler<ListAssignmentsCommand, ListAssignmentsResult>
{
    public async Task<ListAssignmentsResult> ExecuteAsync(ListAssignmentsCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}