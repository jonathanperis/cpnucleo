namespace WebApi.Endpoints.Organization.UpdateOrganization;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/api/organization");
        Tags("Organizations");
        AllowAnonymous();

        Summary(s => {
            s.Summary = "short summary goes here";
            s.Description = "long description goes here";
        });   
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {        
        Logger.LogInformation("Service started processing request.");
        
        try
        {
            Logger.LogInformation("Checking if an organization entity exists with Id: {OrganizationId}", request.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
            var item = await repository.GetByIdAsync(request.Id);            
            
            if (item is null)
                await SendNotFoundAsync(cancellation: cancellationToken);

            Logger.LogInformation("Updating organization entity with Id: {OrganizationId}", request.Id);
            Domain.Entities.Organization.Update(item, request.Name, request.Description);

            Logger.LogInformation("Updating entity in repository.");
            Response.Success = await repository.UpdateAsync(item);
            
            Logger.LogInformation("Update result: {Success}", Response.Success);
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