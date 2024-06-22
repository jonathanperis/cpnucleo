namespace Application.UseCases.UserAssignment.ListUserAssignments;

public sealed class ListUserAssignmentsQueryHandler(IUserAssignmentRepository userAssignmentRepository) : IRequestHandler<ListUserAssignmentsQuery, ListUserAssignmentsQueryViewModel>
{
    public async ValueTask<ListUserAssignmentsQueryViewModel> Handle(ListUserAssignmentsQuery request, CancellationToken cancellationToken)
    {
        var userAssignments = await userAssignmentRepository.ListUserAssignments();

        var operationResult = userAssignments is not null ? OperationResult.Success : OperationResult.NotFound;
        var userAssignmentsList = userAssignments ?? [];  // Return an empty list if no userAssignments are found

        return new ListUserAssignmentsQueryViewModel(operationResult, userAssignmentsList);
    }
}
