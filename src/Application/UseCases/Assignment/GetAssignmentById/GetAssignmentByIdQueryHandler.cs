namespace Application.UseCases.Assignment.GetAssignmentById;

public sealed class GetAssignmentByIdQueryHandler(IAssignmentRepository assignmentRepository) : IRequestHandler<GetAssignmentByIdQuery, GetAssignmentByIdQueryViewModel>
{
    public async ValueTask<GetAssignmentByIdQueryViewModel> Handle(GetAssignmentByIdQuery request, CancellationToken cancellationToken)
    {
        var assignment = await assignmentRepository.GetAssignmentById(request.Id);

        var operationResult = assignment is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetAssignmentByIdQueryViewModel(operationResult, assignment);
    }
}
