namespace Application.UseCases.UserAssignment.GetUserAssignmentById;

public sealed class GetUserAssignmentByIdQueryHandler(IUserAssignmentRepository userAssignmentRepository) : IRequestHandler<GetUserAssignmentByIdQuery, GetUserAssignmentByIdQueryViewModel>
{
    public async ValueTask<GetUserAssignmentByIdQueryViewModel> Handle(GetUserAssignmentByIdQuery request, CancellationToken cancellationToken)
    {
        var userAssignment = await userAssignmentRepository.GetUserAssignmentById(request.Id);

        var operationResult = userAssignment is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetUserAssignmentByIdQueryViewModel(operationResult, userAssignment?.MapToDto());
    }
}
