namespace WebApi.Endpoints.Impediment.RestoreImpediment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<RestoreImpedimentRequest, Response>
{
    public override void Configure()
    {
        Post("/impediment/restore");
        Description(x => x.WithTags("Impediments"));

        Summary(s =>
        {
            s.Summary = "Restore removed impediments by Ids";
            s.Description = "Undoes the soft delete of the impediments specified by the provided Ids in one transaction. If any id is missing, still active or not visible, nothing is restored.";
            s.Responses[404] = "An id is missing, active or not visible; nothing was restored.";
        });
    }

    public override async Task HandleAsync(RestoreImpedimentRequest request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        var ids = BatchIds.Normalize(request.Ids, "restored");
        Logger.LogInformation("Checking that {Count} impediment entities are removed.", ids.Length);
        var items = await dbContext.Impediments!.IgnoreQueryFilters().Where(x => ids.Contains(x.Id) && !x.Active).ToListAsync(cancellationToken);

        if (items.Count != ids.Length)
        {
            Logger.LogWarning("At least one impediment to restore was not found or is not removed; nothing was restored.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Restoring {Count} impediment entities in one transaction.", items.Count);
        foreach (var item in items) Domain.Entities.Impediment.Restore(item);

        // A single SaveChanges is one transaction: every restore succeeds or none does. Access checks
        // (AccessGuardInterceptor) and the relationship triggers apply as for any other write.
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Restore result: {Success}", Response.Success);
        Logger.LogInformation("Service completed successfully.");

        if (Response.Success) HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged(nameof(Domain.Entities.Impediment));

        await Send.OkAsync(Response, cancellationToken);
    }
}
