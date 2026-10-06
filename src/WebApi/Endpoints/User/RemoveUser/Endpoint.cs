namespace WebApi.Endpoints.User.RemoveUser;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<RemoveUserRequest, Response>
{
    public override void Configure()
    {
        Delete("/user");
        Description(x => x.WithTags("Users"));
        Policies("UserAdministration");

        Summary(s =>
        {
            s.Summary = "Delete users by Ids";
            s.Description = "Deletes the users specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
        });
    }

    public override async Task HandleAsync(RemoveUserRequest request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Checking if user entities exist for Ids: {UserIds}", string.Join(",", request.Ids));
        var ids = BatchIds.Normalize(request.Ids);
        var items = await dbContext.Users!.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        if (items.Count != ids.Length)
        {
            Logger.LogWarning("At least one user to remove was not found; nothing was removed.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Removing {Count} user entities in one transaction.", items.Count);
        foreach (var item in items) Domain.Entities.User.Remove(item);

        // A single SaveChanges is one transaction: every removal succeeds or none does.
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Remove result: {Success}", Response.Success);
        Logger.LogInformation("Service completed successfully.");

        if (Response.Success) HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged(nameof(Domain.Entities.User), nameof(Domain.Entities.UserProject), nameof(Domain.Entities.UserAssignment));

        await Send.OkAsync(Response, cancellationToken);
    }
}
