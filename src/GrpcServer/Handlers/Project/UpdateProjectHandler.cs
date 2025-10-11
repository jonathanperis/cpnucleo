namespace GrpcServer.Handlers.Project;

// Dapper Repository Basic
public sealed class UpdateProjectHandler(IProjectRepository repository, ILogger<UpdateProjectHandler> logger) : ICommandHandler<UpdateProjectCommand, UpdateProjectResult>
{
    public async Task<UpdateProjectResult> ExecuteAsync(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an project entity exists with Id: {ProjectId}", command.Id);
            var item = await repository.GetByIdAsync(command.Id);

            if (item is null)
            {
                logger.LogWarning("Project not found with Id: {ProjectId}", command.Id);
                return new UpdateProjectResult 
            { 
                Success = false,
                Message = "Project not found."
            };
            }

            logger.LogInformation("Updating project entity with Id: {ProjectId}", command.Id);
            Domain.Entities.Project.Update(item, command.Name, command.OrganizationId);

            logger.LogInformation("Updating entity in repository.");
            var success = await repository.UpdateAsync(item);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Service completed successfully.");

            return new UpdateProjectResult 
            { 
                Success = success,
                Message = success ? "Project updated successfully." : "Failed to update Project."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}