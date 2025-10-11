namespace GrpcServer.Handlers.Workflow;

// Dapper Repository Advanced
public sealed class RemoveWorkflowHandler(IUnitOfWork unitOfWork, ILogger<RemoveWorkflowHandler> logger) : ICommandHandler<RemoveWorkflowCommand, RemoveWorkflowResult>
{
    public async Task<RemoveWorkflowResult> ExecuteAsync(RemoveWorkflowCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Checking if workflow entities exist for Ids: {WorkflowIds}", string.Join(",", command.Ids));
            var repository = unitOfWork.GetRepository<Domain.Entities.Workflow>();
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    logger.LogWarning("Workflow not found with Id: {WorkflowId}", id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return new RemoveWorkflowResult 
                    { 
                        Success = false,
                        Message = "Workflow not found."
                    };
                }

                logger.LogInformation("Removing workflow entity with Id: {WorkflowId}", id);
                Domain.Entities.Workflow.Remove(item);

                logger.LogInformation("Deleting workflow entity from repository with Id: {WorkflowId}.", id);
                var result = await repository.DeleteAsync(id);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed, rolling back transaction.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveWorkflowResult 
                { 
                    Success = false,
                    Message = "Workflow not found."
                };
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new RemoveWorkflowResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "Workflow removed successfully." : "Failed to remove Workflow."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command. Rolling back transaction.");
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}