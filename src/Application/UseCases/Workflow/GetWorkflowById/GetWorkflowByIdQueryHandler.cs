namespace Application.UseCases.Workflow.GetWorkflowById;

public sealed class GetWorkflowByIdQueryHandler : IRequestHandler<GetWorkflowByIdQuery, GetWorkflowByIdQueryViewModel>
{
    private readonly IWorkflowRepository _workflowRepository;

    public GetWorkflowByIdQueryHandler(IWorkflowRepository workflowRepository)
    {
        _workflowRepository = workflowRepository;
    }

    public async ValueTask<GetWorkflowByIdQueryViewModel> Handle(GetWorkflowByIdQuery request, CancellationToken cancellationToken)
    {
        var workflow = await _workflowRepository.GetWorkflowById(request.Id);

        var operationResult = workflow is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetWorkflowByIdQueryViewModel(operationResult, workflow);
    }
}
