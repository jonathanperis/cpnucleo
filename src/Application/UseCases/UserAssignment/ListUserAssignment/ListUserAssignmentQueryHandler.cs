namespace Application.UseCases.UserAssignment.ListUserAssignment;

// Dapper Repository Advanced
public sealed class ListUserAssignmentQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListUserAssignmentQuery, ListUserAssignmentQueryViewModel>
{
    public async ValueTask<ListUserAssignmentQueryViewModel> Handle(ListUserAssignmentQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.UserAssignment>();
        var response = await repository.GetAllAsync(request.Pagination);        
        
        var operationResult = response.Data != null && response.Data.Any() ? OperationResult.Success : OperationResult.NotFound;

        return new ListUserAssignmentQueryViewModel(operationResult, MapToPaginatedDto(response));
    }
    
    private static PaginatedResult<UserAssignmentDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.UserAssignment?> result)
    {
        return new PaginatedResult<UserAssignmentDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}