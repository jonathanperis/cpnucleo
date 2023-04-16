namespace Cpnucleo.Shared.Queries.GetWorkflow;

public sealed record GetWorkflowViewModel : BaseQuery
{
    public WorkflowDto Workflow { get; set; }
    public OperationResult OperationResult { get; set; }
}
