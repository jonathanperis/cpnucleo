namespace WebApi.Endpoints.Workflow.RemoveWorkflow;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<RemoveWorkflowRequest, Response>
{
    public override void Configure()
    {
        Delete("/workflow");
        Description(x => x.WithTags("Workflows"));

        Summary(s =>
        {
            s.Summary = "Delete workflows by Ids";
            s.Description = "Deletes the workflows specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
            s.Responses[404] = "An id is missing or not visible; nothing was removed.";
            s.Responses[409] = "A record still has active dependent data; nothing was removed.";
        });
    }

    public override async Task HandleAsync(RemoveWorkflowRequest request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Checking if workflow entities exist for Ids: {WorkflowIds}", string.Join(",", request.Ids));
        var ids = BatchIds.Normalize(request.Ids);
        var items = await dbContext.Workflows!.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        if (items.Count != ids.Length)
        {
            Logger.LogWarning("At least one workflow to remove was not found; nothing was removed.");
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Removing {Count} workflow entities in one transaction.", items.Count);
        foreach (var item in items) Domain.Entities.Workflow.Remove(item);

        // A single SaveChanges is one transaction: every removal succeeds or none does.
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Remove result: {Success}", Response.Success);
        Logger.LogInformation("Service completed successfully.");

        if (Response.Success) HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged(nameof(Domain.Entities.Workflow));

        await Send.OkAsync(Response, cancellationToken);
    }
}
