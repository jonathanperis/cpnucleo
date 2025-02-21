namespace Application.UseCases.Assignment.GetAssignmentById;

// Dapper Repository Advanced
public sealed class GetAssignmentByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAssignmentByIdQuery, GetAssignmentByIdQueryViewModel>
{
    public async ValueTask<GetAssignmentByIdQueryViewModel> Handle(GetAssignmentByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.Assignment>();
        var response = await repository.GetByIdAsync(request.Id);        
        
        var operationResult = response is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetAssignmentByIdQueryViewModel(operationResult, response?.MapToDto());
    }
}