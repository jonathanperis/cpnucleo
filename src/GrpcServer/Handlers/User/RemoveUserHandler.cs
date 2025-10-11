namespace GrpcServer.Handlers.User;

public sealed class RemoveUserHandler : ICommandHandler<RemoveUserCommand, RemoveUserResult>
{
    public async Task<RemoveUserResult> ExecuteAsync(RemoveUserCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}