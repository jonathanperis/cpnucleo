namespace WebApi.Endpoints.UserProject.RemoveUserProject;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Delete("/api/userProject");
        Description(x => x.WithTags("UserProjects"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Delete userProjects by Ids";
            s.Description = "Deletes the userProjects specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        try
        {
            Logger.LogInformation("Checking if userProject entities exist for Ids: {UserProjectIds}", string.Join(",", request.Ids));
            var allSuccess = true;

            foreach (var id in request.Ids)
            {
                var item = await dbContext.UserProjects!.FindAsync(id, cancellationToken);
                if (item is null)
                {
                    await SendNotFoundAsync(cancellation: cancellationToken);
                    return;
                }

                Logger.LogInformation("Removing userProject entity with Id: {UserProjectId}", id);
                Domain.Entities.UserProject.Remove(item);

                Logger.LogInformation("Updating repository for removed entity {UserProjectId}.", id);
                var result = await dbContext.SaveChangesAsync(cancellationToken);

                if (!result) allSuccess = false;
            }

            Response.Success = allSuccess;

            if (!allSuccess)
            {
                Logger.LogWarning("One or more deletions failed.");
                await SendErrorsAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Remove result: {Success}", Response.Success);
            Logger.LogInformation("Service completed successfully.");

            await SendOkAsync(Response, cancellation: cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while processing the request.");
            ThrowError("An error has occurred.");
        }
    }
}
