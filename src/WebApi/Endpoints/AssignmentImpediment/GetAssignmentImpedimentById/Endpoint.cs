namespace WebApi.Endpoints.AssignmentImpediment.GetAssignmentImpedimentById;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/assignmentImpediment");
        Description(x => x.WithTags("AssignmentImpediments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve an assignmentImpediment by Id";
            s.Description = "Fetches the assignmentImpediment matching the provided Id. Returns 404 if not found, otherwise returns the assignmentImpediment data mapped to a DTO.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Fetching assignmentImpediment entity with Id: {AssignmentImpedimentId}", request.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentImpediment>();
        var item = await repository.GetByIdAsync(request.Id);

        if (item is null)
        {
            Logger.LogWarning("AssignmentImpediment not found with Id: {AssignmentImpedimentId}", request.Id);
            await SendNotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Mapping entity to DTO and setting response for Id: {AssignmentImpedimentId}", request.Id);
        Response.AssignmentImpediment = item!.MapToDto();

        Logger.LogInformation("Service completed successfully.");

        await SendOkAsync(Response, cancellation: cancellationToken);
    }
}
