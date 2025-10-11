namespace GrpcServer.Handlers.UserAssignment;

// EF Core
public sealed class RemoveUserAssignmentHandler(IApplicationDbContext dbContext, ILogger<RemoveUserAssignmentHandler> logger) : ICommandHandler<RemoveUserAssignmentCommand, RemoveUserAssignmentResult>
{
    public async Task<RemoveUserAssignmentResult> ExecuteAsync(RemoveUserAssignmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if userAssignment entities exist for Ids: {UserAssignmentIds}", string.Join(",", command.Ids));
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await dbContext.UserAssignments!.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
                if (item is null)
                {
                    logger.LogWarning("UserAssignment not found with Id: {UserAssignmentId}", id);
                    return new RemoveUserAssignmentResult 
                    { 
                        Success = false,
                        Message = "UserAssignment not found."
                    };
                }

                logger.LogInformation("Removing userAssignment entity with Id: {UserAssignmentId}", id);
                Domain.Entities.UserAssignment.Remove(item);

                logger.LogInformation("Updating repository for removed entity {UserAssignmentId}.", id);
                var result = await dbContext.SaveChangesAsync(cancellationToken);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed.");
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Service completed successfully.");

            return new RemoveUserAssignmentResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "UserAssignment removed successfully." : "Failed to remove UserAssignment."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}