namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

public class ListWorkflowViewModel
{
    public IEnumerable<WorkflowDTO> Workflows { get; set; }
    public OperationResult OperationResult { get; set; }
}
