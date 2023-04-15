namespace Cpnucleo.Application.Commands.UpdateTipoTarefa;

public sealed class UpdateTipoTarefaCommandHandler : IRequestHandler<UpdateTipoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateTipoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(UpdateTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        var tipoTarefa = await _context.TipoTarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (tipoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        tipoTarefa = TipoTarefa.Update(tipoTarefa, request.Nome, request.Image);
        _context.TipoTarefas.Update(tipoTarefa);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
