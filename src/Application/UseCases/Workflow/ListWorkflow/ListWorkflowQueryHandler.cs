namespace Application.UseCases.Workflow.ListWorkflow;

// Dapper Repository Advanced
public sealed class ListWorkflowQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListWorkflowQuery, ListWorkflowQueryViewModel>
{
    public async ValueTask<ListWorkflowQueryViewModel> Handle(ListWorkflowQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.Workflow>();
        var response = await repository.GetAllAsync(request.Pagination);        
        
        var operationResult = response.Data != null && response.Data.Any() ? OperationResult.Success : OperationResult.NotFound;

        return new ListWorkflowQueryViewModel(operationResult, MapToPaginatedDto(response));
    }
    
    private static PaginatedResult<WorkflowDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.Workflow?> result)
    {
        return new PaginatedResult<WorkflowDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}