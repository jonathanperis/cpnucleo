namespace GrpcServer.Handlers.UserProject;

public sealed class CreateUserProjectHandler : ICommandHandler<CreateUserProjectCommand, CreateUserProjectResult>
{
    public async Task<CreateUserProjectResult> ExecuteAsync(CreateUserProjectCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}