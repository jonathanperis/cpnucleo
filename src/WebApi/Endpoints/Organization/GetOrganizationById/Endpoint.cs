namespace WebApi.Endpoints.Organization.GetOrganizationById;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/organization");
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
        
        var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
        var item = await repository.GetByIdAsync(request.Id);        
        
        if (item is null)
            await SendNotFoundAsync(cancellation: cancellationToken);

        Response.Organization = item!.MapToDto();

        Logger.LogInformation("Service completed successfully.");
        
        await SendOkAsync(Response, cancellation: cancellationToken);
    }
}