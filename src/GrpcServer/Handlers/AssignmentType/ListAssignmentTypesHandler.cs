namespace GrpcServer.Handlers.AssignmentType;

public sealed class ListAssignmentTypesHandler : ICommandHandler<ListAssignmentTypesCommand, ListAssignmentTypesResult>
{
    public async Task<ListAssignmentTypesResult> ExecuteAsync(ListAssignmentTypesCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}