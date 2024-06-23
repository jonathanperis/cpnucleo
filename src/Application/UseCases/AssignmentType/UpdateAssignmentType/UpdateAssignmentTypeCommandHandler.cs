namespace Application.UseCases.AssignmentType.UpdateAssignmentType;

public sealed class UpdateAssignmentTypeCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateAssignmentTypeCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateAssignmentTypeCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.AssignmentTypes is not null)
        {
            var assignmentType = await dbContext.AssignmentTypes
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (assignmentType is null)
            {
                return OperationResult.NotFound;
            }

            assignmentType = Domain.Entities.AssignmentType.Update(assignmentType, request.Name);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
