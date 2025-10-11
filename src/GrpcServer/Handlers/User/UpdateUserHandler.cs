namespace GrpcServer.Handlers.User;

public sealed class UpdateUserHandler : ICommandHandler<UpdateUserCommand, UpdateUserResult>
{
    public async Task<UpdateUserResult> ExecuteAsync(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}