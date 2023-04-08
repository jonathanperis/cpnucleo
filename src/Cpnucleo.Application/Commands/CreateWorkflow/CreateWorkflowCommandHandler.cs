using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Commands.CreateWorkflow;

namespace Cpnucleo.Application.Commands.CreateWorkflow;

public sealed class CreateWorkflowCommandHandler : IRequestHandler<CreateWorkflowCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateWorkflowCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = Domain.Entities.Workflow.Create(request.Nome, request.Ordem);
        _context.Workflows.Add(workflow);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
