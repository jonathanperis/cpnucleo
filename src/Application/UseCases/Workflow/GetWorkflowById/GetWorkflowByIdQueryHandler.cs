namespace Application.UseCases.Workflow.GetWorkflowById;

public sealed class GetWorkflowByIdQueryHandler(IWorkflowRepository workflowRepository) : IRequestHandler<GetWorkflowByIdQuery, GetWorkflowByIdQueryViewModel>
{
    public async ValueTask<GetWorkflowByIdQueryViewModel> Handle(GetWorkflowByIdQuery request, CancellationToken cancellationToken)
    {
        var workflow = await workflowRepository.GetWorkflowById(request.Id);

        var operationResult = workflow is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetWorkflowByIdQueryViewModel(operationResult, workflow?.MapToDto());
    }
}
