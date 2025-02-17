namespace Application.UseCases.Assignment.ListAssignment;

public sealed class ListAssignmentQueryHandler(IAssignmentRepository assignmentRepository) : IRequestHandler<ListAssignmentQuery, ListAssignmentQueryViewModel>
{
    public async ValueTask<ListAssignmentQueryViewModel> Handle(ListAssignmentQuery request, CancellationToken cancellationToken)
    {
        var assignments = await assignmentRepository.ListAssignments();

        var operationResult = assignments is not null ? OperationResult.Success : OperationResult.NotFound;
        var assignmentsList = assignments ?? [];  // Return an empty list if no assignments are found

        var result = assignmentsList.Select(assignment => (AssignmentDto)assignment).ToList();

        return new ListAssignmentQueryViewModel(operationResult, result);
    }
}
