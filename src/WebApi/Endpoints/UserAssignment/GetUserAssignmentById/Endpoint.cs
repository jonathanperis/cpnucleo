namespace WebApi.Endpoints.UserAssignment.GetUserAssignmentById;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/userAssignment");
        Description(x => x.WithTags("UserAssignments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve an userAssignment by Id";
            s.Description = "Fetches the userAssignment matching the provided Id. Returns 404 if not found, otherwise returns the userAssignment data mapped to a DTO.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Fetching userAssignment entity with Id: {UserAssignmentId}", request.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.UserAssignment>();
        var item = await repository.GetByIdAsync(request.Id);

        if (item is null)
        {
            Logger.LogWarning("UserAssignment not found with Id: {UserAssignmentId}", request.Id);
            await Send.NotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Mapping entity to DTO and setting response for Id: {UserAssignmentId}", request.Id);
        Response.UserAssignment = item.MapToDto();

        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);
    }
}
