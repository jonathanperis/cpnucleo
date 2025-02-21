namespace Application.UseCases.AssignmentType.ListAssignmentType;

// Dapper Repository Advanced
public sealed class ListAssignmentTypeQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListAssignmentTypeQuery, ListAssignmentTypeQueryViewModel>
{
    public async ValueTask<ListAssignmentTypeQueryViewModel> Handle(ListAssignmentTypeQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentType>();
        var response = await repository.GetAllAsync(request.Pagination);        
        
        var operationResult = response.Data != null && response.Data.Any() ? OperationResult.Success : OperationResult.NotFound;

        return new ListAssignmentTypeQueryViewModel(operationResult, MapToPaginatedDto(response));
    }
    
    private static PaginatedResult<AssignmentTypeDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.AssignmentType?> result)
    {
        return new PaginatedResult<AssignmentTypeDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}