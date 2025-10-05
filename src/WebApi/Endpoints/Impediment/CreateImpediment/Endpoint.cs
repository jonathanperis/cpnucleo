namespace WebApi.Endpoints.Impediment.CreateImpediment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/impediment");
        Description(x => x.WithTags("Impediments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Create a new impediment";
            s.Description = "Creates a new impediment record with the given data and custom Id. Validates uniqueness and returns the created impediment's data.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {ImpedimentId}", request.Name, request.Id);

            Logger.LogInformation("Checking if an impediment entity exists with Id: {ImpedimentId}", request.Id);
            var itemExists = dbContext.Impediments!.Any(x => x.Id == request.Id);

            if (itemExists)
            {
                Logger.LogWarning("Impediment Id conflict for Id: {ImpedimentId}", request.Id);
                AddError(r => r.Id, "this Id is already in use!");
            }

            ThrowIfAnyErrors();

            Logger.LogInformation("Validation passed, proceeding to create new impediment entity.");
            var newItem = Domain.Entities.Impediment.Create(request.Name, request.Id);
            Logger.LogInformation("Created new impediment entity with Id: {ImpedimentId}", newItem.Id);

            Logger.LogInformation("Adding impediment to repository.");
            await dbContext.Impediments!.AddAsync(newItem, cancellationToken);

            Logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Fetching impediment by Id: {ImpedimentId}", newItem.Id);
            var createdItem = await dbContext.Impediments!.FindAsync(newItem.Id, cancellationToken);

            Response.Impediment = createdItem!.MapToDto();

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
