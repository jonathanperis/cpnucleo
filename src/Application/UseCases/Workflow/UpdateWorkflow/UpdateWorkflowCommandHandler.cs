namespace Application.UseCases.Workflow.UpdateWorkflow;

// EF Core
public sealed class UpdateWorkflowCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateWorkflowCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateWorkflowCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Workflows is not null)
        {
            var workflow = await dbContext.Workflows
                    .FirstOrDefaultAsync(w => w.Id == request.Id && w.Active, cancellationToken);

            if (workflow is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.Workflow.Update(workflow, request.Name, request.Order);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
