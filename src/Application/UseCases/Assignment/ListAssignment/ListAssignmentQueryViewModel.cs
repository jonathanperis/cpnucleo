namespace Application.UseCases.Assignment.ListAssignment;

public sealed record ListAssignmentQueryViewModel(OperationResult OperationResult, List<AssignmentDto?>? Assignments);
