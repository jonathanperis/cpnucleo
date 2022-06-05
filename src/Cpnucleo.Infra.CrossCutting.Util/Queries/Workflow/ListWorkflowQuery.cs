namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

public class ListWorkflowQuery : BaseQuery, IRequest<ListWorkflowViewModel>
{
    public bool GetDependencies { get; set; }
}
