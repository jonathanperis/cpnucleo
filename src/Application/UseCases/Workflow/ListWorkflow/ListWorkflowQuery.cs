namespace Application.UseCases.Workflow.ListWorkflow;

public sealed record ListWorkflowQuery() : BaseQuery, IRequest<ListWorkflowQueryViewModel>;
