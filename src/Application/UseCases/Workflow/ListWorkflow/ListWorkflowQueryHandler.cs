namespace Application.UseCases.Workflow.ListWorkflow;

public sealed class ListWorkflowQueryHandler : IRequestHandler<ListWorkflowQuery, ListWorkflowQueryViewModel>
{
    private readonly IWorkflowRepository _workflowRepository;

    public ListWorkflowQueryHandler(IWorkflowRepository workflowRepository)
    {
        _workflowRepository = workflowRepository;
    }

    public async ValueTask<ListWorkflowQueryViewModel> Handle(ListWorkflowQuery request, CancellationToken cancellationToken)
    {
        var workflows = await _workflowRepository.ListWorkflow();

        var operationResult = workflows is not null ? OperationResult.Success : OperationResult.NotFound;
        var workflowList = workflows ?? new List<WorkflowDto>();  // Return an empty list if no workflows are found

        return new ListWorkflowQueryViewModel(operationResult, workflowList);
    }
}
