namespace Application.UseCases.UserAssignment.UpdateUserAssignment;

// EF Core
public sealed class UpdateUserAssignmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateUserAssignmentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateUserAssignmentCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.UserAssignments is not null)
        {
            var userAssignment = await dbContext.UserAssignments
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (userAssignment is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.UserAssignment.Update(userAssignment, request.UserId, request.AssignmentId);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
