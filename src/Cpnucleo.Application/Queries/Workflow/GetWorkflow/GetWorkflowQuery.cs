namespace Cpnucleo.Application.Queries.Workflow.GetWorkflow;

public class GetWorkflowQuery : IRequest<GetWorkflowViewModel>
{
    public Guid Id { get; }

    public GetWorkflowQuery(Guid id)
    {
        Id = id;
    }
}
