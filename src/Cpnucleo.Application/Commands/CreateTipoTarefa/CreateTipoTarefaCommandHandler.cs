namespace Cpnucleo.Application.Commands.CreateTipoTarefa;

public sealed class CreateTipoTarefaCommandHandler : IRequestHandler<CreateTipoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateTipoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(CreateTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        var tipoTarefa = TipoTarefa.Create(request.Nome, request.Image);
        _context.TipoTarefas.Add(tipoTarefa);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
