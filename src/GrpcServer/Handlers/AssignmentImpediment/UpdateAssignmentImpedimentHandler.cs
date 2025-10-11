namespace GrpcServer.Handlers.AssignmentImpediment;

// EF Core
public sealed class UpdateAssignmentImpedimentHandler(IApplicationDbContext dbContext, ILogger<UpdateAssignmentImpedimentHandler> logger) : ICommandHandler<UpdateAssignmentImpedimentCommand, UpdateAssignmentImpedimentResult>
{
    public async Task<UpdateAssignmentImpedimentResult> ExecuteAsync(UpdateAssignmentImpedimentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an assignmentImpediment entity exists with Id: {AssignmentImpedimentId}", command.Id);
            var item = await dbContext.AssignmentImpediments!.FindAsync([command.Id, cancellationToken], cancellationToken: cancellationToken);

            if (item is null)
            {
                logger.LogWarning("AssignmentImpediment not found with Id: {AssignmentImpedimentId}", command.Id);
                return new UpdateAssignmentImpedimentResult { Success = false };
            }

            logger.LogInformation("Updating assignmentImpediment entity with Id: {AssignmentImpedimentId}", command.Id);
            Domain.Entities.AssignmentImpediment.Update(item, command.Description, command.AssignmentId, command.ImpedimentId);

            logger.LogInformation("Updating entity in repository.");
            var success = await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Service completed successfully.");

            return new UpdateAssignmentImpedimentResult { Success = success };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}