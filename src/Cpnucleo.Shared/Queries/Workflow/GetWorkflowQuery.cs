namespace Cpnucleo.Shared.Queries.Workflow;

public sealed record GetWorkflowQuery(Guid Id) : BaseQuery, IRequest<GetWorkflowViewModel>;