namespace GrpcServer.Handlers.AssignmentType;

// Dapper Repository Advanced
public sealed class CreateAssignmentTypeHandler(IUnitOfWork unitOfWork, ILogger<CreateAssignmentTypeHandler> logger) : ICommandHandler<CreateAssignmentTypeCommand, CreateAssignmentTypeResult>
{
    public async Task<CreateAssignmentTypeResult> ExecuteAsync(CreateAssignmentTypeCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {AssignmentTypeId}", command.Name, command.Id);

        try
        {
            logger.LogInformation("Checking if an assignmentType entity exists with Id: {AssignmentTypeId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentType>();
            var itemExists = await repository.ExistsAsync(command.Id);

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

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Adding assignmentType to repository.");
            var createdId = await repository.AddAsync(newItem);

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Fetching assignmentType by Id: {AssignmentTypeId}", createdId);
            var createdItem = await repository.GetByIdAsync(createdId);

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
            logger.LogError(ex, "An error occurred while processing the command. Rolling back transaction.");
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}