namespace Application.UseCases.Organization.ListOrganization;

// Dapper Repository Advanced
public sealed class ListOrganizationQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListOrganizationQuery, ListOrganizationQueryViewModel>
{
    public async ValueTask<ListOrganizationQueryViewModel> Handle(ListOrganizationQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
        var response = await repository.GetAllAsync(request.Pagination);        
        
        var operationResult = response.Data != null && response.Data.Any() ? OperationResult.Success : OperationResult.NotFound;

        return new ListOrganizationQueryViewModel(operationResult, MapToPaginatedDto(response));
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
