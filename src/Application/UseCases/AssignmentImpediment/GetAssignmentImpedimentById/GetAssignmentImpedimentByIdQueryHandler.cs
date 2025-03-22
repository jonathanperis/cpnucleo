namespace Application.UseCases.AssignmentImpediment.GetAssignmentImpedimentById;

// Dapper Repository Advanced
public sealed class GetAssignmentImpedimentByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAssignmentImpedimentByIdQuery, GetAssignmentImpedimentByIdQueryViewModel>
{
    public async ValueTask<GetAssignmentImpedimentByIdQueryViewModel> Handle(GetAssignmentImpedimentByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentImpediment>();
        var response = await repository.GetByIdAsync(request.Id);        
        
        var operationResult = response is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetAssignmentImpedimentByIdQueryViewModel(operationResult, response?.MapToDto());
    }
}