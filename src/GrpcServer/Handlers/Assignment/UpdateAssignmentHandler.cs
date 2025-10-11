namespace GrpcServer.Handlers.Assignment;

// EF Core
public sealed class UpdateAssignmentHandler(IApplicationDbContext dbContext, ILogger<UpdateAssignmentHandler> logger) : ICommandHandler<UpdateAssignmentCommand, UpdateAssignmentResult>
{
    public async Task<UpdateAssignmentResult> ExecuteAsync(UpdateAssignmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an assignment entity exists with Id: {AssignmentId}", command.Id);
            var item = await dbContext.Assignments!.FindAsync([command.Id, cancellationToken], cancellationToken: cancellationToken);

            if (item is null)
            {
                logger.LogWarning("Assignment not found with Id: {AssignmentId}", command.Id);
                return new UpdateAssignmentResult { Success = false };
            }

            logger.LogInformation("Updating assignment entity with Id: {AssignmentId}", command.Id);
            Domain.Entities.Assignment.Update(item,
                                             command.Name,
                                             command.Description,
                                             command.StartDate,
                                             command.EndDate,
                                             command.AmountHours,
                                             command.ProjectId,
                                             command.WorkflowId,
                                             command.UserId,
                                             command.AssignmentTypeId);

            logger.LogInformation("Updating entity in repository.");
            var success = await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Service completed successfully.");

            return new UpdateAssignmentResult { Success = success };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}