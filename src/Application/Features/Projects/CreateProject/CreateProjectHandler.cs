namespace Application.Features.Projects.CreateProject;

public sealed record CreateProjectRequest(Guid Id, string? Name, Guid OrganizationId);

public sealed record CreateProjectResult(bool Success, string Message, ProjectDetails? Project = null);

public interface IProjectCreateStore
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists the project and, when <paramref name="creatorUserId"/> refers to an active user,
    /// makes that user a member in the same transaction.
    /// </summary>
    Task<Project> AddAsync(Project project, Guid? creatorUserId, CancellationToken cancellationToken = default);
}

public sealed class CreateProjectHandler(IProjectCreateStore store, ICurrentUser currentUser, ILogger<CreateProjectHandler> logger)
{
    public async Task<CreateProjectResult> ExecuteAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Service started processing request for project Id: {ProjectId}", request.Id);

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            logger.LogWarning("Project validation failed: Name is required.");
            return new CreateProjectResult(false, "Name is required.");
        }

        if (request.OrganizationId == Guid.Empty)
        {
            logger.LogWarning("Project validation failed: OrganizationId is required.");
            return new CreateProjectResult(false, "OrganizationId is required.");
        }

        logger.LogInformation("Checking if a project entity exists with Id: {ProjectId}", request.Id);
        var itemExists = await store.ExistsAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (itemExists)
        {
            logger.LogWarning("Project Id conflict for Id: {ProjectId}", request.Id);
            return new CreateProjectResult(false, "this Id is already in use!");
        }

        logger.LogInformation("Validation passed, proceeding to create new project entity.");
        var newItem = Project.Create(request.Name, request.OrganizationId, request.Id);
        logger.LogInformation("Created new project entity with Id: {ProjectId}", newItem.Id);

        logger.LogInformation("Adding project to store.");
        var creator = currentUser.IsSystem ? null : currentUser.UserId;
        var createdItem = await store.AddAsync(newItem, creator, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Service completed successfully.");
        return new CreateProjectResult(true, "Project created successfully.", ProjectDetails.FromEntity(createdItem));
    }
}
