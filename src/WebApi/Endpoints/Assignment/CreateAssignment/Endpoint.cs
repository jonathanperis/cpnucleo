namespace WebApi.Endpoints.Assignment.CreateAssignment;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/assignment");
        Description(x => x.WithTags("Assignments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Create a new assignment";
            s.Description = "Creates a new assignment record with the given data and custom Id. Validates uniqueness and returns the created assignment's data.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {AssignmentId}", request.Name, request.Id);

            Logger.LogInformation("Checking if an assignment entity exists with Id: {AssignmentId}", request.Id);
            var itemExists = dbContext.Assignments!.Any(x => x.Id == request.Id);

            if (itemExists)
            {
                Logger.LogWarning("Assignment Id conflict for Id: {AssignmentId}", request.Id);
                AddError(r => r.Id, "this Id is already in use!");
            }

            ThrowIfAnyErrors();

            Logger.LogInformation("Validation passed, proceeding to create new assignment entity.");
            var newItem = Domain.Entities.Assignment.Create(request.Name,
                                                            request.Description,
                                                            request.StartDate,
                                                            request.EndDate,
                                                            request.AmountHours,
                                                            request.ProjectId,
                                                            request.WorkflowId,
                                                            request.UserId,
                                                            request.AssignmentTypeId,
                                                            request.Id);
            Logger.LogInformation("Created new assignment entity with Id: {AssignmentId}", newItem.Id);

            Logger.LogInformation("Adding assignment to repository.");
            await dbContext.Assignments!.AddAsync(newItem, cancellationToken);

            Logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            Logger.LogInformation("Fetching assignment by Id: {AssignmentId}", newItem.Id);
            var createdItem = await dbContext.Assignments!.FindAsync(newItem.Id, cancellationToken);

            Response.Assignment = createdItem!.MapToDto();

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
