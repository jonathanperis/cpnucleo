namespace Cpnucleo.Application.Commands.UpdateWorkflow;

public sealed class UpdateWorkflowCommandHandler : IRequestHandler<UpdateWorkflowCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateWorkflowCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(UpdateWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = await _context.Workflows
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (workflow is null)
        {
            return OperationResult.NotFound;
        }

        workflow = Workflow.Update(workflow, request.Nome, request.Ordem);
        _context.Workflows.Update(workflow);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
