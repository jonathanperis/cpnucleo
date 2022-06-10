namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Workflow;

public class ListWorkflowQuery : BaseQuery, IRequest<ListWorkflowViewModel>
{
    public bool GetDependencies { get; set; }
}
