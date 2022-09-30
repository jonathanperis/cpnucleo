namespace Cpnucleo.Shared.Queries.Workflow;

public sealed record ListWorkflowQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListWorkflowViewModel>;