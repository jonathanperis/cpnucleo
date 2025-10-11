namespace GrpcServer.Handlers.Impediment;

public sealed class RemoveImpedimentHandler : ICommandHandler<RemoveImpedimentCommand, RemoveImpedimentResult>
{
    public async Task<RemoveImpedimentResult> ExecuteAsync(RemoveImpedimentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}