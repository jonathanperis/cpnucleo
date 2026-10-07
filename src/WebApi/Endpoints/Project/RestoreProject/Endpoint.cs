namespace WebApi.Endpoints.Project.RestoreProject;

public class Endpoint(IProjectRepository repository) : Endpoint<RestoreProjectRequest, Response>
{
    public override void Configure()
    {
        Post("/project/restore");
        Description(x => x.WithTags("Projects"));
        Summary(s =>
        {
            s.Summary = "Restore removed projects atomically";
            s.Description = "All supplied removed projects are restored in one transaction, with the memberships removed together with them. A missing, active or invisible project leaves the whole batch unchanged.";
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

        HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged(nameof(Domain.Entities.Project), nameof(Domain.Entities.UserProject));
        await Send.OkAsync(Response, cancellationToken);
    }
}
