namespace Cpnucleo.Application.Queries;

public sealed class ListRecursoTarefaQueryHandler : IRequestHandler<ListRecursoTarefaQuery, ListRecursoTarefaViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListRecursoTarefaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListRecursoTarefaViewModel> Handle(ListRecursoTarefaQuery request, CancellationToken cancellationToken)
    {
        var recursoTarefas = await _context.RecursoTarefas
            .AsNoTracking()
            .Include(x => x.Recurso)
            .Include(x => x.Tarefa)
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (recursoTarefas is null)
        {
            return new ListRecursoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoTarefaViewModel { RecursoTarefas = recursoTarefas, OperationResult = OperationResult.Success };
    }
}