namespace GrpcServer.Handlers.UserProject;

// Dapper Repository Advanced
public sealed class GetUserProjectByIdHandler(IUnitOfWork unitOfWork, ILogger<GetUserProjectByIdHandler> logger) : ICommandHandler<GetUserProjectByIdCommand, GetUserProjectByIdResult>
{
    public async Task<GetUserProjectByIdResult> ExecuteAsync(GetUserProjectByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        logger.LogInformation("Fetching userProject entity with Id: {UserProjectId}", command.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.UserProject>();
        var item = await repository.GetByIdAsync(command.Id);

        if (item is null)
        {
            logger.LogWarning("UserProject not found with Id: {UserProjectId}", command.Id);
            return new GetUserProjectByIdResult
            {
                Success = false,
                Message = "UserProject not found."
            };
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {UserProjectId}", command.Id);
        var result = new GetUserProjectByIdResult
        {
            Success = true,
            Message = "UserProject fetched successfully.",
            UserProject = item.MapToDto()
        };

        logger.LogInformation("Service completed successfully.");

        return result;
    }
}