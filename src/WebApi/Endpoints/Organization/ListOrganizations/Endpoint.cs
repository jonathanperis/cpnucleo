namespace WebApi.Endpoints.Organization.ListOrganizations;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/organizations");
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
        var response = await repository.GetAllAsync(request.Pagination);        
        
        Response.Result = MapToPaginatedDto(response);

        Logger.LogInformation("Service completed successfully.");
        
        await SendOkAsync(Response, cancellation: cancellationToken);
    }

    private static PaginatedResult<OrganizationDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.Organization?> result)
    {
        return new PaginatedResult<OrganizationDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }    
}