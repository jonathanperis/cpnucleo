namespace GrpcServer.Handlers.Project;

public sealed class ListProjectsHandler : ICommandHandler<ListProjectsCommand, ListProjectsResult>
{
    public async Task<ListProjectsResult> ExecuteAsync(ListProjectsCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}