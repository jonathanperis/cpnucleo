namespace Cpnucleo.Shared.Queries.Workflow;

public record GetWorkflowQuery(Guid Id) : BaseQuery, IRequest<GetWorkflowViewModel>;