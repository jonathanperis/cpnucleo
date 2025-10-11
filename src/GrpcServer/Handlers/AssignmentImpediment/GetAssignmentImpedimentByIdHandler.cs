namespace GrpcServer.Handlers.AssignmentImpediment;

public sealed class GetAssignmentImpedimentByIdHandler : ICommandHandler<GetAssignmentImpedimentByIdCommand, GetAssignmentImpedimentByIdResult>
{
    public async Task<GetAssignmentImpedimentByIdResult> ExecuteAsync(GetAssignmentImpedimentByIdCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}