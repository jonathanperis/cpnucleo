namespace WebApi.Endpoints.Organization.RemoveOrganization;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Delete("/api/organization");
        Tags("Organizations");
        AllowAnonymous();

        Summary(s => {
            s.Summary = "Delete organizations by Ids";
            s.Description = "Deletes the organizations specified by the provided Ids. Validates existence of each, removes them, updates the repository, and commits the transaction.";
        });   
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {        
        Logger.LogInformation("Service started processing request.");
        
        try
        {
            Logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            Logger.LogInformation("Checking if organization entities exist for Ids: {OrganizationIds}", string.Join(",", request.Ids));
            var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
            var allSuccess = true;

            foreach (var id in request.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    await SendNotFoundAsync(cancellation: cancellationToken);
                    return;
                }

                Logger.LogInformation("Removing organization entity with Id: {OrganizationId}", id);
                Domain.Entities.Organization.Remove(item);

                Logger.LogInformation("Updating repository for removed entity {OrganizationId}.", id);
                var result = await repository.UpdateAsync(item);

                if (!result) allSuccess = false;
            }
            
            Response.Success = allSuccess;
            
            if (!allSuccess)
            {
                Logger.LogWarning("One or more deletions failed, rolling back transaction.");
                await unitOfWork.RollbackAsync(cancellationToken);
                await SendErrorsAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Remove result: {Success}", Response.Success);
            Logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);
            
            Logger.LogInformation("Service completed successfully.");
            
            await SendOkAsync(Response, cancellation: cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while processing the request. Rolling back transaction.");
            await unitOfWork.RollbackAsync(cancellationToken);
            ThrowError("An error has occurred.");
        }
    }
}