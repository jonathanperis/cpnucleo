namespace Application.UseCases.Workflow.RemoveWorkflow;

public sealed class RemoveWorkflowCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RemoveWorkflowCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveWorkflowCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Workflows is not null)
        {
            var workflow = await dbContext.Workflows
                    .FirstOrDefaultAsync(w => w.Id == request.Id && w.Active, cancellationToken);

            if (workflow is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.Workflow.Remove(workflow);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
