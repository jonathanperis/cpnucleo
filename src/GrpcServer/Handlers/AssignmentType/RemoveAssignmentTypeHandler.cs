namespace GrpcServer.Handlers.AssignmentType;

// Dapper Repository Advanced
public sealed class RemoveAssignmentTypeHandler(IUnitOfWork unitOfWork, ILogger<RemoveAssignmentTypeHandler> logger) : ICommandHandler<RemoveAssignmentTypeCommand, RemoveAssignmentTypeResult>
{
    public async Task<RemoveAssignmentTypeResult> ExecuteAsync(RemoveAssignmentTypeCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Checking if assignmentType entities exist for Ids: {AssignmentTypeIds}", string.Join(",", command.Ids));
            var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentType>();
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    logger.LogWarning("AssignmentType not found with Id: {AssignmentTypeId}", id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return new RemoveAssignmentTypeResult 
                    { 
                        Success = false,
                        Message = "AssignmentType not found."
                    };
                }

                logger.LogInformation("Removing assignmentType entity with Id: {AssignmentTypeId}", id);
                Domain.Entities.AssignmentType.Remove(item);

                logger.LogInformation("Deleting assignmentType entity from repository with Id: {AssignmentTypeId}.", id);
                var result = await repository.DeleteAsync(id);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed, rolling back transaction.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveAssignmentTypeResult 
                { 
                    Success = false,
                    Message = "AssignmentType not found."
                };
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new RemoveAssignmentTypeResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "AssignmentType removed successfully." : "Failed to remove AssignmentType."
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