namespace Cpnucleo.Application.Commands;

public sealed class CreateTarefaCommandHandler : IRequestHandler<CreateTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(CreateTarefaCommand request, CancellationToken cancellationToken)
    {
        var tarefa = Tarefa.Create(request.Nome,
                                                   request.DataInicio,
                                                   request.DataTermino,
                                                   request.QtdHoras,
                                                   request.Detalhe,
                                                   request.IdProjeto,
                                                   request.IdWorkflow,
                                                   request.IdRecurso,
                                                   request.IdTipoTarefa);
        _context.Tarefas.Add(tarefa);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
