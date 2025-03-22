namespace Application.UseCases.Workflow.GetWorkflowById;

public sealed record GetWorkflowByIdQueryViewModel(OperationResult OperationResult, WorkflowDto? Workflow);
