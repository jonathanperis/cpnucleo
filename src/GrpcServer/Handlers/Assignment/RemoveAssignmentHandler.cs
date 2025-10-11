namespace GrpcServer.Handlers.Assignment;

// EF Core
public sealed class RemoveAssignmentHandler(IApplicationDbContext dbContext, ILogger<RemoveAssignmentHandler> logger) : ICommandHandler<RemoveAssignmentCommand, RemoveAssignmentResult>
{
    public async Task<RemoveAssignmentResult> ExecuteAsync(RemoveAssignmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if assignment entities exist for Ids: {AssignmentIds}", string.Join(",", command.Ids));
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await dbContext.Assignments!.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
                if (item is null)
                {
                    logger.LogWarning("Assignment not found with Id: {AssignmentId}", id);
                    return new RemoveAssignmentResult 
                    { 
                        Success = false,
                        Message = "Assignment not found."
                    };
                }

                logger.LogInformation("Removing assignment entity with Id: {AssignmentId}", id);
                Domain.Entities.Assignment.Remove(item);

                logger.LogInformation("Updating repository for removed entity {AssignmentId}.", id);
                var result = await dbContext.SaveChangesAsync(cancellationToken);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed.");
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Service completed successfully.");

            return new RemoveAssignmentResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "Assignment removed successfully." : "Failed to remove Assignment."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}