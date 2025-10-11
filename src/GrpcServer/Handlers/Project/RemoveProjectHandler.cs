namespace GrpcServer.Handlers.Project;

public sealed class RemoveProjectHandler : ICommandHandler<RemoveProjectCommand, RemoveProjectResult>
{
    public async Task<RemoveProjectResult> ExecuteAsync(RemoveProjectCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}