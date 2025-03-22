namespace Application.UseCases.UserProject.ListUserProject;

// Dapper Repository Advanced
public sealed class ListUserProjectQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListUserProjectQuery, ListUserProjectQueryViewModel>
{
    public async ValueTask<ListUserProjectQueryViewModel> Handle(ListUserProjectQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.UserProject>();
        var response = await repository.GetAllAsync(request.Pagination);        
        
        var operationResult = response.Data != null && response.Data.Any() ? OperationResult.Success : OperationResult.NotFound;

        return new ListUserProjectQueryViewModel(operationResult, MapToPaginatedDto(response));
    }
    
    private static PaginatedResult<UserProjectDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.UserProject?> result)
    {
        return new PaginatedResult<UserProjectDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}