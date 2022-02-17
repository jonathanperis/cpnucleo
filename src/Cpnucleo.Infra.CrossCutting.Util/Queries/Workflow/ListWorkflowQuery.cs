namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

public class ListWorkflowQuery : IRequest<ListWorkflowViewModel>
{
    public bool GetDependencies { get; set; }
}
