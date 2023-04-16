namespace Cpnucleo.Application.Commands;

public sealed class RemoveTipoTarefaCommandHandler : IRequestHandler<RemoveTipoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveTipoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(RemoveTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        var tipoTarefa = await _context.TipoTarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (tipoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        tipoTarefa = TipoTarefa.Remove(tipoTarefa);
        _context.TipoTarefas.Update(tipoTarefa); //JONATHAN - Soft Delete.

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
