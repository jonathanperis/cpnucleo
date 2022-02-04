namespace Cpnucleo.Application.Queries.Workflow.ListWorkflow;

public class ListWorkflowViewModel
{
    public IEnumerable<WorkflowDTO> Workflows { get; set; }
    public OperationResult OperationResult { get; set; }
}
