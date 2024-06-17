namespace Application.UseCases.Workflow.UpdateWorkflow;

public sealed class UpdateWorkflowCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateWorkflowCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateWorkflowCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Workflows is not null)
        {
            var workflow = await dbContext.Workflows
                    .FirstOrDefaultAsync(w => w.Id == request.Id && w.Active, cancellationToken);

            if (workflow == null)
            {
                return OperationResult.NotFound;
            }

            workflow = Domain.Entities.Workflow.Update(workflow, request.Name, request.Order);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
