namespace Cpnucleo.Application.Commands.RemoveTarefa;

public sealed class RemoveTarefaCommandHandler : IRequestHandler<RemoveTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(RemoveTarefaCommand request, CancellationToken cancellationToken)
    {
        var tarefa = await _context.Tarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (tarefa is null)
        {
            return OperationResult.NotFound;
        }

        tarefa = Domain.Entities.Tarefa.Remove(tarefa);
        _context.Tarefas.Update(tarefa); //JONATHAN - Soft Delete.

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
