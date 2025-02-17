namespace Application.UseCases.Assignment.ListAssignment;

public sealed class ListAssignmentQueryHandler(IAssignmentRepository assignmentRepository) : IRequestHandler<ListAssignmentQuery, ListAssignmentQueryViewModel>
{
    public async ValueTask<ListAssignmentQueryViewModel> Handle(ListAssignmentQuery request, CancellationToken cancellationToken)
    {
        var assignments = await assignmentRepository.ListAssignments();

        var operationResult = assignments is not null ? OperationResult.Success : OperationResult.NotFound;

        var result = assignments?
                                        .Select(x => x?.MapToDto())
                                        .ToList();

        return new ListAssignmentQueryViewModel(operationResult, result);
    }
}
