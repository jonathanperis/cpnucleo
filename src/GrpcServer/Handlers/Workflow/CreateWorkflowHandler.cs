namespace GrpcServer.Handlers.Workflow;

// EF Core
public sealed class CreateWorkflowHandler(IApplicationDbContext dbContext, ILogger<CreateWorkflowHandler> logger) : ICommandHandler<CreateWorkflowCommand, CreateWorkflowResult>
{
    public async Task<CreateWorkflowResult> ExecuteAsync(CreateWorkflowCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {WorkflowId}", command.Name, command.Id);

            logger.LogInformation("Checking if an workflow entity exists with Id: {WorkflowId}", command.Id);
            var itemExists = dbContext.Workflows!.Any(x => x.Id == command.Id);

            if (itemExists)
            {
                logger.LogWarning("Workflow Id conflict for Id: {WorkflowId}", command.Id);
                return new CreateWorkflowResult();
            }

            logger.LogInformation("Validation passed, proceeding to create new workflow entity.");
            var newItem = Domain.Entities.Workflow.Create(command.Name, command.Order, command.Id);
            logger.LogInformation("Created new workflow entity with Id: {WorkflowId}", newItem.Id);

            logger.LogInformation("Adding workflow to repository.");
            await dbContext.Workflows!.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Fetching workflow by Id: {WorkflowId}", newItem.Id);
            var createdItem = await dbContext.Workflows!.FindAsync([newItem.Id, cancellationToken], cancellationToken: cancellationToken);

            var result = new CreateWorkflowResult
            {
                Workflow = createdItem!.MapToDto()
            };

            logger.LogInformation("Service completed successfully.");

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}