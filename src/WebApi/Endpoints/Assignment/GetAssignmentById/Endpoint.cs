namespace WebApi.Endpoints.Assignment.GetAssignmentById;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/assignment");
        Description(x => x.WithTags("Assignments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve an assignment by Id";
            s.Description = "Fetches the assignment matching the provided Id. Returns 404 if not found, otherwise returns the assignment data mapped to a DTO.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Fetching assignment entity with Id: {AssignmentId}", request.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.Assignment>();
        var item = await repository.GetByIdAsync(request.Id);

        if (item is null)
        {
            Logger.LogWarning("Assignment not found with Id: {AssignmentId}", request.Id);
            await SendNotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Mapping entity to DTO and setting response for Id: {AssignmentId}", request.Id);
        Response.Assignment = item!.MapToDto();

        Logger.LogInformation("Service completed successfully.");

        await SendOkAsync(Response, cancellation: cancellationToken);
    }
}
