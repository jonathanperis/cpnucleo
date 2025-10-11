namespace GrpcServer.Handlers.Project;

public sealed class UpdateProjectHandler : ICommandHandler<UpdateProjectCommand, UpdateProjectResult>
{
    public async Task<UpdateProjectResult> ExecuteAsync(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}