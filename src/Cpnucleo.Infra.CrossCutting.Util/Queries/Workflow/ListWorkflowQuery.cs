namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

public class ListWorkflowQuery : BaseQuery, IRequest<IEnumerable<WorkflowViewModel>>
{
    public bool GetDependencies { get; set; }
}