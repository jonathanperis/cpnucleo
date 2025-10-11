namespace GrpcServer.Handlers.Organization;

// Dapper Repository Advanced
public sealed class RemoveOrganizationHandler(IUnitOfWork unitOfWork, ILogger<RemoveOrganizationHandler> logger) : ICommandHandler<RemoveOrganizationCommand, RemoveOrganizationResult>
{
    public async Task<RemoveOrganizationResult> ExecuteAsync(RemoveOrganizationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Checking if organization entities exist for Ids: {OrganizationIds}", string.Join(",", command.Ids));
            var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    logger.LogWarning("Organization not found with Id: {OrganizationId}", id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return new RemoveOrganizationResult { Success = false };
                }

                logger.LogInformation("Removing organization entity with Id: {OrganizationId}", id);
                Domain.Entities.Organization.Remove(item);

                logger.LogInformation("Deleting organization entity from repository with Id: {OrganizationId}.", id);
                var result = await repository.DeleteAsync(item);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed, rolling back transaction.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveOrganizationResult { Success = false };
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new RemoveOrganizationResult { Success = allSuccess };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command. Rolling back transaction.");
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}