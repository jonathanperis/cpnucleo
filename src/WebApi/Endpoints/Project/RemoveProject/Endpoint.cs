namespace WebApi.Endpoints.Project.RemoveProject;

public class Endpoint(IProjectRepository repository) : Endpoint<RemoveProjectRequest, Response>
{
    public override void Configure()
    {
        Delete("/project");
        Description(x => x.WithTags("Projects"));
        Summary(s =>
        {
            s.Summary = "Soft-delete projects atomically";
            s.Description = "All supplied active projects are removed in one transaction. A missing project leaves the whole batch unchanged.";
        });
    }

    public override async Task HandleAsync(RemoveProjectRequest request, CancellationToken cancellationToken)
    {
        Response.Success = await repository.RemoveManyAsync(request.Ids, cancellationToken);
        if (!Response.Success)
        {
            await Send.NotFoundAsync(cancellation: cancellationToken);
            return;
        }

        HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged();
        await Send.OkAsync(Response, cancellationToken);
    }
}
