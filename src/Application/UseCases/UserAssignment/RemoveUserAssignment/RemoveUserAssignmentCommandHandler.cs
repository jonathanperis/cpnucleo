namespace Application.UseCases.UserAssignment.RemoveUserAssignment;

// EF Core
public sealed class RemoveUserAssignmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RemoveUserAssignmentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveUserAssignmentCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.UserAssignments is not null)
        {
            var userAssignment = await dbContext.UserAssignments
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (userAssignment is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.UserAssignment.Remove(userAssignment);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
