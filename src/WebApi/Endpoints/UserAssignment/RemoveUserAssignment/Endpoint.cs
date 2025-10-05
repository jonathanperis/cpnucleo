namespace WebApi.Endpoints.UserAssignment.RemoveUserAssignment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Delete("/api/userAssignment");
        Description(x => x.WithTags("UserAssignments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Delete userAssignments by Ids";
            s.Description = "Deletes the userAssignments specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        try
        {
            Logger.LogInformation("Checking if userAssignment entities exist for Ids: {UserAssignmentIds}", string.Join(",", request.Ids));
            var allSuccess = true;

            foreach (var id in request.Ids)
            {
                var item = await dbContext.UserAssignments!.FindAsync(id, cancellationToken);
                if (item is null)
                {
                    await Send.NotFoundAsync(cancellation: cancellationToken);
                    return;
                }

                Logger.LogInformation("Removing userAssignment entity with Id: {UserAssignmentId}", id);
                Domain.Entities.UserAssignment.Remove(item);

                Logger.LogInformation("Updating repository for removed entity {UserAssignmentId}.", id);
                var result = await dbContext.SaveChangesAsync(cancellationToken);

                if (!result) allSuccess = false;
            }

            Response.Success = allSuccess;

            if (!allSuccess)
            {
                Logger.LogWarning("One or more deletions failed.");
                await Send.ErrorsAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Remove result: {Success}", Response.Success);
            Logger.LogInformation("Service completed successfully.");

            await Send.OkAsync(Response, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while processing the request.");
            ThrowError("An error has occurred.");
        }
    }
}
