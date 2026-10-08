namespace WebApi.Endpoints.AssignmentImpediment.RestoreAssignmentImpediment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext, ListingChangeNotifier listings) : Endpoint<RestoreAssignmentImpedimentRequest, Response>
{
    public override void Configure()
    {
        Post("/assignmentImpediment/restore");
        Description(x => x.WithTags("AssignmentImpediments"));

        Summary(s =>
        {
            s.Summary = "Restore removed assignment impediments by Ids";
            s.Description = "Undoes the soft delete of the assignment impediments specified by the provided Ids in one transaction. If any id is missing, still active or not visible, nothing is restored.";
            s.Responses[404] = "An id is missing, active or not visible; nothing was restored.";
        });
    }

    public override async Task HandleAsync(RestoreAssignmentImpedimentRequest request, CancellationToken cancellationToken)
    {
        var ids = BatchIds.Normalize(request.Ids, "restored");
        Logger.LogInformation("Checking that {Count} assignment impediment entities are removed.", ids.Length);
        var items = await dbContext.AssignmentImpediments!.IgnoreQueryFilters().Where(x => ids.Contains(x.Id) && !x.Active).ToListAsync(cancellationToken);

        if (items.Count != ids.Length)
        {
            Logger.LogWarning("At least one assignment impediment to restore was not found or is not removed; nothing was restored.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Restoring {Count} assignment impediment entities in one transaction.", items.Count);
        foreach (var item in items) Domain.Entities.AssignmentImpediment.Restore(item);

        // A single SaveChanges is one transaction: every restore succeeds or none does. Access checks
        // (AccessGuardInterceptor) and the relationship triggers apply as for any other write.
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Restore result: {Success}", Response.Success);

        if (Response.Success) listings.NotifyChanged(nameof(Domain.Entities.AssignmentImpediment));

        await Send.OkAsync(Response, cancellationToken);
    }
}
