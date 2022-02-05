namespace Cpnucleo.Application.Queries.Workflow.ListWorkflow;

public class ListWorkflowQuery : IRequest<ListWorkflowViewModel>
{
    public bool GetDependencies { get; set; }
}
