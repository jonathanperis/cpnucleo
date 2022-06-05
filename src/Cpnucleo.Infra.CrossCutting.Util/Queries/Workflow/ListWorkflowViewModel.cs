namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

public class ListWorkflowViewModel : BaseQuery
{
    public IEnumerable<WorkflowDTO> Workflows { get; set; }
    public OperationResult OperationResult { get; set; }
}
