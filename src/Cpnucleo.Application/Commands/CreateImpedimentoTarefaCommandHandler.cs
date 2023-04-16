namespace Cpnucleo.Application.Commands;

public sealed class CreateImpedimentoTarefaCommandHandler : IRequestHandler<CreateImpedimentoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateImpedimentoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(CreateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        var impedimentoTarefa = ImpedimentoTarefa.Create(request.Descricao, request.IdTarefa, request.IdImpedimento);
        _context.ImpedimentoTarefas.Add(impedimentoTarefa);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
