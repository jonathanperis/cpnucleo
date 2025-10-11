namespace GrpcServer.Handlers.AssignmentImpediment;

public sealed class ListAssignmentImpedimentsHandler : ICommandHandler<ListAssignmentImpedimentsCommand, ListAssignmentImpedimentsResult>
{
    public async Task<ListAssignmentImpedimentsResult> ExecuteAsync(ListAssignmentImpedimentsCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}