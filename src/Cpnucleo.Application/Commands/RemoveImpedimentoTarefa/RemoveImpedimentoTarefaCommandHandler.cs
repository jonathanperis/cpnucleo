namespace Cpnucleo.Application.Commands.RemoveImpedimentoTarefa;

public sealed class RemoveImpedimentoTarefaCommandHandler : IRequestHandler<RemoveImpedimentoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveImpedimentoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(RemoveImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        var impedimentoTarefa = await _context.ImpedimentoTarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (impedimentoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        impedimentoTarefa = Domain.Entities.ImpedimentoTarefa.Remove(impedimentoTarefa);
        _context.ImpedimentoTarefas.Update(impedimentoTarefa); //JONATHAN - Soft Delete.

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
