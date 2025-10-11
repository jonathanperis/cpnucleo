namespace GrpcServer.Handlers.AssignmentImpediment;

// Dapper Repository Advanced
public sealed class RemoveAssignmentImpedimentHandler(IUnitOfWork unitOfWork, ILogger<RemoveAssignmentImpedimentHandler> logger) : ICommandHandler<RemoveAssignmentImpedimentCommand, RemoveAssignmentImpedimentResult>
{
    public async Task<RemoveAssignmentImpedimentResult> ExecuteAsync(RemoveAssignmentImpedimentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Checking if assignmentImpediment entities exist for Ids: {AssignmentImpedimentIds}", string.Join(",", command.Ids));
            var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentImpediment>();
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    logger.LogWarning("AssignmentImpediment not found with Id: {AssignmentImpedimentId}", id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return new RemoveAssignmentImpedimentResult 
                    { 
                        Success = false,
                        Message = "AssignmentImpediment not found."
                    };
                }

                logger.LogInformation("Removing assignmentImpediment entity with Id: {AssignmentImpedimentId}", id);
                Domain.Entities.AssignmentImpediment.Remove(item);

                logger.LogInformation("Deleting assignmentImpediment entity from repository with Id: {AssignmentImpedimentId}.", id);
                var result = await repository.DeleteAsync(id);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed, rolling back transaction.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveAssignmentImpedimentResult 
                { 
                    Success = false,
                    Message = "AssignmentImpediment not found."
                };
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new RemoveAssignmentImpedimentResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "AssignmentImpediment removed successfully." : "Failed to remove AssignmentImpediment."
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