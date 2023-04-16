namespace Cpnucleo.Application.Commands;

public sealed class RemoveWorkflowCommandHandler : IRequestHandler<RemoveWorkflowCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveWorkflowCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(RemoveWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = await _context.Workflows
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (workflow is null)
        {
            return OperationResult.NotFound;
        }

        workflow = Workflow.Remove(workflow);
        _context.Workflows.Update(workflow); //JONATHAN - Soft Delete.

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
