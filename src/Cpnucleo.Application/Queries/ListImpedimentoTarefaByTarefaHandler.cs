namespace Cpnucleo.Application.Queries;

public sealed class ListImpedimentoTarefaByTarefaQueryHandler : IRequestHandler<ListImpedimentoTarefaByTarefaQuery, ListImpedimentoTarefaByTarefaViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListImpedimentoTarefaByTarefaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListImpedimentoTarefaByTarefaViewModel> Handle(ListImpedimentoTarefaByTarefaQuery request, CancellationToken cancellationToken)
    {
        var impedimentoTarefas = await _context.ImpedimentoTarefas
            .AsNoTracking()
            .Include(x => x.Tarefa)
            .Where(x => x.IdTarefa == request.IdTarefa && x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (impedimentoTarefas is null)
        {
            return new ListImpedimentoTarefaByTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListImpedimentoTarefaByTarefaViewModel { ImpedimentoTarefas = impedimentoTarefas, OperationResult = OperationResult.Success };
    }
}