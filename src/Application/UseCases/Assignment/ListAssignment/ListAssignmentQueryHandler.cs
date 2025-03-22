namespace Application.UseCases.Assignment.ListAssignment;

// Dapper Repository Advanced
public sealed class ListAssignmentQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListAssignmentQuery, ListAssignmentQueryViewModel>
{
    public async ValueTask<ListAssignmentQueryViewModel> Handle(ListAssignmentQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.Assignment>();
        var response = await repository.GetAllAsync(request.Pagination);        
        
        var operationResult = response.Data != null && response.Data.Any() ? OperationResult.Success : OperationResult.NotFound;

        return new ListAssignmentQueryViewModel(operationResult, MapToPaginatedDto(response));
    }
    
    private static PaginatedResult<AssignmentDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.Assignment?> result)
    {
        return new PaginatedResult<AssignmentDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}