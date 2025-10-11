namespace GrpcServer.Handlers.Assignment;

// Dapper Repository Advanced
public sealed class RemoveAssignmentHandler(IUnitOfWork unitOfWork, ILogger<RemoveAssignmentHandler> logger) : ICommandHandler<RemoveAssignmentCommand, RemoveAssignmentResult>
{
    public async Task<RemoveAssignmentResult> ExecuteAsync(RemoveAssignmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Checking if assignment entities exist for Ids: {AssignmentIds}", string.Join(",", command.Ids));
            var repository = unitOfWork.GetRepository<Domain.Entities.Assignment>();
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    logger.LogWarning("Assignment not found with Id: {AssignmentId}", id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return new RemoveAssignmentResult 
                    { 
                        Success = false,
                        Message = "Assignment not found."
                    };
                }

                logger.LogInformation("Removing assignment entity with Id: {AssignmentId}", id);
                Domain.Entities.Assignment.Remove(item);

                logger.LogInformation("Deleting assignment entity from repository with Id: {AssignmentId}.", id);
                var result = await repository.DeleteAsync(id);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed, rolling back transaction.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveAssignmentResult 
                { 
                    Success = false,
                    Message = "Assignment not found."
                };
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new RemoveAssignmentResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "Assignment removed successfully." : "Failed to remove Assignment."
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