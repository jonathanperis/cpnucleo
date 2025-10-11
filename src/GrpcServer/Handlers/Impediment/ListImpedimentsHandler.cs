namespace GrpcServer.Handlers.Impediment;

public sealed class ListImpedimentsHandler : ICommandHandler<ListImpedimentsCommand, ListImpedimentsResult>
{
    public async Task<ListImpedimentsResult> ExecuteAsync(ListImpedimentsCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}