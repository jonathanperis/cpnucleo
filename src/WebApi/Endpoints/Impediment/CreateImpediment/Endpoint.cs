namespace WebApi.Endpoints.Impediment.CreateImpediment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext, ListingChangeNotifier listings) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/impediment");
        Description(x => x.WithTags("Impediments"));

        Summary(s =>
        {
            s.Summary = "Create a new impediment";
            s.Description = "Creates a new impediment record with the given data and custom Id. Validates uniqueness and returns the created impediment's data.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Checking if an impediment entity exists with Id: {ImpedimentId}", request.Id);
        var itemExists = await dbContext.Impediments!.IgnoreQueryFilters().AnyAsync(x => x.Id == request.Id, cancellationToken);

        if (itemExists)
        {
            Logger.LogWarning("Impediment Id conflict for Id: {ImpedimentId}", request.Id);
            ThrowError(r => r.Id, "this Id is already in use!", StatusCodes.Status409Conflict);
        }

        Logger.LogInformation("Validation passed, proceeding to create new impediment entity.");
        var newItem = Domain.Entities.Impediment.Create(request.Name, request.Id);
        Logger.LogInformation("Created new impediment entity with Id: {ImpedimentId}", newItem.Id);

        Logger.LogInformation("Adding impediment to repository.");
        await dbContext.Impediments!.AddAsync(newItem, cancellationToken);

        Logger.LogInformation("Committing transaction.");
        await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Fetching impediment by Id: {ImpedimentId}", newItem.Id);
        var createdItem = await dbContext.Impediments!.FindAsync([newItem.Id], cancellationToken: cancellationToken);

        Response.Impediment = createdItem!.MapToDto();

        listings.NotifyChanged(nameof(Domain.Entities.Impediment));

        await Send.OkAsync(Response, cancellationToken);
    }
}
