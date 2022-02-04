namespace Cpnucleo.Application.Queries.Workflow.ListWorkflow;

public class ListWorkflowQuery : IRequest<ListWorkflowViewModel>
{
    public bool GetDependencies { get; }

    public ListWorkflowQuery(bool getDependencies)
    {
        GetDependencies = getDependencies;
    }
}
