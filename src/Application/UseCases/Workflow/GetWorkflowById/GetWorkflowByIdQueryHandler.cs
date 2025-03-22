namespace Application.UseCases.Workflow.GetWorkflowById;

// Dapper Repository Advanced
public sealed class GetWorkflowByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetWorkflowByIdQuery, GetWorkflowByIdQueryViewModel>
{
    public async ValueTask<GetWorkflowByIdQueryViewModel> Handle(GetWorkflowByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.Workflow>();
        var response = await repository.GetByIdAsync(request.Id);        
        
        var operationResult = response is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetWorkflowByIdQueryViewModel(operationResult, response?.MapToDto());
    }
}