namespace WebApi.Endpoints.Organization.UpdateOrganization;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Patch("/api/organization");
        Description(x => x.WithTags("Organizations"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Update an existing organization";
            s.Description = "Updates the organization identified by the provided Id with given data. Validates existence and returns whether the update was successful.";
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
            {
                await Send.NotFoundAsync(cancellation: cancellationToken);
                return;
            }

            Logger.LogInformation("Updating organization entity with Id: {OrganizationId}", request.Id);
            Domain.Entities.Organization.Update(item, request.Name, request.Description);

            Logger.LogInformation("Updating entity in repository.");
            Response.Success = await repository.UpdateAsync(item);

            Logger.LogInformation("Update result: {Success}", Response.Success);
            Logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            Logger.LogInformation("Service completed successfully.");

            await Send.OkAsync(Response, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while processing the request. Rolling back transaction.");
            await unitOfWork.RollbackAsync(cancellationToken);
            ThrowError("An error has occurred.");
        }
    }
}
