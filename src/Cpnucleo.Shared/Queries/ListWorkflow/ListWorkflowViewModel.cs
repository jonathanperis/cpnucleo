namespace Cpnucleo.Shared.Queries.ListWorkflow;

public sealed record ListWorkflowViewModel : BaseQuery
{
    public IEnumerable<WorkflowDto> Workflows { get; set; }
    public OperationResult OperationResult { get; set; }
}
