namespace WebApi.Endpoints.AssignmentType.UpdateAssignmentType;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/api/assignmentType");
        Description(x => x.WithTags("AssignmentTypes"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Update an existing assignmentType";
            s.Description = "Updates the assignmentType identified by the provided Id with new given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        try
        {
            Logger.LogInformation("Checking if an assignmentType entity exists with Id: {AssignmentTypeId}", request.Id);
            var item = await dbContext.AssignmentTypes!.FindAsync(request.Id, cancellationToken);

            if (item is null)
            {
                await SendNotFoundAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Updating assignmentType entity with Id: {AssignmentTypeId}", request.Id);
            Domain.Entities.AssignmentType.Update(item, request.Name);

            Logger.LogInformation("Updating entity in repository.");
            Response.Success = await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Update result: {Success}", Response.Success);
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
