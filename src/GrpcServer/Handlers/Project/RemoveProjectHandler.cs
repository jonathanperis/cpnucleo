namespace GrpcServer.Handlers.Project;

// Dapper Repository Advanced
public sealed class RemoveProjectHandler(IUnitOfWork unitOfWork, ILogger<RemoveProjectHandler> logger) : ICommandHandler<RemoveProjectCommand, RemoveProjectResult>
{
    public async Task<RemoveProjectResult> ExecuteAsync(RemoveProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Checking if project entities exist for Ids: {ProjectIds}", string.Join(",", command.Ids));
            var repository = unitOfWork.GetRepository<Domain.Entities.Project>();
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    logger.LogWarning("Project not found with Id: {ProjectId}", id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return new RemoveProjectResult 
                    { 
                        Success = false,
                        Message = "Project not found."
                    };
                }

                logger.LogInformation("Removing project entity with Id: {ProjectId}", id);
                Domain.Entities.Project.Remove(item);

                logger.LogInformation("Deleting project entity from repository with Id: {ProjectId}.", id);
                var result = await repository.DeleteAsync(id);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed, rolling back transaction.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveProjectResult 
                { 
                    Success = false,
                    Message = "Project not found."
                };
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new RemoveProjectResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "Project removed successfully." : "Failed to remove Project."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command. Rolling back transaction.");
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}