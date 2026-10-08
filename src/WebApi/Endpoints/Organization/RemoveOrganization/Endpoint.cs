namespace WebApi.Endpoints.Organization.RemoveOrganization;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork, ListingChangeNotifier listings) : Endpoint<RemoveOrganizationRequest, Response>
{
    public override void Configure()
    {
        Delete("/organization");
        Description(x => x.WithTags("Organizations"));

        Summary(s => {
            s.Summary = "Delete organizations by Ids";
            s.Description = "Deletes the organizations specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
            s.Responses[404] = "An id is missing or not visible; nothing was removed.";
            s.Responses[409] = "A record still has active dependent data; nothing was removed.";
        });   
    }

    public override async Task HandleAsync(RemoveOrganizationRequest request, CancellationToken cancellationToken)
    {        
        var ids = BatchIds.Normalize(request.Ids);

        Logger.LogInformation("Beginning transaction.");
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        Logger.LogInformation("Removing {Count} organization entities atomically.", ids.Length);
        var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
        Response.Success = await repository.RemoveManyAsync(ids, cancellationToken);

        if (!Response.Success)
        {
            Logger.LogWarning("At least one organization to remove was not found; rolling back.");
            await unitOfWork.RollbackAsync(cancellationToken);
            await Send.NotFoundEnvelopeAsync(cancellationToken);
            return;
        }

        Logger.LogInformation("Committing transaction.");
        await unitOfWork.CommitAsync(cancellationToken);

        if (Response.Success) listings.NotifyChanged(nameof(Domain.Entities.Organization));

        await Send.OkAsync(Response, cancellationToken);
    }
}
