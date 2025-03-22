namespace Application.UseCases.AssignmentType.ListAssignmentType;

public sealed record ListAssignmentTypeQueryViewModel(OperationResult OperationResult, PaginatedResult<AssignmentTypeDto?> Result);
