namespace GrpcServer.Handlers.Organization;

// Dapper Repository Advanced
public sealed class CreateOrganizationHandler(IUnitOfWork unitOfWork, ILogger<CreateOrganizationHandler> logger) : ICommandHandler<CreateOrganizationCommand, CreateOrganizationResult>
{
    public async Task<CreateOrganizationResult> ExecuteAsync(CreateOrganizationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request with payload Name: {Name}, Description: {Description}, Id: {OrganizationId}", command.Name, command.Description, command.Id);

        try
        {
            logger.LogInformation("Checking if an organization entity exists with Id: {OrganizationId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
            var itemExists = await repository.ExistsAsync(command.Id);

            if (itemExists)
            {
                logger.LogWarning("Organization Id conflict for Id: {OrganizationId}", command.Id);
                return new CreateOrganizationResult
                {
                    Success = false,
                    Message = "this Id is already in use!"
                };
            }

            logger.LogInformation("Validation passed, proceeding to create new organization entity.");
            var newItem = Domain.Entities.Organization.Create(command.Name, command.Description, command.Id);
            logger.LogInformation("Created new organization entity with Id: {OrganizationId}", newItem.Id);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Adding organization to repository.");
            var createdId = await repository.AddAsync(newItem);

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Fetching organization by Id: {OrganizationId}", createdId);
            var createdItem = await repository.GetByIdAsync(createdId);

            var result = new CreateOrganizationResult
            {
                Success = true,
                Message = "Organization created successfully.",
                Organization = createdItem!.MapToDto()
            };

            logger.LogInformation("Service completed successfully.");

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command. Rolling back transaction.");
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}