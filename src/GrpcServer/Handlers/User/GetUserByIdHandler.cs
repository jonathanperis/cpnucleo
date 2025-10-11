namespace GrpcServer.Handlers.User;

public sealed class GetUserByIdHandler : ICommandHandler<GetUserByIdCommand, GetUserByIdResult>
{
    public async Task<GetUserByIdResult> ExecuteAsync(GetUserByIdCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}