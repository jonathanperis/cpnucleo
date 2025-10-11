namespace GrpcServer.Handlers.Organization;

// Dapper Repository Advanced
public sealed class UpdateOrganizationHandler(IUnitOfWork unitOfWork, ILogger<UpdateOrganizationHandler> logger) : ICommandHandler<UpdateOrganizationCommand, UpdateOrganizationResult>
{
    public async Task<UpdateOrganizationResult> ExecuteAsync(UpdateOrganizationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an organization entity exists with Id: {OrganizationId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
            var item = await repository.GetByIdAsync(command.Id);

            if (item is null)
            {
                logger.LogWarning("Organization not found with Id: {OrganizationId}", command.Id);
                return new UpdateOrganizationResult 
                { 
                    Success = false,
                    Message = "Organization not found."
                };
            }

            logger.LogInformation("Updating organization entity with Id: {OrganizationId}", command.Id);
            Domain.Entities.Organization.Update(item, command.Name, command.Description);

            logger.LogInformation("Updating entity in repository.");
            var success = await repository.UpdateAsync(item);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new UpdateOrganizationResult 
            { 
                Success = success,
                Message = success ? "Organization updated successfully." : "Failed to update Organization."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command. Rolling back transaction.");
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
