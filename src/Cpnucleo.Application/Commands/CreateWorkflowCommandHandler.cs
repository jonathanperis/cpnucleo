namespace Cpnucleo.Application.Commands;

public sealed class CreateWorkflowCommandHandler : IRequestHandler<CreateWorkflowCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateWorkflowCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = Workflow.Create(request.Nome, request.Ordem);
        _context.Workflows.Add(workflow);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
