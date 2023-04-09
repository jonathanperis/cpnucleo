namespace Cpnucleo.Application.Commands.UpdateImpedimentoTarefa;

public sealed class UpdateImpedimentoTarefaCommandHandler : IRequestHandler<UpdateImpedimentoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateImpedimentoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(UpdateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        var impedimentoTarefa = await _context.ImpedimentoTarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (impedimentoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        impedimentoTarefa = Domain.Entities.ImpedimentoTarefa.Update(impedimentoTarefa, request.Descricao, request.IdTarefa, request.IdImpedimento);
        _context.ImpedimentoTarefas.Update(impedimentoTarefa);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
