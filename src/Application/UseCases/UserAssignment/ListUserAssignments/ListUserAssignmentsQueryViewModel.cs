namespace Application.UseCases.UserAssignment.ListUserAssignments;

public sealed record ListUserAssignmentsQueryViewModel(OperationResult OperationResult, List<UserAssignmentDto> UserAssignments);
