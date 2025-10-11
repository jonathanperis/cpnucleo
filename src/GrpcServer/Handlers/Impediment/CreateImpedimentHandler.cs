namespace GrpcServer.Handlers.Impediment;

public sealed class CreateImpedimentHandler : ICommandHandler<CreateImpedimentCommand, CreateImpedimentResult>
{
    public async Task<CreateImpedimentResult> ExecuteAsync(CreateImpedimentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}