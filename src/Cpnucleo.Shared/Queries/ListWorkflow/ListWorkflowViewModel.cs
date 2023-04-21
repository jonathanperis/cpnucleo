namespace Cpnucleo.Shared.Queries.ListWorkflow;

public sealed record ListWorkflowViewModel : BaseQuery
{
    public List<WorkflowDto>? Workflows { get; set; }
    public required OperationResult OperationResult { get; set; }
}
