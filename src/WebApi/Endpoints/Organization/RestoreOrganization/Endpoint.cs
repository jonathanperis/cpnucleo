namespace WebApi.Endpoints.Organization.RestoreOrganization;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<RestoreOrganizationRequest, Response>
{
    public override void Configure()
    {
        Post("/organization/restore");
        Description(x => x.WithTags("Organizations"));

        Summary(s =>
        {
            s.Summary = "Restore removed organizations by Ids";
            s.Description = "Undoes the soft delete of the organizations specified by the provided Ids in one transaction. If any id is missing, still active or not visible, nothing is restored.";
            s.Responses[404] = "An id is missing, active or not visible; nothing was restored.";
        });
    }

    public override async Task HandleAsync(RestoreOrganizationRequest request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");

        var ids = BatchIds.Normalize(request.Ids, "restored");

        Logger.LogInformation("Beginning transaction.");
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        Logger.LogInformation("Restoring {Count} organization entities atomically.", ids.Length);
        var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
        Response.Success = await repository.RestoreManyAsync(ids, cancellationToken);

        if (!Response.Success)
        {
            Logger.LogWarning("At least one organization to restore was not found or is not removed; rolling back.");
            await unitOfWork.RollbackAsync(cancellationToken);
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Committing transaction.");
        await unitOfWork.CommitAsync(cancellationToken);
        Logger.LogInformation("Service completed successfully.");

        HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged(nameof(Domain.Entities.Organization));

        await Send.OkAsync(Response, cancellationToken);
    }
}
