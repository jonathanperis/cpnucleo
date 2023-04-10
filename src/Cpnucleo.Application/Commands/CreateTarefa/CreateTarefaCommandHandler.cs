namespace Cpnucleo.Application.Commands.CreateTarefa;

public sealed class CreateTarefaCommandHandler : IRequestHandler<CreateTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(CreateTarefaCommand request, CancellationToken cancellationToken)
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

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
