namespace Application.UseCases.User.GetUserById;

// Dapper Repository Advanced
public sealed class GetUserByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryViewModel>
{
    public async ValueTask<GetUserByIdQueryViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.User>();
        var response = await repository.GetByIdAsync(request.Id);        
        
        var operationResult = response is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetUserByIdQueryViewModel(operationResult, response?.MapToDto());
    }
}