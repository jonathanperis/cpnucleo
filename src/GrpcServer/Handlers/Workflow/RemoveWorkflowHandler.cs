namespace GrpcServer.Handlers.Workflow;

// EF Core
public sealed class RemoveWorkflowHandler(IApplicationDbContext dbContext, ILogger<RemoveWorkflowHandler> logger) : ICommandHandler<RemoveWorkflowCommand, RemoveWorkflowResult>
{
    public async Task<RemoveWorkflowResult> ExecuteAsync(RemoveWorkflowCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if workflow entities exist for Ids: {WorkflowIds}", string.Join(",", command.Ids));
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await dbContext.Workflows!.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
                if (item is null)
                {
                    logger.LogWarning("Workflow not found with Id: {WorkflowId}", id);
                    return new RemoveWorkflowResult { Success = false };
                }

                logger.LogInformation("Removing workflow entity with Id: {WorkflowId}", id);
                Domain.Entities.Workflow.Remove(item);

                logger.LogInformation("Updating repository for removed entity {WorkflowId}.", id);
                var result = await dbContext.SaveChangesAsync(cancellationToken);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed.");
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Service completed successfully.");

            return new RemoveWorkflowResult { Success = allSuccess };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}