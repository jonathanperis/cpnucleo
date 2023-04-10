namespace Cpnucleo.Application.Commands.CreateRecursoTarefa;

public sealed class CreateRecursoTarefaCommandHandler : IRequestHandler<CreateRecursoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateRecursoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(CreateRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        var recursoTarefa = RecursoTarefa.Create(request.IdTarefa, request.IdRecurso);
        _context.RecursoTarefas.Add(recursoTarefa);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
