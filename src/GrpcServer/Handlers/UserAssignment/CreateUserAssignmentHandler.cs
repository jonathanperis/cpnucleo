namespace GrpcServer.Handlers.UserAssignment;

// Dapper Repository Advanced
public sealed class CreateUserAssignmentHandler(IUnitOfWork unitOfWork, ILogger<CreateUserAssignmentHandler> logger) : ICommandHandler<CreateUserAssignmentCommand, CreateUserAssignmentResult>
{
    public async Task<CreateUserAssignmentResult> ExecuteAsync(CreateUserAssignmentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking if an userAssignment entity exists with Id: {UserAssignmentId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.UserAssignment>();
            var itemExists = await repository.ExistsAsync(command.Id, cancellationToken);

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
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Adding userAssignment to repository.");
            var createdId = await repository.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Fetching userAssignment by Id: {UserAssignmentId}", createdId);
            var createdItem = await repository.GetByIdAsync(createdId, cancellationToken);

            var result = new CreateUserAssignmentResult
            {
                Success = true,
                Message = "UserAssignment created successfully.",
                UserAssignment = createdItem!.MapToDto()
            };

            return result;
        }
        catch
        {
            // Rejections and failures are translated (and unexpected ones logged) by the gRPC pipeline.
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}