namespace Cpnucleo.Shared.Queries.Workflow;

public sealed record GetWorkflowViewModel : BaseQuery
{
    public WorkflowDTO Workflow { get; set; }
    public OperationResult OperationResult { get; set; }
}
