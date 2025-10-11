namespace GrpcServer.Handlers.UserAssignment;

// Dapper Repository Advanced
public sealed class GetUserAssignmentByIdHandler(IUnitOfWork unitOfWork, ILogger<GetUserAssignmentByIdHandler> logger) : ICommandHandler<GetUserAssignmentByIdCommand, GetUserAssignmentByIdResult>
{
    public async Task<GetUserAssignmentByIdResult> ExecuteAsync(GetUserAssignmentByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        logger.LogInformation("Fetching userAssignment entity with Id: {UserAssignmentId}", command.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.UserAssignment>();
        var item = await repository.GetByIdAsync(command.Id);

        if (item is null)
        {
            logger.LogWarning("UserAssignment not found with Id: {UserAssignmentId}", command.Id);
            return new GetUserAssignmentByIdResult
            {
                Success = false,
                Message = "UserAssignment not found."
            };
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {UserAssignmentId}", command.Id);
        var result = new GetUserAssignmentByIdResult
        {
            Success = true,
            Message = "UserAssignment fetched successfully.",
            UserAssignment = item.MapToDto()
        };

        logger.LogInformation("Service completed successfully.");

        return result;
    }
}