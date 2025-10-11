namespace GrpcServer.Handlers.UserProject;

// EF Core
public sealed class UpdateUserProjectHandler(IApplicationDbContext dbContext, ILogger<UpdateUserProjectHandler> logger) : ICommandHandler<UpdateUserProjectCommand, UpdateUserProjectResult>
{
    public async Task<UpdateUserProjectResult> ExecuteAsync(UpdateUserProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an userProject entity exists with Id: {UserProjectId}", command.Id);
            var item = await dbContext.UserProjects!.FindAsync([command.Id, cancellationToken], cancellationToken: cancellationToken);

            if (item is null)
            {
                logger.LogWarning("UserProject not found with Id: {UserProjectId}", command.Id);
                return new UpdateUserProjectResult 
            { 
                Success = false,
                Message = "UserProject not found."
            };
            }

            logger.LogInformation("Updating userProject entity with Id: {UserProjectId}", command.Id);
            Domain.Entities.UserProject.Update(item, command.UserId, command.ProjectId);

            logger.LogInformation("Updating entity in repository.");
            var success = await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Service completed successfully.");

            return new UpdateUserProjectResult 
            { 
                Success = success,
                Message = success ? "UserProject updated successfully." : "Failed to update UserProject."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}