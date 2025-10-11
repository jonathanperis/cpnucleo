namespace GrpcServer.Handlers.Project;

// Dapper Repository Advanced
public sealed class GetProjectByIdHandler(IUnitOfWork unitOfWork, ILogger<GetProjectByIdHandler> logger) : ICommandHandler<GetProjectByIdCommand, GetProjectByIdResult>
{
    public async Task<GetProjectByIdResult> ExecuteAsync(GetProjectByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        logger.LogInformation("Fetching project entity with Id: {ProjectId}", command.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.Project>();
        var item = await repository.GetByIdAsync(command.Id);

        if (item is null)
        {
            logger.LogWarning("Project not found with Id: {ProjectId}", command.Id);
            return new GetProjectByIdResult
            {
                Success = false,
                Message = "Project not found."
            };
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {ProjectId}", command.Id);
        var result = new GetProjectByIdResult
        {
            Success = true,
            Message = "Project fetched successfully.",
            Project = item.MapToDto()
        };

        logger.LogInformation("Service completed successfully.");

        return result;
    }
}