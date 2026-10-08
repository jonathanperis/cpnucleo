namespace WebApi.Endpoints.Impediment.RemoveImpediment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<RemoveImpedimentRequest, Response>
{
    public override void Configure()
    {
        Delete("/impediment");
        Description(x => x.WithTags("Impediments"));

        Summary(s =>
        {
            s.Summary = "Delete impediments by Ids";
            s.Description = "Deletes the impediments specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
            s.Responses[404] = "An id is missing or not visible; nothing was removed.";
            s.Responses[409] = "A record still has active dependent data; nothing was removed.";
        });
    }

    public override async Task HandleAsync(RemoveImpedimentRequest request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Checking if impediment entities exist for Ids: {ImpedimentIds}", string.Join(",", request.Ids));
        var ids = BatchIds.Normalize(request.Ids);
        var items = await dbContext.Impediments!.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        if (items.Count != ids.Length)
        {
            Logger.LogWarning("At least one impediment to remove was not found; nothing was removed.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Removing {Count} impediment entities in one transaction.", items.Count);
        foreach (var item in items) Domain.Entities.Impediment.Remove(item);

        // A single SaveChanges is one transaction: every removal succeeds or none does.
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Remove result: {Success}", Response.Success);
        Logger.LogInformation("Service completed successfully.");

        if (Response.Success) HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged(nameof(Domain.Entities.Impediment));

        await Send.OkAsync(Response, cancellationToken);
    }
}
