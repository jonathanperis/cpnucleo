namespace Application.UseCases.Project.ListProject;

// Dapper Repository Basic
public sealed class ListProjectQueryHandler(IProjectRepository repository) : IRequestHandler<ListProjectQuery, ListProjectQueryViewModel>
{
    public async ValueTask<ListProjectQueryViewModel> Handle(ListProjectQuery request, CancellationToken cancellationToken)
    {
        var response = await repository.GetAllAsync(request.Pagination);        
        
        var operationResult = response.Data != null && response.Data.Any() ? OperationResult.Success : OperationResult.NotFound;

        return new ListProjectQueryViewModel(operationResult, MapToPaginatedDto(response));
    }
    
    private static PaginatedResult<ProjectDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.Project?> result)
    {
        return new PaginatedResult<ProjectDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}