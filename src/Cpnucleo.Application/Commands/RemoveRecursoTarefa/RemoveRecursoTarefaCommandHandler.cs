namespace Cpnucleo.Application.Commands.RemoveRecursoTarefa;

public sealed class RemoveRecursoTarefaCommandHandler : IRequestHandler<RemoveRecursoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveRecursoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(RemoveRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        var recursoTarefa = await _context.RecursoTarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (recursoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        recursoTarefa = RecursoTarefa.Remove(recursoTarefa);
        _context.RecursoTarefas.Update(recursoTarefa); //JONATHAN - Soft Delete.

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
