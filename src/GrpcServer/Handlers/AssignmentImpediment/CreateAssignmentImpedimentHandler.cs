namespace GrpcServer.Handlers.AssignmentImpediment;

public sealed class CreateAssignmentImpedimentHandler : ICommandHandler<CreateAssignmentImpedimentCommand, CreateAssignmentImpedimentResult>
{
    public async Task<CreateAssignmentImpedimentResult> ExecuteAsync(CreateAssignmentImpedimentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}