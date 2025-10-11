namespace GrpcServer.Handlers.AssignmentImpediment;

// EF Core
public sealed class RemoveAssignmentImpedimentHandler(IApplicationDbContext dbContext, ILogger<RemoveAssignmentImpedimentHandler> logger) : ICommandHandler<RemoveAssignmentImpedimentCommand, RemoveAssignmentImpedimentResult>
{
    public async Task<RemoveAssignmentImpedimentResult> ExecuteAsync(RemoveAssignmentImpedimentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if assignmentImpediment entities exist for Ids: {AssignmentImpedimentIds}", string.Join(",", command.Ids));
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await dbContext.AssignmentImpediments!.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
                if (item is null)
                {
                    logger.LogWarning("AssignmentImpediment not found with Id: {AssignmentImpedimentId}", id);
                    return new RemoveAssignmentImpedimentResult { Success = false };
                }

                logger.LogInformation("Removing assignmentImpediment entity with Id: {AssignmentImpedimentId}", id);
                Domain.Entities.AssignmentImpediment.Remove(item);

                logger.LogInformation("Updating repository for removed entity {AssignmentImpedimentId}.", id);
                var result = await dbContext.SaveChangesAsync(cancellationToken);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed.");
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Service completed successfully.");

            return new RemoveAssignmentImpedimentResult { Success = allSuccess };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}