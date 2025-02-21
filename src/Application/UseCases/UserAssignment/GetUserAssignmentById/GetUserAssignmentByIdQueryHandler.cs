namespace Application.UseCases.UserAssignment.GetUserAssignmentById;

// Dapper Repository Advanced
public sealed class GetUserAssignmentByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserAssignmentByIdQuery, GetUserAssignmentByIdQueryViewModel>
{
    public async ValueTask<GetUserAssignmentByIdQueryViewModel> Handle(GetUserAssignmentByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.UserAssignment>();
        var response = await repository.GetByIdAsync(request.Id);        
        
        var operationResult = response is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetUserAssignmentByIdQueryViewModel(operationResult, response?.MapToDto());
    }
}