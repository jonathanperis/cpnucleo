namespace Application.UseCases.AssignmentImpediment.UpdateAssignmentImpediment;

// EF Core
public sealed class UpdateAssignmentImpedimentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateAssignmentImpedimentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateAssignmentImpedimentCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.AssignmentImpediments is not null)
        {
            var assignmentImpediment = await dbContext.AssignmentImpediments
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (assignmentImpediment is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.AssignmentImpediment.Update(assignmentImpediment, request.Description, request.AssignmentId, request.ImpedimentId);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
