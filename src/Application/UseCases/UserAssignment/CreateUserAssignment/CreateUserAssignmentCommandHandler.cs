namespace Application.UseCases.UserAssignment.CreateUserAssignment;

public sealed class CreateUserAssignmentCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<CreateUserAssignmentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateUserAssignmentCommand request, CancellationToken cancellationToken)
    {
        var userAssignment = Domain.Entities.UserAssignment.Create(request.UserId, request.AssignmentId, request.Id);

        if (dbContext.UserAssignments is not null)
            await dbContext.UserAssignments.AddAsync(userAssignment, cancellationToken);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
