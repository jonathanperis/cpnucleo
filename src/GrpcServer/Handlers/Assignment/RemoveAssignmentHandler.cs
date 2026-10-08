namespace GrpcServer.Handlers.Assignment;

// Dapper Repository Advanced
public sealed class RemoveAssignmentHandler(IUnitOfWork unitOfWork, ILogger<RemoveAssignmentHandler> logger) : ICommandHandler<RemoveAssignmentCommand, RemoveAssignmentResult>
{
    public async Task<RemoveAssignmentResult> ExecuteAsync(RemoveAssignmentCommand command, CancellationToken cancellationToken)
    {
        var ids = BatchIds.Normalize(command.Ids);

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Removing {Count} assignment entities atomically.", ids.Length);
            var repository = unitOfWork.GetRepository<Domain.Entities.Assignment>();
            var success = await repository.RemoveManyAsync(ids, cancellationToken);

            if (!success)
            {
                logger.LogWarning("At least one assignment to remove was not found; rolling back.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveAssignmentResult
                {
                    Success = false,
                    Message = "Assignment not found."
                };
            }

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            return new RemoveAssignmentResult
            {
                Success = true,
                Message = "Assignment removed successfully."
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
