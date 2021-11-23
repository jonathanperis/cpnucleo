namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;

public class ListWorkflowResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<WorkflowViewModel> Workflows { get; set; }
}