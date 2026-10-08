namespace GrpcServer.Handlers.Organization;

// Dapper Repository Advanced
public sealed class RemoveOrganizationHandler(IUnitOfWork unitOfWork, ILogger<RemoveOrganizationHandler> logger) : ICommandHandler<RemoveOrganizationCommand, RemoveOrganizationResult>
{
    public async Task<RemoveOrganizationResult> ExecuteAsync(RemoveOrganizationCommand command, CancellationToken cancellationToken)
    {
        var ids = BatchIds.Normalize(command.Ids);

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Removing {Count} organization entities atomically.", ids.Length);
            var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
            var success = await repository.RemoveManyAsync(ids, cancellationToken);

            if (!success)
            {
                logger.LogWarning("At least one organization to remove was not found; rolling back.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveOrganizationResult
                {
                    Success = false,
                    Message = "Organization not found."
                };
            }

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            return new RemoveOrganizationResult
            {
                Success = true,
                Message = "Organization removed successfully."
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
