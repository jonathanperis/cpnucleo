namespace Application.UseCases.AssignmentType.ListAssignmentType;

public sealed record ListAssignmentTypeQueryViewModel(OperationResult OperationResult, List<AssignmentTypeDto?>? AssignmentTypes);
