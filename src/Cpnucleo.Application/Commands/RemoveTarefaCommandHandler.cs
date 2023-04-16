namespace Cpnucleo.Application.Commands;

public sealed class RemoveTarefaCommandHandler : IRequestHandler<RemoveTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(RemoveTarefaCommand request, CancellationToken cancellationToken)
    {
        var tarefa = await _context.Tarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (tarefa is null)
        {
            return OperationResult.NotFound;
        }

        tarefa = Tarefa.Remove(tarefa);
        _context.Tarefas.Update(tarefa); //JONATHAN - Soft Delete.

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
