namespace WebApi.Endpoints.Organization.CreateOrganization;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/organization");
        Tags("Organizations");
        AllowAnonymous();

        Summary(s => {
            s.Summary = "Create a new organization";
            s.Description = "Creates a new organization record with the given name, description, and custom Id. Validates uniqueness and returns the created organization's data.";
        });    
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {        
        Logger.LogInformation("Service started processing request with payload Name: {Name}, Description: {Description}, Id: {OrganizationId}", request.Name, request.Description, request.Id);
        
        try
        {
            Logger.LogInformation("Checking if an organization entity exists with Id: {OrganizationId}", request.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
            var itemExists = await repository.ExistsAsync(request.Id);            
            
            if (itemExists)
            {
                Logger.LogWarning("Organization Id conflict for Id: {OrganizationId}", request.Id);
                AddError(r => r.Id, "this Id is already in use!");
            }
            
            ThrowIfAnyErrors();

            Logger.LogInformation("Validation passed, proceeding to create new organization entity.");
            var newItem = Domain.Entities.Organization.Create(request.Name, request.Description, request.Id);
            Logger.LogInformation("Created new organization entity with Id: {OrganizationId}", newItem.Id);

            Logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();
        
            Logger.LogInformation("Adding organization to repository.");
            var response = await repository.AddAsync(newItem);    
        
            Logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);
            
            Logger.LogInformation("Fetching organization by Id: {OrganizationId}", newItem.Id);
            var createdItem = await repository.GetByIdAsync(newItem.Id);

            Response.Organization = createdItem!.MapToDto();
            
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