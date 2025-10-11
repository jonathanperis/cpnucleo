namespace GrpcServer.Handlers.AssignmentImpediment;

public sealed class RemoveAssignmentImpedimentHandler : ICommandHandler<RemoveAssignmentImpedimentCommand, RemoveAssignmentImpedimentResult>
{
    public async Task<RemoveAssignmentImpedimentResult> ExecuteAsync(RemoveAssignmentImpedimentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}