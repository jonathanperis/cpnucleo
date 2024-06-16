namespace Application.UseCases.Workflow.CreateWorkflow;

public sealed class CreateWorkflowCommandHandler : IRequestHandler<CreateWorkflowCommand, OperationResult>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateWorkflowCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<OperationResult> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = Domain.Entities.Workflow.Create(request.Name, request.Order, request.Id);

        if (_dbContext.Workflows is not null)
            await _dbContext.Workflows.AddAsync(workflow, cancellationToken);

        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
