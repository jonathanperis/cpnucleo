namespace Cpnucleo.Application.Commands.UpdateRecursoTarefa;

public sealed class UpdateRecursoTarefaCommandHandler : IRequestHandler<UpdateRecursoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateRecursoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(UpdateRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        var recursoTarefa = await _context.RecursoTarefas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (recursoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        recursoTarefa = RecursoTarefa.Update(recursoTarefa, request.IdRecurso, request.IdTarefa);
        _context.RecursoTarefas.Update(recursoTarefa);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
