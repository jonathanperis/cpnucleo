namespace GrpcServer.Handlers.Project;

public sealed class CreateProjectHandler : ICommandHandler<CreateProjectCommand, CreateProjectResult>
{
    public async Task<CreateProjectResult> ExecuteAsync(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}