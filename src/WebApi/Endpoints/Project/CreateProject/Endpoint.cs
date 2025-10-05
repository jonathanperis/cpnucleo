namespace WebApi.Endpoints.Project.CreateProject;

// Dapper Repository Basic
public class Endpoint(IProjectRepository repository) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/project");
        Description(x => x.WithTags("Projects"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Create a new project";
            s.Description = "Creates a new project record with the given data and custom Id. Validates uniqueness and returns the created project's data.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {ProjectId}", request.Name, request.Id);

        try
        {
            Logger.LogInformation("Checking if an project entity exists with Id: {ProjectId}", request.Id);
            var itemExists = await repository.ExistsAsync(request.Id);

            if (itemExists)
            {
                Logger.LogWarning("Project Id conflict for Id: {ProjectId}", request.Id);
                AddError(r => r.Id, "this Id is already in use!");
            }

            ThrowIfAnyErrors();

            Logger.LogInformation("Validation passed, proceeding to create new project entity.");
            var newItem = Domain.Entities.Project.Create(request.Name, request.OrganizationId, request.Id);
            Logger.LogInformation("Created new project entity with Id: {ProjectId}", newItem.Id);

            Logger.LogInformation("Adding project to repository.");
            var newItemId = await repository.AddAsync(newItem);

            Logger.LogInformation("Fetching project by Id: {ProjectId}", newItemId);
            var createdItem = await repository.GetByIdAsync(newItemId);

            Response.Project = createdItem!.MapToDto();

            Logger.LogInformation("Service completed successfully.");

            await Send.OkAsync(Response, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while processing the request.");
            ThrowError("An error has occurred.");
        }
    }
}
