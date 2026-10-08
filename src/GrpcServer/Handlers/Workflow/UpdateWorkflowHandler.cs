namespace GrpcServer.Handlers.Workflow;

// Dapper Repository Advanced
public sealed class UpdateWorkflowHandler(IUnitOfWork unitOfWork, ILogger<UpdateWorkflowHandler> logger) : ICommandHandler<UpdateWorkflowCommand, UpdateWorkflowResult>
{
    public async Task<UpdateWorkflowResult> ExecuteAsync(UpdateWorkflowCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking if an workflow entity exists with Id: {WorkflowId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.Workflow>();
            var item = await repository.GetByIdAsync(command.Id, cancellationToken);

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

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            
            logger.LogInformation("Updating entity in repository.");
            var success = await repository.UpdateAsync(item, cancellationToken);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            return new UpdateWorkflowResult 
            { 
                Success = success,
                Message = success ? "Workflow updated successfully." : "Failed to update Workflow."
            };
        }
        catch
        {
            // Rejections and failures are translated (and unexpected ones logged) by the gRPC pipeline.
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}