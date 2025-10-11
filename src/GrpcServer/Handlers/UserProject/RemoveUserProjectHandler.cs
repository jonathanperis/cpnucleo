namespace GrpcServer.Handlers.UserProject;

public sealed class RemoveUserProjectHandler : ICommandHandler<RemoveUserProjectCommand, RemoveUserProjectResult>
{
    public async Task<RemoveUserProjectResult> ExecuteAsync(RemoveUserProjectCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}