namespace Application.UseCases.AssignmentImpediment.ListAssignmentImpediment;

public sealed class ListAssignmentImpedimentQueryHandler(IAssignmentImpedimentRepository assignmentImpedimentRepository) : IRequestHandler<ListAssignmentImpedimentQuery, ListAssignmentImpedimentQueryViewModel>
{
    public async ValueTask<ListAssignmentImpedimentQueryViewModel> Handle(ListAssignmentImpedimentQuery request, CancellationToken cancellationToken)
    {
        var assignmentImpediments = await assignmentImpedimentRepository.ListAssignmentImpediments();

        var operationResult = assignmentImpediments is not null ? OperationResult.Success : OperationResult.NotFound;
        var assignmentImpedimentsList = assignmentImpediments ?? [];  // Return an empty list if no assignment impediments are found

        return new ListAssignmentImpedimentQueryViewModel(operationResult, assignmentImpedimentsList);
    }
}
