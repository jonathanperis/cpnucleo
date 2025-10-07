namespace WebApi.Endpoints.UserProject.CreateUserProject;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/userProject");
        Description(x => x.WithTags("UserProjects"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Create a new userProject";
            s.Description = "Creates a new userProject record with the given data and custom Id. Validates uniqueness and returns the created userProject's data.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Service started processing request with payload UserId: {UserId}, UserProjectId: {UserProjectId}, Id: {Id}", request.UserId, request.ProjectId, request.Id);

            Logger.LogInformation("Checking if an userProject entity exists with Id: {UserProjectId}", request.Id);
            var itemExists = dbContext.UserProjects!.Any(x => x.Id == request.Id);

            if (itemExists)
            {
                Logger.LogWarning("UserProject Id conflict for Id: {UserProjectId}", request.Id);
                AddError(r => r.Id, "this Id is already in use!");
            }

            ThrowIfAnyErrors();

            Logger.LogInformation("Validation passed, proceeding to create new userProject entity.");
            var newItem = Domain.Entities.UserProject.Create(request.UserId, request.ProjectId, request.Id);
            Logger.LogInformation("Created new userProject entity with Id: {UserProjectId}", newItem.Id);

            Logger.LogInformation("Adding userProject to repository.");
            await dbContext.UserProjects!.AddAsync(newItem, cancellationToken);

            Logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Fetching userProject by Id: {UserProjectId}", newItem.Id);
            var createdItem = await dbContext.UserProjects!.FindAsync([newItem.Id, cancellationToken], cancellationToken: cancellationToken);

            Response.UserProject = createdItem!.MapToDto();

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
