namespace Application.UseCases.User.ListUser;

// Dapper Repository Advanced
public sealed class ListUserQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ListUserQuery, ListUserQueryViewModel>
{
    public async ValueTask<ListUserQueryViewModel> Handle(ListUserQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.User>();
        var response = await repository.GetAllAsync(request.Pagination);        
        
        var operationResult = response.Data != null && response.Data.Any() ? OperationResult.Success : OperationResult.NotFound;

        return new ListUserQueryViewModel(operationResult, MapToPaginatedDto(response));
    }
    
    private static PaginatedResult<UserDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.User?> result)
    {
        return new PaginatedResult<UserDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}