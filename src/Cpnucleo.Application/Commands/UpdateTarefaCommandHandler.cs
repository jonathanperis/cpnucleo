namespace Cpnucleo.Application.Commands;

public sealed class UpdateTarefaCommandHandler : IRequestHandler<UpdateTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(UpdateTarefaCommand request, CancellationToken cancellationToken)
    {
        var tarefa = await _context.Tarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (tarefa is null)
        {
            return OperationResult.NotFound;
        }

        tarefa = Tarefa.Update(tarefa,
                                                   request.Nome,
                                                   request.DataInicio,
                                                   request.DataTermino,
                                                   request.QtdHoras,
                                                   request.Detalhe,
                                                   request.IdProjeto,
                                                   request.IdWorkflow,
                                                   request.IdRecurso,
                                                   request.IdTipoTarefa);
        _context.Tarefas.Update(tarefa);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
