namespace GrpcServer.Handlers.UserProject;

public sealed class GetUserProjectByIdHandler : ICommandHandler<GetUserProjectByIdCommand, GetUserProjectByIdResult>
{
    public async Task<GetUserProjectByIdResult> ExecuteAsync(GetUserProjectByIdCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}