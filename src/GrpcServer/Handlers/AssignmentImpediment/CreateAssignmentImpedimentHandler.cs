namespace GrpcServer.Handlers.AssignmentImpediment;

// EF Core
public sealed class CreateAssignmentImpedimentHandler(IApplicationDbContext dbContext, ILogger<CreateAssignmentImpedimentHandler> logger) : ICommandHandler<CreateAssignmentImpedimentCommand, CreateAssignmentImpedimentResult>
{
    public async Task<CreateAssignmentImpedimentResult> ExecuteAsync(CreateAssignmentImpedimentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Service started processing request with payload Description: {Description}, Id: {AssignmentImpedimentId}", command.Description, command.Id);

            logger.LogInformation("Checking if an assignmentImpediment entity exists with Id: {AssignmentImpedimentId}", command.Id);
            var itemExists = dbContext.AssignmentImpediments!.Any(x => x.Id == command.Id);

            if (itemExists)
            {
                logger.LogWarning("AssignmentImpediment Id conflict for Id: {AssignmentImpedimentId}", command.Id);
                return new CreateAssignmentImpedimentResult();
            }

            logger.LogInformation("Validation passed, proceeding to create new assignmentImpediment entity.");
            var newItem = Domain.Entities.AssignmentImpediment.Create(command.Description, command.AssignmentId, command.ImpedimentId, command.Id);
            logger.LogInformation("Created new assignmentImpediment entity with Id: {AssignmentImpedimentId}", newItem.Id);

            logger.LogInformation("Adding assignmentImpediment to repository.");
            await dbContext.AssignmentImpediments!.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Fetching assignmentImpediment by Id: {AssignmentImpedimentId}", newItem.Id);
            var createdItem = await dbContext.AssignmentImpediments!.FindAsync([newItem.Id, cancellationToken], cancellationToken: cancellationToken);

            var result = new CreateAssignmentImpedimentResult
            {
                AssignmentImpediment = createdItem!.MapToDto()
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