namespace GrpcServer.Handlers.AssignmentType;

// Dapper Repository Advanced
public sealed class CreateAssignmentTypeHandler(IUnitOfWork unitOfWork, ILogger<CreateAssignmentTypeHandler> logger) : ICommandHandler<CreateAssignmentTypeCommand, CreateAssignmentTypeResult>
{
    public async Task<CreateAssignmentTypeResult> ExecuteAsync(CreateAssignmentTypeCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking if an assignmentType entity exists with Id: {AssignmentTypeId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentType>();
            var itemExists = await repository.ExistsAsync(command.Id, cancellationToken);

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
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Adding assignmentType to repository.");
            var createdId = await repository.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Fetching assignmentType by Id: {AssignmentTypeId}", createdId);
            var createdItem = await repository.GetByIdAsync(createdId, cancellationToken);

            var result = new CreateAssignmentTypeResult
            {
                Success = true,
                Message = "AssignmentType created successfully.",
                AssignmentType = createdItem!.MapToDto()
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