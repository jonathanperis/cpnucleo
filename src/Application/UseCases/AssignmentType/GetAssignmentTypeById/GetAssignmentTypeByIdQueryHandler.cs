namespace Application.UseCases.AssignmentType.GetAssignmentTypeById;

// Dapper Repository Advanced
public sealed class GetAssignmentTypeByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAssignmentTypeByIdQuery, GetAssignmentTypeByIdQueryViewModel>
{
    public async ValueTask<GetAssignmentTypeByIdQueryViewModel> Handle(GetAssignmentTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentType>();
        var response = await repository.GetByIdAsync(request.Id);        
        
        var operationResult = response is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetAssignmentTypeByIdQueryViewModel(operationResult, response?.MapToDto());
    }
}