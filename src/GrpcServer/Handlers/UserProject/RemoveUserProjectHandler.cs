namespace GrpcServer.Handlers.UserProject;

// Dapper Repository Advanced
public sealed class RemoveUserProjectHandler(IUnitOfWork unitOfWork, ILogger<RemoveUserProjectHandler> logger) : ICommandHandler<RemoveUserProjectCommand, RemoveUserProjectResult>
{
    public async Task<RemoveUserProjectResult> ExecuteAsync(RemoveUserProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Checking if userProject entities exist for Ids: {UserProjectIds}", string.Join(",", command.Ids));
            var repository = unitOfWork.GetRepository<Domain.Entities.UserProject>();
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    logger.LogWarning("UserProject not found with Id: {UserProjectId}", id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return new RemoveUserProjectResult 
                    { 
                        Success = false,
                        Message = "UserProject not found."
                    };
                }

                logger.LogInformation("Removing userProject entity with Id: {UserProjectId}", id);
                Domain.Entities.UserProject.Remove(item);

                logger.LogInformation("Deleting userProject entity from repository with Id: {UserProjectId}.", id);
                var result = await repository.DeleteAsync(id);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed, rolling back transaction.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveUserProjectResult 
                { 
                    Success = false,
                    Message = "UserProject not found."
                };
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new RemoveUserProjectResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "UserProject removed successfully." : "Failed to remove UserProject."
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
