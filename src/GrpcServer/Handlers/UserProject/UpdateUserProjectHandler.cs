namespace GrpcServer.Handlers.UserProject;

public sealed class UpdateUserProjectHandler : ICommandHandler<UpdateUserProjectCommand, UpdateUserProjectResult>
{
    public async Task<UpdateUserProjectResult> ExecuteAsync(UpdateUserProjectCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}