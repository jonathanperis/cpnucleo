namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Workflow;

public record ListWorkflowViewModel : BaseQuery
{
    public IEnumerable<WorkflowDTO> Workflows { get; set; }
    public OperationResult OperationResult { get; set; }
}
