namespace WebApi.Endpoints.Project.UpdateProject;

// Dapper Repository Basic
public class Endpoint(IProjectRepository repository, ListingChangeNotifier listings) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/project");
        Description(x => x.WithTags("Projects"));

        Summary(s =>
        {
            s.Summary = "Update an existing project";
            s.Description = "Updates the project identified by the provided Id with given data. Validates existence and returns whether the update was successful.";
            s.Responses[409] = "expectedVersion is stale: the project changed after it was read.";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Checking if an project entity exists with Id: {ProjectId}", request.Id);
        var item = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (item is null)
        {
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Updating project entity with Id: {ProjectId}", request.Id);
        Domain.Entities.Project.Update(item, request.Name, request.OrganizationId);

        Logger.LogInformation("Updating entity in repository.");
        Response.Success = request.ExpectedVersion is { } version
            ? await repository.UpdateIfVersionAsync(item, version, cancellationToken)
            : await repository.UpdateAsync(item, cancellationToken);
        if (!Response.Success && request.ExpectedVersion is not null)
        {
            Logger.LogInformation("Project {ProjectId} changed since version {ExpectedVersion}.", request.Id, request.ExpectedVersion);
            await Send.ConflictEnvelopeAsync("The project changed. Reload before saving your changes.", cancellationToken);
            return;
        }

        Logger.LogInformation("Update result: {Success}", Response.Success);

        if (Response.Success) listings.NotifyChanged(nameof(Domain.Entities.Project));

        await Send.OkAsync(Response, cancellationToken);
    }
}
