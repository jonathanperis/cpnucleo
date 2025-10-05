namespace WebApi.Endpoints.UserAssignment.CreateUserAssignment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/userAssignment");
        Description(x => x.WithTags("UserAssignments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Create a new userAssignment";
            s.Description = "Creates a new userAssignment record with the given data and custom Id. Validates uniqueness and returns the created userAssignment's data.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Service started processing request with payload UserId: {UserId}, UserAssignmentId: {UserAssignmentId}, Id: {Id}", request.UserId, request.AssignmentId, request.Id);

            Logger.LogInformation("Checking if an userAssignment entity exists with Id: {UserAssignmentId}", request.Id);
            var itemExists = dbContext.UserAssignments!.Any(x => x.Id == request.Id);

            if (itemExists)
            {
                Logger.LogWarning("UserAssignment Id conflict for Id: {UserAssignmentId}", request.Id);
                AddError(r => r.Id, "this Id is already in use!");
            }

            ThrowIfAnyErrors();

            Logger.LogInformation("Validation passed, proceeding to create new userAssignment entity.");
            var newItem = Domain.Entities.UserAssignment.Create(request.UserId, request.AssignmentId, request.Id);
            Logger.LogInformation("Created new userAssignment entity with Id: {UserAssignmentId}", newItem.Id);

            Logger.LogInformation("Adding userAssignment to repository.");
            await dbContext.UserAssignments!.AddAsync(newItem, cancellationToken);

            Logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Fetching userAssignment by Id: {UserAssignmentId}", newItem.Id);
            var createdItem = await dbContext.UserAssignments!.FindAsync(newItem.Id, cancellationToken);

            Response.UserAssignment = createdItem!.MapToDto();

            Logger.LogInformation("Service completed successfully.");

            await SendOkAsync(Response, cancellation: cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while processing the request.");
            ThrowError("An error has occurred.");
        }
    }
}
