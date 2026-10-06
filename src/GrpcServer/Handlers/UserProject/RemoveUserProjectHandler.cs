namespace GrpcServer.Handlers.UserProject;

// Dapper Repository Advanced
public sealed class RemoveUserProjectHandler(IUnitOfWork unitOfWork, ILogger<RemoveUserProjectHandler> logger) : ICommandHandler<RemoveUserProjectCommand, RemoveUserProjectResult>
{
    public async Task<RemoveUserProjectResult> ExecuteAsync(RemoveUserProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");
        var ids = BatchIds.Normalize(command.Ids);

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Removing {Count} user project entities atomically.", ids.Length);
            var repository = unitOfWork.GetRepository<Domain.Entities.UserProject>();
            var success = await repository.RemoveManyAsync(ids, cancellationToken);

            if (!success)
            {
                logger.LogWarning("At least one user project to remove was not found; rolling back.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveUserProjectResult
                {
                    Success = false,
                    Message = "UserProject not found."
                };
            }

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");
            return new RemoveUserProjectResult
            {
                Success = true,
                Message = "UserProject removed successfully."
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
