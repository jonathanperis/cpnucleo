namespace GrpcServer.Handlers.Impediment;

// EF Core
public sealed class RemoveImpedimentHandler(IApplicationDbContext dbContext, ILogger<RemoveImpedimentHandler> logger) : ICommandHandler<RemoveImpedimentCommand, RemoveImpedimentResult>
{
    public async Task<RemoveImpedimentResult> ExecuteAsync(RemoveImpedimentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if impediment entities exist for Ids: {ImpedimentIds}", string.Join(",", command.Ids));
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await dbContext.Impediments!.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
                if (item is null)
                {
                    logger.LogWarning("Impediment not found with Id: {ImpedimentId}", id);
                    return new RemoveImpedimentResult 
                    { 
                        Success = false,
                        Message = "Impediment not found."
                    };
                }

                logger.LogInformation("Removing impediment entity with Id: {ImpedimentId}", id);
                Domain.Entities.Impediment.Remove(item);

                logger.LogInformation("Updating repository for removed entity {ImpedimentId}.", id);
                var result = await dbContext.SaveChangesAsync(cancellationToken);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed.");
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Service completed successfully.");

            return new RemoveImpedimentResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "Impediment removed successfully." : "Failed to remove Impediment."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}