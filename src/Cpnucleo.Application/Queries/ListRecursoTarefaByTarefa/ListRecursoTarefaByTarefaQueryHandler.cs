namespace Cpnucleo.Application.Queries.ListRecursoTarefaByTarefa;

public sealed class ListRecursoTarefaByTarefaQueryHandler : IRequestHandler<ListRecursoTarefaByTarefaQuery, ListRecursoTarefaByTarefaViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListRecursoTarefaByTarefaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListRecursoTarefaByTarefaViewModel> Handle(ListRecursoTarefaByTarefaQuery request, CancellationToken cancellationToken)
    {
        var recursoTarefas = await _context.RecursoTarefas
            .AsNoTracking()
            .Include(x => x.Recurso)
            .Include(x => x.Tarefa)
            .Where(x => x.IdTarefa == request.IdTarefa && x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (recursoTarefas is null)
        {
            return new ListRecursoTarefaByTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoTarefaByTarefaViewModel { RecursoTarefas = recursoTarefas, OperationResult = OperationResult.Success };
    }
}