namespace GrpcServer.Handlers.User;

// Dapper Repository Advanced
public sealed class RemoveUserHandler(IUnitOfWork unitOfWork, ILogger<RemoveUserHandler> logger) : ICommandHandler<RemoveUserCommand, RemoveUserResult>
{
    [Microsoft.AspNetCore.Authorization.Authorize(Policy = Common.Security.UserAdministration.Policy)]
    public async Task<RemoveUserResult> ExecuteAsync(RemoveUserCommand command, CancellationToken cancellationToken)
    {
        var ids = BatchIds.Normalize(command.Ids);

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Removing {Count} user entities atomically.", ids.Length);
            var repository = unitOfWork.GetRepository<Domain.Entities.User>();
            var success = await repository.RemoveManyAsync(ids, cancellationToken);

            if (!success)
            {
                logger.LogWarning("At least one user to remove was not found; rolling back.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveUserResult
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            return new RemoveUserResult
            {
                Success = true,
                Message = "User removed successfully."
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
