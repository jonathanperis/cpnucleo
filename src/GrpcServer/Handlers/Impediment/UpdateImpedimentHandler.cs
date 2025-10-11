namespace GrpcServer.Handlers.Impediment;

public sealed class UpdateImpedimentHandler : ICommandHandler<UpdateImpedimentCommand, UpdateImpedimentResult>
{
    public async Task<UpdateImpedimentResult> ExecuteAsync(UpdateImpedimentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}