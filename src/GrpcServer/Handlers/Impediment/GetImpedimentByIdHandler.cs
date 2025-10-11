namespace GrpcServer.Handlers.Impediment;

public sealed class GetImpedimentByIdHandler : ICommandHandler<GetImpedimentByIdCommand, GetImpedimentByIdResult>
{
    public async Task<GetImpedimentByIdResult> ExecuteAsync(GetImpedimentByIdCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}