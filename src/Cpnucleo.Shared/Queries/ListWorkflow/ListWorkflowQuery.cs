namespace Cpnucleo.Shared.Queries.ListWorkflow;

public sealed record ListWorkflowQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListWorkflowViewModel>;