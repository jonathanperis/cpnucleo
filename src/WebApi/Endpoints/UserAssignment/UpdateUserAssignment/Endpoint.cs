namespace WebApi.Endpoints.UserAssignment.UpdateUserAssignment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/api/userAssignment");
        Description(x => x.WithTags("UserAssignments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Update an existing userAssignment";
            s.Description = "Updates the userAssignment identified by the provided Id with new given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        try
        {
            Logger.LogInformation("Checking if an userAssignment entity exists with Id: {UserAssignmentId}", request.Id);
            var item = await dbContext.UserAssignments!.FindAsync([request.Id, cancellationToken], cancellationToken: cancellationToken);

            if (item is null)
            {
                await Send.NotFoundAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Updating userAssignment entity with Id: {UserAssignmentId}", request.Id);
            Domain.Entities.UserAssignment.Update(item, request.UserId, request.AssignmentId);

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
