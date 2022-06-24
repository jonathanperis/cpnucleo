namespace Cpnucleo.Shared.Queries.Workflow;

public record ListWorkflowQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListWorkflowViewModel>;