namespace WebApi.Endpoints.Project.UpdateProject;

// Dapper Repository Basic
public class Endpoint(IProjectRepository repository) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/api/project");
        Description(x => x.WithTags("Projects"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Update an existing project";
            s.Description = "Updates the project identified by the provided Id with given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        try
        {
            Logger.LogInformation("Checking if an project entity exists with Id: {ProjectId}", request.Id);
            var item = await repository.GetByIdAsync(request.Id);

            if (item is null)
            {
                await Send.NotFoundAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Updating project entity with Id: {ProjectId}", request.Id);
            Domain.Entities.Project.Update(item, request.Name, request.OrganizationId);

            Logger.LogInformation("Updating entity in repository.");
            Response.Success = await repository.UpdateAsync(item);

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
