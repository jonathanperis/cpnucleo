namespace GrpcServer.Handlers.Project;

// Dapper Repository Advanced
public sealed class RemoveProjectHandler(IUnitOfWork unitOfWork, ILogger<RemoveProjectHandler> logger) : ICommandHandler<RemoveProjectCommand, RemoveProjectResult>
{
    public async Task<RemoveProjectResult> ExecuteAsync(RemoveProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");
        var ids = BatchIds.Normalize(command.Ids);

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Removing {Count} project entities atomically.", ids.Length);
            var repository = unitOfWork.GetRepository<Domain.Entities.Project>();
            var success = await repository.RemoveManyAsync(ids, cancellationToken);

            if (!success)
            {
                logger.LogWarning("At least one project to remove was not found; rolling back.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveProjectResult
                {
                    Success = false,
                    Message = "Project not found."
                };
            }

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");
            return new RemoveProjectResult
            {
                Success = true,
                Message = "Project removed successfully."
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
