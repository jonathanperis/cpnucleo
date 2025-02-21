namespace Application.UseCases.Workflow.ListWorkflow;

public sealed record ListWorkflowQuery(PaginationParams Pagination) : BaseQuery, IRequest<ListWorkflowQueryViewModel>;
