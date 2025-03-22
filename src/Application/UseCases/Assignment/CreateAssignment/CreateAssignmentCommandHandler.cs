namespace Application.UseCases.Assignment.CreateAssignment;

// EF Core
public sealed class CreateAssignmentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateAssignmentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
    {
        var assignment = Domain.Entities.Assignment.Create(request.Name, request.Description, request.StartDate, request.EndDate, request.AmountHours, request.ProjectId, request.WorkflowId, request.UserId, request.AssignmentTypeId, request.Id);

        if (dbContext.Assignments is not null)
            await dbContext.Assignments.AddAsync(assignment, cancellationToken);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
