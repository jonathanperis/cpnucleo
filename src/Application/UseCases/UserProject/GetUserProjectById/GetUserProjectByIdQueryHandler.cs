namespace Application.UseCases.UserProject.GetUserProjectById;

// Dapper Repository Advanced
public sealed class GetUserProjectByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserProjectByIdQuery, GetUserProjectByIdQueryViewModel>
{
    public async ValueTask<GetUserProjectByIdQueryViewModel> Handle(GetUserProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.UserProject>();
        var response = await repository.GetByIdAsync(request.Id);        
        
        var operationResult = response is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetUserProjectByIdQueryViewModel(operationResult, response?.MapToDto());
    }
}