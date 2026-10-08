namespace GrpcServer.Handlers.UserAssignment;

// Dapper Repository Advanced
public sealed class RemoveUserAssignmentHandler(IUnitOfWork unitOfWork, ILogger<RemoveUserAssignmentHandler> logger) : ICommandHandler<RemoveUserAssignmentCommand, RemoveUserAssignmentResult>
{
    public async Task<RemoveUserAssignmentResult> ExecuteAsync(RemoveUserAssignmentCommand command, CancellationToken cancellationToken)
    {
        var ids = BatchIds.Normalize(command.Ids);

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Removing {Count} user assignment entities atomically.", ids.Length);
            var repository = unitOfWork.GetRepository<Domain.Entities.UserAssignment>();
            var success = await repository.RemoveManyAsync(ids, cancellationToken);

            if (!success)
            {
                logger.LogWarning("At least one user assignment to remove was not found; rolling back.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveUserAssignmentResult
                {
                    Success = false,
                    Message = "UserAssignment not found."
                };
            }

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            return new RemoveUserAssignmentResult
            {
                Success = true,
                Message = "UserAssignment removed successfully."
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
