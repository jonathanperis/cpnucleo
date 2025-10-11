namespace GrpcServer.Handlers.Impediment;

// Dapper Repository Advanced
public sealed class RemoveImpedimentHandler(IUnitOfWork unitOfWork, ILogger<RemoveImpedimentHandler> logger) : ICommandHandler<RemoveImpedimentCommand, RemoveImpedimentResult>
{
    public async Task<RemoveImpedimentResult> ExecuteAsync(RemoveImpedimentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Checking if impediment entities exist for Ids: {ImpedimentIds}", string.Join(",", command.Ids));
            var repository = unitOfWork.GetRepository<Domain.Entities.Impediment>();
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    logger.LogWarning("Impediment not found with Id: {ImpedimentId}", id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return new RemoveImpedimentResult 
                    { 
                        Success = false,
                        Message = "Impediment not found."
                    };
                }

                logger.LogInformation("Removing impediment entity with Id: {ImpedimentId}", id);
                Domain.Entities.Impediment.Remove(item);

                logger.LogInformation("Deleting impediment entity from repository with Id: {ImpedimentId}.", id);
                var result = await repository.DeleteAsync(id);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed, rolling back transaction.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveImpedimentResult 
                { 
                    Success = false,
                    Message = "Impediment not found."
                };
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new RemoveImpedimentResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "Impediment removed successfully." : "Failed to remove Impediment."
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