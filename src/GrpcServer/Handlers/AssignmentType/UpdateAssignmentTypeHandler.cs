namespace GrpcServer.Handlers.AssignmentType;

// EF Core
public sealed class UpdateAssignmentTypeHandler(IApplicationDbContext dbContext, ILogger<UpdateAssignmentTypeHandler> logger) : ICommandHandler<UpdateAssignmentTypeCommand, UpdateAssignmentTypeResult>
{
    public async Task<UpdateAssignmentTypeResult> ExecuteAsync(UpdateAssignmentTypeCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an assignmentType entity exists with Id: {AssignmentTypeId}", command.Id);
            var item = await dbContext.AssignmentTypes!.FindAsync([command.Id, cancellationToken], cancellationToken: cancellationToken);

            if (item is null)
            {
                logger.LogWarning("AssignmentType not found with Id: {AssignmentTypeId}", command.Id);
                return new UpdateAssignmentTypeResult 
            { 
                Success = false,
                Message = "AssignmentType not found."
            };
            }

            logger.LogInformation("Updating assignmentType entity with Id: {AssignmentTypeId}", command.Id);
            Domain.Entities.AssignmentType.Update(item, command.Name);

            logger.LogInformation("Updating entity in repository.");
            var success = await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Service completed successfully.");

            return new UpdateAssignmentTypeResult 
            { 
                Success = success,
                Message = success ? "AssignmentType updated successfully." : "Failed to update AssignmentType."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}