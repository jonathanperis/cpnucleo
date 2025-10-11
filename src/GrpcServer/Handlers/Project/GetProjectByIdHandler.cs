namespace GrpcServer.Handlers.Project;

public sealed class GetProjectByIdHandler : ICommandHandler<GetProjectByIdCommand, GetProjectByIdResult>
{
    public async Task<GetProjectByIdResult> ExecuteAsync(GetProjectByIdCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}