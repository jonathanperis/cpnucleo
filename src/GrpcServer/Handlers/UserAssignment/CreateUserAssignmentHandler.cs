namespace GrpcServer.Handlers.UserAssignment;

// Dapper Repository Advanced
public sealed class CreateUserAssignmentHandler(IUnitOfWork unitOfWork, ILogger<CreateUserAssignmentHandler> logger) : ICommandHandler<CreateUserAssignmentCommand, CreateUserAssignmentResult>
{
    public async Task<CreateUserAssignmentResult> ExecuteAsync(CreateUserAssignmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request with payload UserId: {UserId}, UserAssignmentId: {UserAssignmentId}, Id: {Id}", command.UserId, command.AssignmentId, command.Id);

        try
        {
            logger.LogInformation("Checking if an userAssignment entity exists with Id: {UserAssignmentId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.UserAssignment>();
            var itemExists = await repository.ExistsAsync(command.Id);

            if (itemExists)
            {
                logger.LogWarning("UserAssignment Id conflict for Id: {UserAssignmentId}", command.Id);
                return new CreateUserAssignmentResult
                {
                    Success = false,
                    Message = "this Id is already in use!"
                };
            }

            logger.LogInformation("Validation passed, proceeding to create new userAssignment entity.");
            var newItem = Domain.Entities.UserAssignment.Create(command.UserId, command.AssignmentId, command.Id);
            logger.LogInformation("Created new userAssignment entity with Id: {UserAssignmentId}", newItem.Id);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Adding userAssignment to repository.");
            var createdId = await repository.AddAsync(newItem);

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Fetching userAssignment by Id: {UserAssignmentId}", createdId);
            var createdItem = await repository.GetByIdAsync(createdId);

            var result = new CreateUserAssignmentResult
            {
                Success = true,
                Message = "UserAssignment created successfully.",
                UserAssignment = createdItem!.MapToDto()
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