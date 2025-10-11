namespace GrpcServer.Handlers.Impediment;

// EF Core
public sealed class CreateImpedimentHandler(IApplicationDbContext dbContext, ILogger<CreateImpedimentHandler> logger) : ICommandHandler<CreateImpedimentCommand, CreateImpedimentResult>
{
    public async Task<CreateImpedimentResult> ExecuteAsync(CreateImpedimentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {ImpedimentId}", command.Name, command.Id);

            logger.LogInformation("Checking if an impediment entity exists with Id: {ImpedimentId}", command.Id);
            var itemExists = dbContext.Impediments!.Any(x => x.Id == command.Id);

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

            logger.LogInformation("Adding impediment to repository.");
            await dbContext.Impediments!.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Fetching impediment by Id: {ImpedimentId}", newItem.Id);
            var createdItem = await dbContext.Impediments!.FindAsync([newItem.Id, cancellationToken], cancellationToken: cancellationToken);

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
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}