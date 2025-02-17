namespace Application.UseCases.AssignmentType.ListAssignmentType;

public sealed class ListAssignmentTypeQueryHandler(IAssignmentTypeRepository assignmentTypeRepository) : IRequestHandler<ListAssignmentTypeQuery, ListAssignmentTypeQueryViewModel>
{
    public async ValueTask<ListAssignmentTypeQueryViewModel> Handle(ListAssignmentTypeQuery request, CancellationToken cancellationToken)
    {
        var assignmentTypes = await assignmentTypeRepository.ListAssignmentTypes();

        var operationResult = assignmentTypes is not null ? OperationResult.Success : OperationResult.NotFound;
        var assignmentTypesList = assignmentTypes ?? [];  // Return an empty list if no assignmentTypes are found

        var result = assignmentTypesList.Select(assignmentType => (AssignmentTypeDto)assignmentType).ToList();

        return new ListAssignmentTypeQueryViewModel(operationResult, result);
    }
}
