namespace Cpnucleo.Shared.Queries.ListWorkflow;

public sealed record ListWorkflowViewModel : BaseQuery
{
    public IEnumerable<WorkflowDTO> Workflows { get; set; }
    public OperationResult OperationResult { get; set; }
}
