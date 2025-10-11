namespace GrpcServer.Handlers.Project;

// Dapper Repository Basic
public sealed class CreateProjectHandler(IProjectRepository repository, ILogger<CreateProjectHandler> logger) : ICommandHandler<CreateProjectCommand, CreateProjectResult>
{
    public async Task<CreateProjectResult> ExecuteAsync(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {ProjectId}", command.Name, command.Id);

        try
        {
            logger.LogInformation("Checking if an project entity exists with Id: {ProjectId}", command.Id);
            var itemExists = await repository.ExistsAsync(command.Id);

            if (itemExists)
            {
                logger.LogWarning("Project Id conflict for Id: {ProjectId}", command.Id);
                return new CreateProjectResult
                {
                    Success = false,
                    Message = "this Id is already in use!"
                };
            }

            logger.LogInformation("Validation passed, proceeding to create new project entity.");
            var newItem = Domain.Entities.Project.Create(command.Name, command.OrganizationId, command.Id);
            logger.LogInformation("Created new project entity with Id: {ProjectId}", newItem.Id);

            logger.LogInformation("Adding project to repository.");
            var newItemId = await repository.AddAsync(newItem);

            logger.LogInformation("Fetching project by Id: {ProjectId}", newItemId);
            var createdItem = await repository.GetByIdAsync(newItemId);

            var result = new CreateProjectResult
            {
                Success = true,
                Message = "Project created successfully.",
                Project = createdItem!.MapToDto()
            };

            logger.LogInformation("Service completed successfully.");

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}