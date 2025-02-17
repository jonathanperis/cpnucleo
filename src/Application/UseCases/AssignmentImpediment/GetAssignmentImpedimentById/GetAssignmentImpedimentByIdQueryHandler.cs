namespace Application.UseCases.AssignmentImpediment.GetAssignmentImpedimentById;

public sealed class GetAssignmentImpedimentByIdQueryHandler(IAssignmentImpedimentRepository assignmentImpedimentRepository) : IRequestHandler<GetAssignmentImpedimentByIdQuery, GetAssignmentImpedimentByIdQueryViewModel>
{
    public async ValueTask<GetAssignmentImpedimentByIdQueryViewModel> Handle(GetAssignmentImpedimentByIdQuery request, CancellationToken cancellationToken)
    {
        var assignmentImpediment = await assignmentImpedimentRepository.GetAssignmentImpedimentById(request.Id);

        var operationResult = assignmentImpediment is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetAssignmentImpedimentByIdQueryViewModel(operationResult, assignmentImpediment?.MapToDto());
    }
}
