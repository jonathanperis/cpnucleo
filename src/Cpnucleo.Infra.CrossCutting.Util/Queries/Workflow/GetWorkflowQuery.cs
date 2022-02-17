namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

public class GetWorkflowQuery : IRequest<GetWorkflowViewModel>
{
    public Guid Id { get; set; }
}
