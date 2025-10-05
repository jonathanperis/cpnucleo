namespace WebApi.Endpoints.AssignmentImpediment.CreateAssignmentImpediment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/assignmentImpediment");
        Description(x => x.WithTags("AssignmentImpediments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Create a new assignmentImpediment";
            s.Description = "Creates a new assignmentImpediment record with the given data and custom Id. Validates uniqueness and returns the created assignmentImpediment's data.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Service started processing request with payload Description: {Description}, Id: {AssignmentImpedimentId}", request.Description, request.Id);

            Logger.LogInformation("Checking if an assignmentImpediment entity exists with Id: {AssignmentImpedimentId}", request.Id);
            var itemExists = dbContext.AssignmentImpediments!.Any(x => x.Id == request.Id);

            if (itemExists)
            {
                Logger.LogWarning("AssignmentImpediment Id conflict for Id: {AssignmentImpedimentId}", request.Id);
                AddError(r => r.Id, "this Id is already in use!");
            }

            ThrowIfAnyErrors();

            Logger.LogInformation("Validation passed, proceeding to create new assignmentImpediment entity.");
            var newItem = Domain.Entities.AssignmentImpediment.Create(request.Description, request.AssignmentId, request.ImpedimentId, request.Id);
            Logger.LogInformation("Created new assignmentImpediment entity with Id: {AssignmentImpedimentId}", newItem.Id);

            Logger.LogInformation("Adding assignmentImpediment to repository.");
            await dbContext.AssignmentImpediments!.AddAsync(newItem, cancellationToken);

            Logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Fetching assignmentImpediment by Id: {AssignmentImpedimentId}", newItem.Id);
            var createdItem = await dbContext.AssignmentImpediments!.FindAsync(newItem.Id, cancellationToken);

            Response.AssignmentImpediment = createdItem!.MapToDto();

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
