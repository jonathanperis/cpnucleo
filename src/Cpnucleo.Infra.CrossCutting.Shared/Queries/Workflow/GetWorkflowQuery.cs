namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Workflow;

public class GetWorkflowQuery : BaseQuery, IRequest<GetWorkflowViewModel>
{
    public Guid Id { get; set; }
}
