namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

public class GetWorkflowQuery : BaseQuery, IRequest<WorkflowViewModel>
{
    public Guid Id { get; set; }
}