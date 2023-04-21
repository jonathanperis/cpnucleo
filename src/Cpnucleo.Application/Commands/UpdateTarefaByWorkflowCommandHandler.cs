namespace Cpnucleo.Application.Commands;

public sealed class UpdateTarefaByWorkflowCommandHandler : IRequestHandler<UpdateTarefaByWorkflowCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateTarefaByWorkflowCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(UpdateTarefaByWorkflowCommand request, CancellationToken cancellationToken)
    {
        var tarefa = await _context.Tarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (tarefa is null)
        {
            return OperationResult.NotFound;
        }

        tarefa = Tarefa.Update(tarefa, request.IdWorkflow);
        _context.Tarefas.Update(tarefa);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
