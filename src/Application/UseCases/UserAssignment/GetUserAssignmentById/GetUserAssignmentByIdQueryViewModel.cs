namespace Application.UseCases.UserAssignment.GetUserAssignmentById;

public sealed record GetUserAssignmentByIdQueryViewModel(OperationResult OperationResult, UserAssignmentDto? UserAssignment);
