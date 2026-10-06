namespace WebApi.Endpoints.Organization.RemoveOrganization;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<RemoveOrganizationRequest, Response>
{
    public override void Configure()
    {
        Delete("/organization");
        Description(x => x.WithTags("Organizations"));

        Summary(s => {
            s.Summary = "Delete organizations by Ids";
            s.Description = "Deletes the organizations specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
        });   
    }

    public override async Task HandleAsync(RemoveOrganizationRequest request, CancellationToken cancellationToken)
    {        
        Logger.LogInformation("Service started processing request.");

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
        Logger.LogInformation("Service completed successfully.");

        if (Response.Success) HttpContext.RequestServices.GetRequiredService<ListingChangeNotifier>().NotifyChanged(nameof(Domain.Entities.Organization));

        await Send.OkAsync(Response, cancellationToken);
    }
}
