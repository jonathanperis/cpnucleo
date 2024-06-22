namespace Application.UseCases.AssignmentType.GetAssignmentTypeById;

public sealed class GetAssignmentTypeByIdQueryHandler(IAssignmentTypeRepository assignmentTypeRepository) : IRequestHandler<GetAssignmentTypeByIdQuery, GetAssignmentTypeByIdQueryViewModel>
{
    public async ValueTask<GetAssignmentTypeByIdQueryViewModel> Handle(GetAssignmentTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var assignmentType = await assignmentTypeRepository.GetAssignmentTypeById(request.Id);

        var operationResult = assignmentType is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetAssignmentTypeByIdQueryViewModel(operationResult, assignmentType);
    }
}
