namespace GrpcServer.Handlers.User;

// EF Core
public sealed class UpdateUserHandler(IApplicationDbContext dbContext, ILogger<UpdateUserHandler> logger) : ICommandHandler<UpdateUserCommand, UpdateUserResult>
{
    public async Task<UpdateUserResult> ExecuteAsync(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an user entity exists with Id: {UserId}", command.Id);
            var item = await dbContext.Users!.FindAsync([command.Id, cancellationToken], cancellationToken: cancellationToken);

            if (item is null)
            {
                logger.LogWarning("User not found with Id: {UserId}", command.Id);
                return new UpdateUserResult 
            { 
                Success = false,
                Message = "User not found."
            };
            }

            logger.LogInformation("Updating user entity with Id: {UserId}", command.Id);
            Domain.Entities.User.Update(item, command.Name, command.Password);

            logger.LogInformation("Updating entity in repository.");
            var success = await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Service completed successfully.");

            return new UpdateUserResult 
            { 
                Success = success,
                Message = success ? "User updated successfully." : "Failed to update User."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}