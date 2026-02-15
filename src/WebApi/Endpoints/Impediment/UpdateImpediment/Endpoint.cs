namespace WebApi.Endpoints.Impediment.UpdateImpediment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/api/impediment");
        Description(x => x.WithTags("Impediments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Update an existing impediment";
            s.Description = "Updates the impediment identified by the provided Id with given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Checking if an impediment entity exists with Id: {ImpedimentId}", request.Id);
        var item = await dbContext.Impediments!.FindAsync([request.Id, cancellationToken], cancellationToken: cancellationToken);

        if (item is null)
        {
            await Send.NotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Updating impediment entity with Id: {ImpedimentId}", request.Id);
        Domain.Entities.Impediment.Update(item, request.Name);

        Logger.LogInformation("Updating entity in repository.");
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Update result: {Success}", Response.Success);
        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);
    }
}
