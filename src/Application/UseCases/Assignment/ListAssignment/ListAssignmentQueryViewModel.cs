namespace Application.UseCases.Assignment.ListAssignment;

public sealed record ListAssignmentQueryViewModel(OperationResult OperationResult, PaginatedResult<AssignmentDto?> Result);
