namespace WebApi.Endpoints.Impediment.GetImpedimentById;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/impediment");
        Description(x => x.WithTags("Impediments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve an impediment by Id";
            s.Description = "Fetches the impediment matching the provided Id. Returns 404 if not found, otherwise returns the impediment data mapped to a DTO.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Fetching impediment entity with Id: {ImpedimentId}", request.Id);
        var item = await dbContext.Impediments!.FindAsync([request.Id, cancellationToken], cancellationToken: cancellationToken);

        if (item is null)
        {
            Logger.LogWarning("Impediment not found with Id: {ImpedimentId}", request.Id);
            await Send.NotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Mapping entity to DTO and setting response for Id: {ImpedimentId}", request.Id);
        Response.Impediment = item.MapToDto();

        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);
    }
}
