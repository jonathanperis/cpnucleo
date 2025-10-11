namespace GrpcServer.Handlers.User;

public sealed class CreateUserHandler : ICommandHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> ExecuteAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}