namespace Application.UseCases.UserAssignment.RemoveUserAssignment;

public sealed class RemoveUserAssignmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RemoveUserAssignmentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveUserAssignmentCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.UserAssignments is not null)
        {
            var userAssignment = await dbContext.UserAssignments
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (userAssignment == null)
            {
                return OperationResult.NotFound;
            }

            userAssignment = Domain.Entities.UserAssignment.Remove(userAssignment);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
