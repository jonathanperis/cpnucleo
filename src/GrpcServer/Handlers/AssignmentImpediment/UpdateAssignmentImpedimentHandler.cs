namespace GrpcServer.Handlers.AssignmentImpediment;

public sealed class UpdateAssignmentImpedimentHandler : ICommandHandler<UpdateAssignmentImpedimentCommand, UpdateAssignmentImpedimentResult>
{
    public async Task<UpdateAssignmentImpedimentResult> ExecuteAsync(UpdateAssignmentImpedimentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}