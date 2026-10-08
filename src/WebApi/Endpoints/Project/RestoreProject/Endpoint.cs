namespace WebApi.Endpoints.Project.RestoreProject;

public class Endpoint(IProjectRepository repository, ListingChangeNotifier listings) : Endpoint<RestoreProjectRequest, Response>
{
    public override void Configure()
    {
        Post("/project/restore");
        Description(x => x.WithTags("Projects"));
        Summary(s =>
        {
            s.Summary = "Restore removed projects atomically";
            s.Description = "All supplied removed projects are restored in one transaction, with the memberships removed together with them. A missing, active or invisible project leaves the whole batch unchanged.";
            s.Responses[404] = "An id is missing, active or not visible; nothing was restored.";
        });
    }

    public override async Task HandleAsync(RestoreProjectRequest request, CancellationToken cancellationToken)
    {
        Response.Success = await repository.RestoreManyAsync(BatchIds.Normalize(request.Ids, "restored"), cancellationToken);
        if (!Response.Success)
        {
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        listings.NotifyChanged(nameof(Domain.Entities.Project), nameof(Domain.Entities.UserProject));
        await Send.OkAsync(Response, cancellationToken);
    }
}
