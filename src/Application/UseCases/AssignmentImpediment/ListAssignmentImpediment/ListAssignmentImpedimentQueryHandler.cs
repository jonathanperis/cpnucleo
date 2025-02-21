namespace Application.UseCases.AssignmentImpediment.ListAssignmentImpediment;

// Dapper Repository Advanced
public sealed class ListAssignmentImpedimentQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListAssignmentImpedimentQuery, ListAssignmentImpedimentQueryViewModel>
{
    public async ValueTask<ListAssignmentImpedimentQueryViewModel> Handle(ListAssignmentImpedimentQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentImpediment>();
        var response = await repository.GetAllAsync(request.Pagination);        
        
        var operationResult = response.Data != null && response.Data.Any() ? OperationResult.Success : OperationResult.NotFound;

        return new ListAssignmentImpedimentQueryViewModel(operationResult, MapToPaginatedDto(response));
    }
    
    private static PaginatedResult<AssignmentImpedimentDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.AssignmentImpediment?> result)
    {
        return new PaginatedResult<AssignmentImpedimentDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}