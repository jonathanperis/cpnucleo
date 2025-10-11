namespace GrpcServer.Handlers.UserAssignment;

// Dapper Repository Advanced
public sealed class RemoveUserAssignmentHandler(IUnitOfWork unitOfWork, ILogger<RemoveUserAssignmentHandler> logger) : ICommandHandler<RemoveUserAssignmentCommand, RemoveUserAssignmentResult>
{
    public async Task<RemoveUserAssignmentResult> ExecuteAsync(RemoveUserAssignmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Checking if userAssignment entities exist for Ids: {UserAssignmentIds}", string.Join(",", command.Ids));
            var repository = unitOfWork.GetRepository<Domain.Entities.UserAssignment>();
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    logger.LogWarning("UserAssignment not found with Id: {UserAssignmentId}", id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return new RemoveUserAssignmentResult 
                    { 
                        Success = false,
                        Message = "UserAssignment not found."
                    };
                }

                logger.LogInformation("Removing userAssignment entity with Id: {UserAssignmentId}", id);
                Domain.Entities.UserAssignment.Remove(item);

                logger.LogInformation("Deleting userAssignment entity from repository with Id: {UserAssignmentId}.", id);
                var result = await repository.DeleteAsync(id);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed, rolling back transaction.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveUserAssignmentResult 
                { 
                    Success = false,
                    Message = "UserAssignment not found."
                };
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new RemoveUserAssignmentResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "UserAssignment removed successfully." : "Failed to remove UserAssignment."
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