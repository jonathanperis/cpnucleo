namespace Cpnucleo.Application.Commands;

public sealed class UpdateImpedimentoTarefaCommandHandler : IRequestHandler<UpdateImpedimentoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateImpedimentoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(UpdateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        var impedimentoTarefa = await _context.ImpedimentoTarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (impedimentoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        impedimentoTarefa = ImpedimentoTarefa.Update(impedimentoTarefa, request.Descricao, request.IdTarefa, request.IdImpedimento);
        _context.ImpedimentoTarefas.Update(impedimentoTarefa);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
