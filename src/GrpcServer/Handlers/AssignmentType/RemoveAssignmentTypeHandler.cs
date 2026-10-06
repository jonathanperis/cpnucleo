namespace GrpcServer.Handlers.AssignmentType;

// Dapper Repository Advanced
public sealed class RemoveAssignmentTypeHandler(IUnitOfWork unitOfWork, ILogger<RemoveAssignmentTypeHandler> logger) : ICommandHandler<RemoveAssignmentTypeCommand, RemoveAssignmentTypeResult>
{
    public async Task<RemoveAssignmentTypeResult> ExecuteAsync(RemoveAssignmentTypeCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");
        var ids = BatchIds.Normalize(command.Ids);

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Removing {Count} assignment type entities atomically.", ids.Length);
            var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentType>();
            var success = await repository.RemoveManyAsync(ids, cancellationToken);

            if (!success)
            {
                logger.LogWarning("At least one assignment type to remove was not found; rolling back.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveAssignmentTypeResult
                {
                    Success = false,
                    Message = "AssignmentType not found."
                };
            }

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");
            return new RemoveAssignmentTypeResult
            {
                Success = true,
                Message = "AssignmentType removed successfully."
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
