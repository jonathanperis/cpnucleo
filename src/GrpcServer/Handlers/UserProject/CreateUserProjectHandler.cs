namespace GrpcServer.Handlers.UserProject;

// EF Core
public sealed class CreateUserProjectHandler(IApplicationDbContext dbContext, ILogger<CreateUserProjectHandler> logger) : ICommandHandler<CreateUserProjectCommand, CreateUserProjectResult>
{
    public async Task<CreateUserProjectResult> ExecuteAsync(CreateUserProjectCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Service started processing request with payload UserId: {UserId}, UserProjectId: {UserProjectId}, Id: {Id}", command.UserId, command.ProjectId, command.Id);

            logger.LogInformation("Checking if an userProject entity exists with Id: {UserProjectId}", command.Id);
            var itemExists = dbContext.UserProjects!.Any(x => x.Id == command.Id);

            if (itemExists)
            {
                logger.LogWarning("UserProject Id conflict for Id: {UserProjectId}", command.Id);
                return new CreateUserProjectResult();
            }

            logger.LogInformation("Validation passed, proceeding to create new userProject entity.");
            var newItem = Domain.Entities.UserProject.Create(command.UserId, command.ProjectId, command.Id);
            logger.LogInformation("Created new userProject entity with Id: {UserProjectId}", newItem.Id);

            logger.LogInformation("Adding userProject to repository.");
            await dbContext.UserProjects!.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Fetching userProject by Id: {UserProjectId}", newItem.Id);
            var createdItem = await dbContext.UserProjects!.FindAsync([newItem.Id, cancellationToken], cancellationToken: cancellationToken);

            var result = new CreateUserProjectResult
            {
                UserProject = createdItem!.MapToDto()
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