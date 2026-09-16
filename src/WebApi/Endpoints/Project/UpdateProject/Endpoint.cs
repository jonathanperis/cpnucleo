namespace WebApi.Endpoints.Project.UpdateProject;

// Dapper Repository Basic
public class Endpoint(IProjectRepository repository) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/project");
        Description(x => x.WithTags("Projects"));

        Summary(s =>
        {
            s.Summary = "Update an existing project";
            s.Description = "Updates the project identified by the provided Id with given data. Validates existence and returns whether the update was successful.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

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
        Response.Success = request.ExpectedVersion is { } version
            ? await repository.UpdateIfVersionAsync(item, version, cancellationToken)
            : await repository.UpdateAsync(item);
        if (!Response.Success && request.ExpectedVersion is not null)
        {
            Response.Message = "The project changed. Reload before saving your changes.";
            await Send.ResponseAsync(Response, StatusCodes.Status409Conflict, cancellationToken);
            return;
        }

        Logger.LogInformation("Update result: {Success}", Response.Success);
        Logger.LogInformation("Service completed successfully.");

        if (Response.Success) HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged();

        await Send.OkAsync(Response, cancellationToken);
    }
}
