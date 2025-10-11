namespace GrpcServer.Handlers.Workflow;

// EF Core
public sealed class UpdateWorkflowHandler(IApplicationDbContext dbContext, ILogger<UpdateWorkflowHandler> logger) : ICommandHandler<UpdateWorkflowCommand, UpdateWorkflowResult>
{
    public async Task<UpdateWorkflowResult> ExecuteAsync(UpdateWorkflowCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an workflow entity exists with Id: {WorkflowId}", command.Id);
            var item = await dbContext.Workflows!.FindAsync([command.Id, cancellationToken], cancellationToken: cancellationToken);

            if (item is null)
            {
                logger.LogWarning("Workflow not found with Id: {WorkflowId}", command.Id);
                return new UpdateWorkflowResult 
            { 
                Success = false,
                Message = "Workflow not found."
            };
            }

            logger.LogInformation("Updating workflow entity with Id: {WorkflowId}", command.Id);
            Domain.Entities.Workflow.Update(item, command.Name, command.Order);

            logger.LogInformation("Updating entity in repository.");
            var success = await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Service completed successfully.");

            return new UpdateWorkflowResult 
            { 
                Success = success,
                Message = success ? "Workflow updated successfully." : "Failed to update Workflow."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}