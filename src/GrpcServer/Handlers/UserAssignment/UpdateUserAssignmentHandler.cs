namespace GrpcServer.Handlers.UserAssignment;

// EF Core
public sealed class UpdateUserAssignmentHandler(IApplicationDbContext dbContext, ILogger<UpdateUserAssignmentHandler> logger) : ICommandHandler<UpdateUserAssignmentCommand, UpdateUserAssignmentResult>
{
    public async Task<UpdateUserAssignmentResult> ExecuteAsync(UpdateUserAssignmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an userAssignment entity exists with Id: {UserAssignmentId}", command.Id);
            var item = await dbContext.UserAssignments!.FindAsync([command.Id, cancellationToken], cancellationToken: cancellationToken);

            if (item is null)
            {
                logger.LogWarning("UserAssignment not found with Id: {UserAssignmentId}", command.Id);
                return new UpdateUserAssignmentResult 
                { 
                    Success = false,
                    Message = "UserAssignment not found."
                };
            }

            logger.LogInformation("Updating userAssignment entity with Id: {UserAssignmentId}", command.Id);
            Domain.Entities.UserAssignment.Update(item, command.UserId, command.AssignmentId);

            logger.LogInformation("Updating entity in repository.");
            var success = await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Service completed successfully.");

            return new UpdateUserAssignmentResult 
            { 
                Success = success,
                Message = success ? "UserAssignment updated successfully." : "Failed to update UserAssignment."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}