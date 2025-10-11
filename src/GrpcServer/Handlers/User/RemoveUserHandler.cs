namespace GrpcServer.Handlers.User;

// EF Core
public sealed class RemoveUserHandler(IApplicationDbContext dbContext, ILogger<RemoveUserHandler> logger) : ICommandHandler<RemoveUserCommand, RemoveUserResult>
{
    public async Task<RemoveUserResult> ExecuteAsync(RemoveUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if user entities exist for Ids: {UserIds}", string.Join(",", command.Ids));
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await dbContext.Users!.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
                if (item is null)
                {
                    logger.LogWarning("User not found with Id: {UserId}", id);
                    return new RemoveUserResult 
                    { 
                        Success = false,
                        Message = "User not found."
                    };
                }

                logger.LogInformation("Removing user entity with Id: {UserId}", id);
                Domain.Entities.User.Remove(item);

                logger.LogInformation("Updating repository for removed entity {UserId}.", id);
                var result = await dbContext.SaveChangesAsync(cancellationToken);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed.");
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Service completed successfully.");

            return new RemoveUserResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "User removed successfully." : "Failed to remove User."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}