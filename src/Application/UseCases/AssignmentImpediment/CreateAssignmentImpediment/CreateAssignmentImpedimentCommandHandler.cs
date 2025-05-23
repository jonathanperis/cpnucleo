namespace Application.UseCases.AssignmentImpediment.CreateAssignmentImpediment;

// EF Core
public sealed class CreateAssignmentImpedimentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateAssignmentImpedimentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateAssignmentImpedimentCommand request, CancellationToken cancellationToken)
    {
        var assignmentImpediment = Domain.Entities.AssignmentImpediment.Create(request.Description, request.AssignmentId, request.ImpedimentId, request.Id);

        if (dbContext.AssignmentImpediments is not null)
            await dbContext.AssignmentImpediments.AddAsync(assignmentImpediment, cancellationToken);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
