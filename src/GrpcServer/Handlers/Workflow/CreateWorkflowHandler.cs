namespace GrpcServer.Handlers.Workflow;

// Dapper Repository Advanced
public sealed class CreateWorkflowHandler(IUnitOfWork unitOfWork, ILogger<CreateWorkflowHandler> logger) : ICommandHandler<CreateWorkflowCommand, CreateWorkflowResult>
{
    public async Task<CreateWorkflowResult> ExecuteAsync(CreateWorkflowCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {WorkflowId}", command.Name, command.Id);

            logger.LogInformation("Checking if an workflow entity exists with Id: {WorkflowId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.Workflow>();
            var itemExists = await repository.ExistsAsync(command.Id);

            if (itemExists)
            {
                logger.LogWarning("Workflow Id conflict for Id: {WorkflowId}", command.Id);
                return new CreateWorkflowResult
                {
                    Success = false,
                    Message = "this Id is already in use!"
                };
            }

            logger.LogInformation("Validation passed, proceeding to create new workflow entity.");
            var newItem = Domain.Entities.Workflow.Create(command.Name, command.Order, command.Id);
            logger.LogInformation("Created new workflow entity with Id: {WorkflowId}", newItem.Id);

            logger.LogInformation("Adding workflow to repository.");
            var createdId = await repository.AddAsync(newItem);

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Fetching workflow by Id: {WorkflowId}", newItem.Id);
            var createdItem = await repository.GetByIdAsync(newItem.Id);

            var result = new CreateWorkflowResult
            {
                Success = true,
                Message = "Workflow created successfully.",
                Workflow = createdItem!.MapToDto()
            };

            logger.LogInformation("Service completed successfully.");

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command. Rolling back transaction.");
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}