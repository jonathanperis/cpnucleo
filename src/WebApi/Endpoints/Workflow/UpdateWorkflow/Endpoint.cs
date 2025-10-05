namespace WebApi.Endpoints.Workflow.UpdateWorkflow;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/api/workflow");
        Description(x => x.WithTags("Workflows"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Update an existing workflow";
            s.Description = "Updates the workflow identified by the provided Id with new given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        try
        {
            Logger.LogInformation("Checking if an workflow entity exists with Id: {WorkflowId}", request.Id);
            var item = await dbContext.Workflows!.FindAsync(request.Id, cancellationToken);

            if (item is null)
            {
                await SendNotFoundAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Updating workflow entity with Id: {WorkflowId}", request.Id);
            Domain.Entities.Workflow.Update(item, request.Name, request.Order);

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
