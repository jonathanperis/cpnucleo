namespace GrpcServer.Handlers.AssignmentType;

// EF Core
public sealed class CreateAssignmentTypeHandler(IApplicationDbContext dbContext, ILogger<CreateAssignmentTypeHandler> logger) : ICommandHandler<CreateAssignmentTypeCommand, CreateAssignmentTypeResult>
{
    public async Task<CreateAssignmentTypeResult> ExecuteAsync(CreateAssignmentTypeCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {AssignmentTypeId}", command.Name, command.Id);

            logger.LogInformation("Checking if an assignmentType entity exists with Id: {AssignmentTypeId}", command.Id);
            var itemExists = dbContext.AssignmentTypes!.Any(x => x.Id == command.Id);

            if (itemExists)
            {
                logger.LogWarning("AssignmentType Id conflict for Id: {AssignmentTypeId}", command.Id);
                return new CreateAssignmentTypeResult
            {
                Success = false,
                Message = "this Id is already in use!"
            };
            }

            logger.LogInformation("Validation passed, proceeding to create new assignmentType entity.");
            var newItem = Domain.Entities.AssignmentType.Create(command.Name, command.Id);
            logger.LogInformation("Created new assignmentType entity with Id: {AssignmentTypeId}", newItem.Id);

            logger.LogInformation("Adding assignmentType to repository.");
            await dbContext.AssignmentTypes!.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Fetching assignmentType by Id: {AssignmentTypeId}", newItem.Id);
            var createdItem = await dbContext.AssignmentTypes!.FindAsync([newItem.Id, cancellationToken], cancellationToken: cancellationToken);

            var result = new CreateAssignmentTypeResult
            {
                Success = true,
                Message = "AssignmentType created successfully.",
                AssignmentType = createdItem!.MapToDto()
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