namespace GrpcServer.Handlers.Project;

// Dapper Repository Basic
public sealed class GetProjectByIdHandler(IProjectRepository repository, ILogger<GetProjectByIdHandler> logger) : ICommandHandler<GetProjectByIdCommand, GetProjectByIdResult>
{
    public async Task<GetProjectByIdResult> ExecuteAsync(GetProjectByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        logger.LogInformation("Fetching project entity with Id: {ProjectId}", command.Id);
        var item = await repository.GetByIdAsync(command.Id);

        if (item is null)
        {
            logger.LogWarning("Project not found with Id: {ProjectId}", command.Id);
            return new GetProjectByIdResult();
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {ProjectId}", command.Id);
        var result = new GetProjectByIdResult
        {
            Project = item.MapToDto()
        };

        logger.LogInformation("Service completed successfully.");

        return result;
    }
}