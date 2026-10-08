namespace GrpcServer.Handlers.Workflow;

// Dapper Repository Advanced
public sealed class RemoveWorkflowHandler(IUnitOfWork unitOfWork, ILogger<RemoveWorkflowHandler> logger) : ICommandHandler<RemoveWorkflowCommand, RemoveWorkflowResult>
{
    public async Task<RemoveWorkflowResult> ExecuteAsync(RemoveWorkflowCommand command, CancellationToken cancellationToken)
    {
        var ids = BatchIds.Normalize(command.Ids);

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Removing {Count} workflow entities atomically.", ids.Length);
            var repository = unitOfWork.GetRepository<Domain.Entities.Workflow>();
            var success = await repository.RemoveManyAsync(ids, cancellationToken);

            if (!success)
            {
                logger.LogWarning("At least one workflow to remove was not found; rolling back.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveWorkflowResult
                {
                    Success = false,
                    Message = "Workflow not found."
                };
            }

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            return new RemoveWorkflowResult
            {
                Success = true,
                Message = "Workflow removed successfully."
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
