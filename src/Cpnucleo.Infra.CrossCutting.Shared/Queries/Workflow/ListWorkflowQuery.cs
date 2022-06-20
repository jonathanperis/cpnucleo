namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Workflow;

public record ListWorkflowQuery(bool GetDependencies = false) : BaseQuery, IRequest<ListWorkflowViewModel>;