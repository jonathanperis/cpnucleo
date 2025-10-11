namespace GrpcServer.Handlers.AssignmentType;

// Dapper Repository Advanced
public sealed class GetAssignmentTypeByIdHandler(IUnitOfWork unitOfWork, ILogger<GetAssignmentTypeByIdHandler> logger) : ICommandHandler<GetAssignmentTypeByIdCommand, GetAssignmentTypeByIdResult>
{
    public async Task<GetAssignmentTypeByIdResult> ExecuteAsync(GetAssignmentTypeByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        logger.LogInformation("Fetching assignmentType entity with Id: {AssignmentTypeId}", command.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentType>();
        var item = await repository.GetByIdAsync(command.Id);

        if (item is null)
        {
            logger.LogWarning("AssignmentType not found with Id: {AssignmentTypeId}", command.Id);
            return new GetAssignmentTypeByIdResult
            {
                Success = false,
                Message = "AssignmentType not found."
            };
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {AssignmentTypeId}", command.Id);
        var result = new GetAssignmentTypeByIdResult
        {
            Success = true,
            Message = "AssignmentType fetched successfully.",
            AssignmentType = item.MapToDto()
        };

        logger.LogInformation("Service completed successfully.");

        return result;
    }
}