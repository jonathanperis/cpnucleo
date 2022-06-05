namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

public class GetWorkflowQuery : BaseQuery, IRequest<GetWorkflowViewModel>
{
    public Guid Id { get; set; }
}
