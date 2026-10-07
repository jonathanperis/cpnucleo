namespace WebApi.Endpoints.Assignment.RestoreAssignment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<RestoreAssignmentRequest, Response>
{
    public override void Configure()
    {
        Post("/assignment/restore");
        Description(x => x.WithTags("Assignments"));

        Summary(s =>
        {
            s.Summary = "Restore removed assignments by Ids";
            s.Description = "Undoes the soft delete of the assignments specified by the provided Ids in one transaction. If any id is missing, still active or not visible, nothing is restored.";
        });
    }

    public override async Task HandleAsync(RestoreAssignmentRequest request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        var ids = BatchIds.Normalize(request.Ids, "restored");
        Logger.LogInformation("Checking that {Count} assignment entities are removed.", ids.Length);
        var items = await dbContext.Assignments!.IgnoreQueryFilters().Where(x => ids.Contains(x.Id) && !x.Active).ToListAsync(cancellationToken);

        if (items.Count != ids.Length)
        {
            Logger.LogWarning("At least one assignment to restore was not found or is not removed; nothing was restored.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Restoring {Count} assignment entities in one transaction.", items.Count);
        foreach (var item in items) Domain.Entities.Assignment.Restore(item);

        // A single SaveChanges is one transaction: every restore succeeds or none does. Access checks
        // (AccessGuardInterceptor) and the relationship triggers apply as for any other write.
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Restore result: {Success}", Response.Success);
        Logger.LogInformation("Service completed successfully.");

        if (Response.Success) HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged(nameof(Domain.Entities.Assignment), nameof(Domain.Entities.UserAssignment));

        await Send.OkAsync(Response, cancellationToken);
    }
}
