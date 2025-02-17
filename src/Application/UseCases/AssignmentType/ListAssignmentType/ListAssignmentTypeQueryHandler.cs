namespace Application.UseCases.AssignmentType.ListAssignmentType;

public sealed class ListAssignmentTypeQueryHandler(IAssignmentTypeRepository assignmentTypeRepository) : IRequestHandler<ListAssignmentTypeQuery, ListAssignmentTypeQueryViewModel>
{
    public async ValueTask<ListAssignmentTypeQueryViewModel> Handle(ListAssignmentTypeQuery request, CancellationToken cancellationToken)
    {
        var assignmentTypes = await assignmentTypeRepository.ListAssignmentTypes();

        var operationResult = assignmentTypes is not null ? OperationResult.Success : OperationResult.NotFound;

        var result = assignmentTypes?
                                                .Select(x => x?.MapToDto())
                                                .ToList();

        return new ListAssignmentTypeQueryViewModel(operationResult, result);
    }
}
