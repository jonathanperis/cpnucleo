namespace Application.UseCases.AssignmentImpediment.RemoveAssignmentImpediment;

public sealed class RemoveAssignmentImpedimentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RemoveAssignmentImpedimentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveAssignmentImpedimentCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.AssignmentImpediments is not null)
        {
            var assignmentImpediment = await dbContext.AssignmentImpediments
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (assignmentImpediment is null)
            {
                return OperationResult.NotFound;
            }

            assignmentImpediment = Domain.Entities.AssignmentImpediment.Remove(assignmentImpediment);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
