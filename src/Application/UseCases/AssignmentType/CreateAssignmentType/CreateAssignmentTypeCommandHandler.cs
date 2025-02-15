namespace Application.UseCases.AssignmentType.CreateAssignmentType;

public sealed class CreateAssignmentTypeCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<CreateAssignmentTypeCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateAssignmentTypeCommand request, CancellationToken cancellationToken)
    {
        var assignmentType = Domain.Entities.AssignmentType.Create(request.Name, request.Id);

        if (dbContext.AssignmentTypes is not null)
            await dbContext.AssignmentTypes.AddAsync(assignmentType, cancellationToken);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
