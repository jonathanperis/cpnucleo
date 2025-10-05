namespace WebApi.Endpoints.Project.GetProjectById;

// Dapper Repository Basic
public class Endpoint(IProjectRepository repository) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/project");
        Description(x => x.WithTags("Projects"));
        AllowAnonymous();

        Summary(s => {
            s.Summary = "Retrieve an project by Id";
            s.Description = "Fetches the project matching the provided Id. Returns 404 if not found, otherwise returns the project data mapped to a DTO.";
        });           
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {        
        Logger.LogInformation("Service started processing request.");
        
        Logger.LogInformation("Fetching project entity with Id: {ProjectId}", request.Id);
        var item = await repository.GetByIdAsync(request.Id);        
        
        if (item is null)
        {
            Logger.LogWarning("Project not found with Id: {ProjectId}", request.Id);
            await Send.NotFoundAsync(cancellation: cancellationToken);
            return;
        }

        Logger.LogInformation("Mapping entity to DTO and setting response for Id: {ProjectId}", request.Id);
        Response.Project = item!.MapToDto();

        Logger.LogInformation("Service completed successfully.");
        
        await Send.OkAsync(Response, cancellationToken);
    }
}