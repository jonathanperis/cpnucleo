namespace GrpcServer.Handlers.Impediment;

// Dapper Repository Advanced
public sealed class CreateImpedimentHandler(IUnitOfWork unitOfWork, ILogger<CreateImpedimentHandler> logger) : ICommandHandler<CreateImpedimentCommand, CreateImpedimentResult>
{
    public async Task<CreateImpedimentResult> ExecuteAsync(CreateImpedimentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {ImpedimentId}", command.Name, command.Id);

        try
        {
            logger.LogInformation("Checking if an impediment entity exists with Id: {ImpedimentId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.Impediment>();
            var itemExists = await repository.ExistsAsync(command.Id);

            if (itemExists)
            {
                logger.LogWarning("Impediment Id conflict for Id: {ImpedimentId}", command.Id);
                return new CreateImpedimentResult
                {
                    Success = false,
                    Message = "this Id is already in use!"
                };
            }

            logger.LogInformation("Validation passed, proceeding to create new impediment entity.");
            var newItem = Domain.Entities.Impediment.Create(command.Name, command.Id);
            logger.LogInformation("Created new impediment entity with Id: {ImpedimentId}", newItem.Id);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Adding impediment to repository.");
            var createdId = await repository.AddAsync(newItem);

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Fetching impediment by Id: {ImpedimentId}", createdId);
            var createdItem = await repository.GetByIdAsync(createdId);

            var result = new CreateImpedimentResult
            {
                Success = true,
                Message = "Impediment created successfully.",
                Impediment = createdItem!.MapToDto()
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