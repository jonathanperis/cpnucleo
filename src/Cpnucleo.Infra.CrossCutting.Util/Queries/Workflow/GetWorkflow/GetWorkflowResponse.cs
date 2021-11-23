namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;

public class GetWorkflowResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public WorkflowViewModel Workflow { get; set; }
}