namespace WebApi.Endpoints.UserProject.RestoreUserProject;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<RestoreUserProjectRequest, Response>
{
    public override void Configure()
    {
        Post("/userProject/restore");
        Description(x => x.WithTags("UserProjects"));

        Summary(s =>
        {
            s.Summary = "Restore removed user projects by Ids";
            s.Description = "Undoes the soft delete of the user projects specified by the provided Ids in one transaction. If any id is missing, still active or not visible, nothing is restored.";
        });
    }

    public override async Task HandleAsync(RestoreUserProjectRequest request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        var ids = BatchIds.Normalize(request.Ids, "restored");
        Logger.LogInformation("Checking that {Count} user project entities are removed.", ids.Length);
        var items = await dbContext.UserProjects!.IgnoreQueryFilters().Where(x => ids.Contains(x.Id) && !x.Active).ToListAsync(cancellationToken);

        if (items.Count != ids.Length)
        {
            Logger.LogWarning("At least one user project to restore was not found or is not removed; nothing was restored.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Restoring {Count} user project entities in one transaction.", items.Count);
        foreach (var item in items) Domain.Entities.UserProject.Restore(item);

        // A single SaveChanges is one transaction: every restore succeeds or none does. Access checks
        // (AccessGuardInterceptor) and the relationship triggers apply as for any other write.
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Restore result: {Success}", Response.Success);
        Logger.LogInformation("Service completed successfully.");

        if (Response.Success) HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged(nameof(Domain.Entities.UserProject));

        await Send.OkAsync(Response, cancellationToken);
    }
}
