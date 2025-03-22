namespace Application.UseCases.UserAssignment.ListUserAssignment;

public sealed record ListUserAssignmentQueryViewModel(OperationResult OperationResult, PaginatedResult<UserAssignmentDto?> Result);
