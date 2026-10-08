namespace GrpcServer.Handlers.AssignmentImpediment;

// Dapper Repository Advanced
public sealed class CreateAssignmentImpedimentHandler(IUnitOfWork unitOfWork, ILogger<CreateAssignmentImpedimentHandler> logger) : ICommandHandler<CreateAssignmentImpedimentCommand, CreateAssignmentImpedimentResult>
{
    public async Task<CreateAssignmentImpedimentResult> ExecuteAsync(CreateAssignmentImpedimentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking if an assignmentImpediment entity exists with Id: {AssignmentImpedimentId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentImpediment>();
            var itemExists = await repository.ExistsAsync(command.Id, cancellationToken);

            if (itemExists)
            {
                logger.LogWarning("AssignmentImpediment Id conflict for Id: {AssignmentImpedimentId}", command.Id);
                return new CreateAssignmentImpedimentResult
                {
                    Success = false,
                    Message = "this Id is already in use!"
                };
            }

            logger.LogInformation("Validation passed, proceeding to create new assignmentImpediment entity.");
            var newItem = Domain.Entities.AssignmentImpediment.Create(command.Description, command.AssignmentId, command.ImpedimentId, command.Id);
            logger.LogInformation("Created new assignmentImpediment entity with Id: {AssignmentImpedimentId}", newItem.Id);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Adding assignmentImpediment to repository.");
            var createdId = await repository.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Fetching assignmentImpediment by Id: {AssignmentImpedimentId}", createdId);
            var createdItem = await repository.GetByIdAsync(createdId, cancellationToken);

            var result = new CreateAssignmentImpedimentResult
            {
                Success = true,
                Message = "AssignmentImpediment created successfully.",
                AssignmentImpediment = createdItem!.MapToDto()
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