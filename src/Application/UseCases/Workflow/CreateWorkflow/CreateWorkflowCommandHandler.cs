namespace Application.UseCases.Workflow.CreateWorkflow;

// EF Core
public sealed class CreateWorkflowCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateWorkflowCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = Domain.Entities.Workflow.Create(request.Name, request.Order, request.Id);

        if (dbContext.Workflows is not null)
            await dbContext.Workflows.AddAsync(workflow, cancellationToken);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
