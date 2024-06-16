namespace Application.UseCases.Workflow.UpdateWorkflow;

public sealed class UpdateWorkflowCommandHandler : IRequestHandler<UpdateWorkflowCommand, OperationResult>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateWorkflowCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<OperationResult> Handle(UpdateWorkflowCommand request, CancellationToken cancellationToken)
    {
        if (_dbContext.Workflows is not null)
        {
            var workflow = await _dbContext.Workflows
                    .FirstOrDefaultAsync(w => w.Id == request.Id && w.Active, cancellationToken);

            if (workflow == null)
            {
                return OperationResult.NotFound;
            }

            workflow = Domain.Entities.Workflow.Update(workflow, request.Name, request.Order);
        }

        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
