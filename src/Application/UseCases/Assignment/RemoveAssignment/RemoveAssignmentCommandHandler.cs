namespace Application.UseCases.Assignment.RemoveAssignment;

public sealed class RemoveAssignmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RemoveAssignmentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveAssignmentCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Assignments is not null)
        {
            var assignment = await dbContext.Assignments
                .FirstOrDefaultAsync(a => a.Id == request.Id && a.Active, cancellationToken);

            if (assignment is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.Assignment.Remove(assignment);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
