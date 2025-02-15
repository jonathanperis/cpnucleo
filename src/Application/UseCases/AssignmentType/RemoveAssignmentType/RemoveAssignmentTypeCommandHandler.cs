namespace Application.UseCases.AssignmentType.RemoveAssignmentType;

public sealed class RemoveAssignmentTypeCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RemoveAssignmentTypeCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveAssignmentTypeCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.AssignmentTypes is not null)
        {
            var assignmentType = await dbContext.AssignmentTypes
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (assignmentType is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.AssignmentType.Remove(assignmentType);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
