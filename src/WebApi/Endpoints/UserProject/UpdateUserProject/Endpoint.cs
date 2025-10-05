namespace WebApi.Endpoints.UserProject.UpdateUserProject;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/api/userProject");
        Description(x => x.WithTags("UserProjects"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Update an existing userProject";
            s.Description = "Updates the userProject identified by the provided Id with new given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        try
        {
            Logger.LogInformation("Checking if an userProject entity exists with Id: {UserProjectId}", request.Id);
            var item = await dbContext.UserProjects!.FindAsync(request.Id, cancellationToken);

            if (item is null)
            {
                await Send.NotFoundAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Updating userProject entity with Id: {UserProjectId}", request.Id);
            Domain.Entities.UserProject.Update(item, request.UserId, request.ProjectId);

            Logger.LogInformation("Updating entity in repository.");
            Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Update result: {Success}", Response.Success);
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
