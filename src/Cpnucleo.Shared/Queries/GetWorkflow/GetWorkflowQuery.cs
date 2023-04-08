namespace Cpnucleo.Shared.Queries.GetWorkflow;

public sealed record GetWorkflowQuery(Guid Id) : BaseQuery, IRequest<GetWorkflowViewModel>;