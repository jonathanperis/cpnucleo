using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Commands.UpdateTarefaByWorkflow;

namespace Cpnucleo.Application.Commands.UpdateTarefaByWorkflow;

public sealed class UpdateTarefaByWorkflowCommandHandler : IRequestHandler<UpdateTarefaByWorkflowCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateTarefaByWorkflowCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(UpdateTarefaByWorkflowCommand request, CancellationToken cancellationToken)
    {
        var tarefa = await _context.Tarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (tarefa is null)
        {
            return OperationResult.NotFound;
        }

        tarefa = Domain.Entities.Tarefa.UpdateWorkflow(tarefa, request.IdWorkflow);
        _context.Tarefas.Update(tarefa);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
