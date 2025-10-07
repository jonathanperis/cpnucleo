namespace WebApi.Endpoints.UserProject.GetUserProjectById;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/userProject");
        Description(x => x.WithTags("UserProjects"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve an userProject by Id";
            s.Description = "Fetches the userProject matching the provided Id. Returns 404 if not found, otherwise returns the userProject data mapped to a DTO.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        Logger.LogInformation("Fetching userProject entity with Id: {UserProjectId}", request.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.UserProject>();
        var item = await repository.GetByIdAsync(request.Id);

        if (item is null)
        {
            Logger.LogWarning("UserProject not found with Id: {UserProjectId}", request.Id);
            await Send.NotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Mapping entity to DTO and setting response for Id: {UserProjectId}", request.Id);
        Response.UserProject = item.MapToDto();

        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);
    }
}
