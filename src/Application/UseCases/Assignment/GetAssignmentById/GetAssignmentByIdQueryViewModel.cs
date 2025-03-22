namespace Application.UseCases.Assignment.GetAssignmentById;

public sealed record GetAssignmentByIdQueryViewModel(OperationResult OperationResult, AssignmentDto? Assignment);
