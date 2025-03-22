namespace Application.UseCases.Workflow.ListWorkflow;

public sealed record ListWorkflowQueryViewModel(OperationResult OperationResult, PaginatedResult<WorkflowDto?> Result);
