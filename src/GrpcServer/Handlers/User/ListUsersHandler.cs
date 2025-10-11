namespace GrpcServer.Handlers.User;

public sealed class ListUsersHandler : ICommandHandler<ListUsersCommand, ListUsersResult>
{
    public async Task<ListUsersResult> ExecuteAsync(ListUsersCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}