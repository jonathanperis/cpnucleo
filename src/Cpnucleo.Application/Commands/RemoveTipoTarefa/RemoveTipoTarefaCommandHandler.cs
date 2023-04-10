namespace Cpnucleo.Application.Commands.RemoveTipoTarefa;

public sealed class RemoveTipoTarefaCommandHandler : IRequestHandler<RemoveTipoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveTipoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(RemoveTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        var tipoTarefa = await _context.TipoTarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (tipoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        tipoTarefa = TipoTarefa.Remove(tipoTarefa);
        _context.TipoTarefas.Update(tipoTarefa); //JONATHAN - Soft Delete.

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
