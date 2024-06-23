namespace Application.UseCases.Assignment.UpdateAssignment;

public sealed class UpdateAssignmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateAssignmentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Assignments is not null)
        {
            var assignment = await dbContext.Assignments
                .FirstOrDefaultAsync(a => a.Id == request.Id && a.Active, cancellationToken);

            if (assignment is null)
            {
                return OperationResult.NotFound;
            }

            assignment = Domain.Entities.Assignment.Update(assignment, request.Name, request.Description, request.StartDate, request.EndDate, request.AmountHours, request.ProjectId, request.WorkflowId, request.UserId, request.AssignmentTypeId);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
