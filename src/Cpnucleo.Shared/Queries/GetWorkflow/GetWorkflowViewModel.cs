namespace Cpnucleo.Shared.Queries.GetWorkflow;

public sealed record GetWorkflowViewModel : BaseQuery
{
    public WorkflowDto? Workflow { get; set; }
    public required OperationResult OperationResult { get; set; }
}
