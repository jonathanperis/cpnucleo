namespace WebApi.Endpoints.AssignmentImpediment.UpdateAssignmentImpediment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/api/assignmentImpediment");
        Description(x => x.WithTags("AssignmentImpediments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Update an existing assignmentImpediment";
            s.Description = "Updates the assignmentImpediment identified by the provided Id with new given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        try
        {
            Logger.LogInformation("Checking if an assignmentImpediment entity exists with Id: {AssignmentImpedimentId}", request.Id);
            var item = await dbContext.AssignmentImpediments!.FindAsync([request.Id, cancellationToken], cancellationToken: cancellationToken);

            if (item is null)
            {
                await Send.NotFoundAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Updating assignmentImpediment entity with Id: {AssignmentImpedimentId}", request.Id);
            Domain.Entities.AssignmentImpediment.Update(item, request.Description, request.AssignmentId, request.ImpedimentId);

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
