namespace WebApi.Endpoints.AssignmentType.RestoreAssignmentType;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<RestoreAssignmentTypeRequest, Response>
{
    public override void Configure()
    {
        Post("/assignmentType/restore");
        Description(x => x.WithTags("AssignmentTypes"));

        Summary(s =>
        {
            s.Summary = "Restore removed assignment types by Ids";
            s.Description = "Undoes the soft delete of the assignment types specified by the provided Ids in one transaction. If any id is missing, still active or not visible, nothing is restored.";
        });
    }

    public override async Task HandleAsync(RestoreAssignmentTypeRequest request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        var ids = BatchIds.Normalize(request.Ids, "restored");
        Logger.LogInformation("Checking that {Count} assignment type entities are removed.", ids.Length);
        var items = await dbContext.AssignmentTypes!.IgnoreQueryFilters().Where(x => ids.Contains(x.Id) && !x.Active).ToListAsync(cancellationToken);

        if (items.Count != ids.Length)
        {
            Logger.LogWarning("At least one assignment type to restore was not found or is not removed; nothing was restored.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Restoring {Count} assignment type entities in one transaction.", items.Count);
        foreach (var item in items) Domain.Entities.AssignmentType.Restore(item);

        // A single SaveChanges is one transaction: every restore succeeds or none does. Access checks
        // (AccessGuardInterceptor) and the relationship triggers apply as for any other write.
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Restore result: {Success}", Response.Success);
        Logger.LogInformation("Service completed successfully.");

        if (Response.Success) HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged(nameof(Domain.Entities.AssignmentType));

        await Send.OkAsync(Response, cancellationToken);
    }
}
