namespace WebApi.Endpoints.UserAssignment.RemoveUserAssignment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext, ListingChangeNotifier listings) : Endpoint<RemoveUserAssignmentRequest, Response>
{
    public override void Configure()
    {
        Delete("/userAssignment");
        Description(x => x.WithTags("UserAssignments"));

        Summary(s =>
        {
            s.Summary = "Delete userAssignments by Ids";
            s.Description = "Deletes the userAssignments specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
            s.Responses[404] = "An id is missing or not visible; nothing was removed.";
            s.Responses[409] = "A record still has active dependent data; nothing was removed.";
        });
    }

    public override async Task HandleAsync(RemoveUserAssignmentRequest request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Checking if userAssignment entities exist for Ids: {UserAssignmentIds}", string.Join(",", request.Ids));
        var ids = BatchIds.Normalize(request.Ids);
        var items = await dbContext.UserAssignments!.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        if (items.Count != ids.Length)
        {
            Logger.LogWarning("At least one user assignment to remove was not found; nothing was removed.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Removing {Count} user assignment entities in one transaction.", items.Count);
        foreach (var item in items) Domain.Entities.UserAssignment.Remove(item);

        // A single SaveChanges is one transaction: every removal succeeds or none does.
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Remove result: {Success}", Response.Success);

        if (Response.Success) listings.NotifyChanged(nameof(Domain.Entities.UserAssignment));

        await Send.OkAsync(Response, cancellationToken);
    }
}
