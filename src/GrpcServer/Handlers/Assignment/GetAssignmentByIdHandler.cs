namespace GrpcServer.Handlers.Assignment;

// Dapper Repository Advanced
public sealed class GetAssignmentByIdHandler(IUnitOfWork unitOfWork, ILogger<GetAssignmentByIdHandler> logger) : ICommandHandler<GetAssignmentByIdCommand, GetAssignmentByIdResult>
{
    public async Task<GetAssignmentByIdResult> ExecuteAsync(GetAssignmentByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        logger.LogInformation("Fetching assignment entity with Id: {AssignmentId}", command.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.Assignment>();
        var item = await repository.GetByIdAsync(command.Id);

        if (item is null)
        {
            logger.LogWarning("Assignment not found with Id: {AssignmentId}", command.Id);
            return new GetAssignmentByIdResult();
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {AssignmentId}", command.Id);
        var result = new GetAssignmentByIdResult
        {
            Assignment = item.MapToDto()
        };

        logger.LogInformation("Service completed successfully.");

        return result;
    }
}