namespace WebApi.Endpoints.UserAssignment.UpdateUserAssignment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext, ListingChangeNotifier listings) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/userAssignment");
        Description(x => x.WithTags("UserAssignments"));

        Summary(s =>
        {
            s.Summary = "Update an existing userAssignment";
            s.Description = "Updates the userAssignment identified by the provided Id with new given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Checking if an userAssignment entity exists with Id: {UserAssignmentId}", request.Id);
        var item = await dbContext.UserAssignments!.FindAsync([request.Id], cancellationToken: cancellationToken);

        if (item is null)
        {
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Updating userAssignment entity with Id: {UserAssignmentId}", request.Id);
        Domain.Entities.UserAssignment.Update(item, request.UserId, request.AssignmentId);

        Logger.LogInformation("Updating entity in repository.");
        Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Update result: {Success}", Response.Success);

        if (Response.Success) listings.NotifyChanged(nameof(Domain.Entities.UserAssignment));

        await Send.OkAsync(Response, cancellationToken);
    }
}
