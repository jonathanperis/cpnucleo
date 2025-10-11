namespace GrpcServer.Handlers.UserProject;

// EF Core
public sealed class RemoveUserProjectHandler(IApplicationDbContext dbContext, ILogger<RemoveUserProjectHandler> logger) : ICommandHandler<RemoveUserProjectCommand, RemoveUserProjectResult>
{
    public async Task<RemoveUserProjectResult> ExecuteAsync(RemoveUserProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if userProject entities exist for Ids: {UserProjectIds}", string.Join(",", command.Ids));
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await dbContext.UserProjects!.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
                if (item is null)
                {
                    logger.LogWarning("UserProject not found with Id: {UserProjectId}", id);
                    return new RemoveUserProjectResult { Success = false };
                }

                logger.LogInformation("Removing userProject entity with Id: {UserProjectId}", id);
                Domain.Entities.UserProject.Remove(item);

                logger.LogInformation("Updating repository for removed entity {UserProjectId}.", id);
                var result = await dbContext.SaveChangesAsync(cancellationToken);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed.");
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Service completed successfully.");

            return new RemoveUserProjectResult { Success = allSuccess };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}