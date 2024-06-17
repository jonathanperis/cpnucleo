namespace Application.UseCases.Workflow.ListWorkflow;

public sealed class ListWorkflowQueryHandler(IWorkflowRepository workflowRepository) : IRequestHandler<ListWorkflowQuery, ListWorkflowQueryViewModel>
{
    public async ValueTask<ListWorkflowQueryViewModel> Handle(ListWorkflowQuery request, CancellationToken cancellationToken)
    {
        var workflows = await workflowRepository.ListWorkflow();

        var operationResult = workflows is not null ? OperationResult.Success : OperationResult.NotFound;
        var workflowList = workflows ?? [];  // Return an empty list if no workflows are found

        return new ListWorkflowQueryViewModel(operationResult, workflowList);
    }
}
