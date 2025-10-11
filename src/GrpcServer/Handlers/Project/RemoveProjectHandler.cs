namespace GrpcServer.Handlers.Project;

// Dapper Repository Basic
public sealed class RemoveProjectHandler(IProjectRepository repository, ILogger<RemoveProjectHandler> logger) : ICommandHandler<RemoveProjectCommand, RemoveProjectResult>
{
    public async Task<RemoveProjectResult> ExecuteAsync(RemoveProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if project entities exist for Ids: {ProjectIds}", string.Join(",", command.Ids));
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    logger.LogWarning("Project not found with Id: {ProjectId}", id);
                    return new RemoveProjectResult { Success = false };
                }

                logger.LogInformation("Removing project entity with Id: {ProjectId}", id);
                Domain.Entities.Project.Remove(item);

                logger.LogInformation("Deleting entity from repository {ProjectId}.", id);
                var result = await repository.DeleteAsync(id);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed.");
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Service completed successfully.");

            return new RemoveProjectResult { Success = allSuccess };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}