namespace WebApi.Endpoints.Assignment.RemoveAssignment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Delete("/api/assignment");
        Description(x => x.WithTags("Assignments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Delete assignments by Ids";
            s.Description = "Deletes the assignments specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        try
        {
            Logger.LogInformation("Checking if assignment entities exist for Ids: {AssignmentIds}", string.Join(",", request.Ids));
            var allSuccess = true;

            foreach (var id in request.Ids)
            {
                var item = await dbContext.Assignments!.FindAsync(id, cancellationToken);
                if (item is null)
                {
                    await Send.NotFoundAsync(cancellation: cancellationToken);
                    return;
                }

                Logger.LogInformation("Removing assignment entity with Id: {AssignmentId}", id);
                Domain.Entities.Assignment.Remove(item);

                Logger.LogInformation("Updating repository for removed entity {AssignmentId}.", id);
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
