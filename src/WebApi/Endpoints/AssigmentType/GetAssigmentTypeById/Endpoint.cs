namespace WebApi.Endpoints.AssignmentType.GetAssignmentTypeById;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/assignmentType");
        Description(x => x.WithTags("AssignmentTypes"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve an assignmentType by Id";
            s.Description = "Fetches the assignmentType matching the provided Id. Returns 404 if not found, otherwise returns the assignmentType data mapped to a DTO.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Fetching assignmentType entity with Id: {AssignmentTypeId}", request.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentType>();
        var item = await repository.GetByIdAsync(request.Id);

        if (item is null)
        {
            Logger.LogWarning("AssignmentType not found with Id: {AssignmentTypeId}", request.Id);
            await SendNotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Mapping entity to DTO and setting response for Id: {AssignmentTypeId}", request.Id);
        Response.AssignmentType = item!.MapToDto();

        Logger.LogInformation("Service completed successfully.");

        await SendOkAsync(Response, cancellation: cancellationToken);
    }
}
