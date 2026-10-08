namespace WebApi.Endpoints.AssignmentImpediment.RemoveAssignmentImpediment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext, ListingChangeNotifier listings) : Endpoint<RemoveAssignmentImpedimentRequest, Response>
{
    public override void Configure()
    {
        Delete("/assignmentImpediment");
        Description(x => x.WithTags("AssignmentImpediments"));

        Summary(s =>
        {
            s.Summary = "Delete assignmentImpediments by Ids";
            s.Description = "Deletes the assignmentImpediments specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
            s.Responses[404] = "An id is missing or not visible; nothing was removed.";
            s.Responses[409] = "A record still has active dependent data; nothing was removed.";
        });
    }

    public override async Task HandleAsync(RemoveAssignmentImpedimentRequest request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Checking if assignmentImpediment entities exist for Ids: {AssignmentImpedimentIds}", string.Join(",", request.Ids));
        var ids = BatchIds.Normalize(request.Ids);
        var items = await dbContext.AssignmentImpediments!.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        if (items.Count != ids.Length)
        {
            Logger.LogWarning("At least one assignment impediment to remove was not found; nothing was removed.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Removing {Count} assignment impediment entities in one transaction.", items.Count);
        foreach (var item in items) Domain.Entities.AssignmentImpediment.Remove(item);

        // A single SaveChanges is one transaction: every removal succeeds or none does.
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Remove result: {Success}", Response.Success);

        if (Response.Success) listings.NotifyChanged(nameof(Domain.Entities.AssignmentImpediment));

        await Send.OkAsync(Response, cancellationToken);
    }
}
