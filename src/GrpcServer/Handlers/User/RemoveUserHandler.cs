namespace GrpcServer.Handlers.User;

// Dapper Repository Advanced
public sealed class RemoveUserHandler(IUnitOfWork unitOfWork, ILogger<RemoveUserHandler> logger) : ICommandHandler<RemoveUserCommand, RemoveUserResult>
{
    public async Task<RemoveUserResult> ExecuteAsync(RemoveUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Checking if user entities exist for Ids: {UserIds}", string.Join(",", command.Ids));
            var repository = unitOfWork.GetRepository<Domain.Entities.User>();
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    logger.LogWarning("User not found with Id: {UserId}", id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return new RemoveUserResult 
                    { 
                        Success = false,
                        Message = "User not found."
                    };
                }

                logger.LogInformation("Removing user entity with Id: {UserId}", id);
                Domain.Entities.User.Remove(item);

                logger.LogInformation("Deleting user entity from repository with Id: {UserId}.", id);
                var result = await repository.DeleteAsync(id);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed, rolling back transaction.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveUserResult 
                { 
                    Success = false,
                    Message = "User not found."
                };
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new RemoveUserResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "User removed successfully." : "Failed to remove User."
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