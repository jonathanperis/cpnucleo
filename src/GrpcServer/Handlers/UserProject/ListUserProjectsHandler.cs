namespace GrpcServer.Handlers.UserProject;

public sealed class ListUserProjectsHandler : ICommandHandler<ListUserProjectsCommand, ListUserProjectsResult>
{
    public async Task<ListUserProjectsResult> ExecuteAsync(ListUserProjectsCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}