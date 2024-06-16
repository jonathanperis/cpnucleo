namespace Application.UseCases.Workflow.RemoveWorkflow;

public sealed class RemoveWorkflowCommandHandler : IRequestHandler<RemoveWorkflowCommand, OperationResult>
{
    private readonly IApplicationDbContext _dbContext;

    public RemoveWorkflowCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<OperationResult> Handle(RemoveWorkflowCommand request, CancellationToken cancellationToken)
    {
        if (_dbContext.Workflows is not null)
        {
            var workflow = await _dbContext.Workflows
                    .FirstOrDefaultAsync(w => w.Id == request.Id && w.Active, cancellationToken);

            if (workflow == null)
            {
                return OperationResult.NotFound;
            }

            workflow = Domain.Entities.Workflow.Remove(workflow);
        }

        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
