namespace WebApi.Endpoints.Assignment.UpdateAssignment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/api/assignment");
        Description(x => x.WithTags("Assignments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Update an existing assignment";
            s.Description = "Updates the assignment identified by the provided Id with new given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        try
        {
            Logger.LogInformation("Checking if an assignment entity exists with Id: {AssignmentId}", request.Id);
            var item = await dbContext.Assignments!.FindAsync(request.Id, cancellationToken);

            if (item is null)
            {
                await Send.NotFoundAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Updating assignment entity with Id: {AssignmentId}", request.Id);
            Domain.Entities.Assignment.Update(item,
                                              request.Name,
                                              request.Description,
                                              request.StartDate,
                                              request.EndDate,
                                              request.AmountHours,
                                              request.ProjectId,
                                              request.WorkflowId,
                                              request.UserId,
                                              request.AssignmentTypeId);

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
