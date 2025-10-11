namespace GrpcServer.Handlers.AssignmentType;

// EF Core
public sealed class RemoveAssignmentTypeHandler(IApplicationDbContext dbContext, ILogger<RemoveAssignmentTypeHandler> logger) : ICommandHandler<RemoveAssignmentTypeCommand, RemoveAssignmentTypeResult>
{
    public async Task<RemoveAssignmentTypeResult> ExecuteAsync(RemoveAssignmentTypeCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if assignmentType entities exist for Ids: {AssignmentTypeIds}", string.Join(",", command.Ids));
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await dbContext.AssignmentTypes!.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
                if (item is null)
                {
                    logger.LogWarning("AssignmentType not found with Id: {AssignmentTypeId}", id);
                    return new RemoveAssignmentTypeResult { Success = false };
                }

                logger.LogInformation("Removing assignmentType entity with Id: {AssignmentTypeId}", id);
                Domain.Entities.AssignmentType.Remove(item);

                logger.LogInformation("Updating repository for removed entity {AssignmentTypeId}.", id);
                var result = await dbContext.SaveChangesAsync(cancellationToken);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed.");
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Service completed successfully.");

            return new RemoveAssignmentTypeResult { Success = allSuccess };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}