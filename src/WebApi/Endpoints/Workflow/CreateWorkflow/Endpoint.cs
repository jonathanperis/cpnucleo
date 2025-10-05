namespace WebApi.Endpoints.Workflow.CreateWorkflow;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/workflow");
        Description(x => x.WithTags("Workflows"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Create a new workflow";
            s.Description = "Creates a new workflow record with the given data and custom Id. Validates uniqueness and returns the created workflow's data.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {WorkflowId}", request.Name, request.Id);

            Logger.LogInformation("Checking if an workflow entity exists with Id: {WorkflowId}", request.Id);
            var itemExists = dbContext.Workflows!.Any(x => x.Id == request.Id);

            if (itemExists)
            {
                Logger.LogWarning("Workflow Id conflict for Id: {WorkflowId}", request.Id);
                AddError(r => r.Id, "this Id is already in use!");
            }

            ThrowIfAnyErrors();

            Logger.LogInformation("Validation passed, proceeding to create new workflow entity.");
            var newItem = Domain.Entities.Workflow.Create(request.Name, request.Order, request.Id);
            Logger.LogInformation("Created new workflow entity with Id: {WorkflowId}", newItem.Id);

            Logger.LogInformation("Adding workflow to repository.");
            await dbContext.Workflows!.AddAsync(newItem, cancellationToken);

            Logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Fetching workflow by Id: {WorkflowId}", newItem.Id);
            var createdItem = await dbContext.Workflows!.FindAsync(newItem.Id, cancellationToken);

            Response.Workflow = createdItem!.MapToDto();

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
