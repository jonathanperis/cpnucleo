namespace Cpnucleo.Application.Commands;

public sealed class CreateRecursoTarefaCommandHandler : IRequestHandler<CreateRecursoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateRecursoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(CreateRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        var recursoTarefa = RecursoTarefa.Create(request.IdTarefa, request.IdRecurso);
        _context.RecursoTarefas.Add(recursoTarefa);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
