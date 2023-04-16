namespace Cpnucleo.Application.Queries;

public sealed class ListImpedimentoTarefaQueryHandler : IRequestHandler<ListImpedimentoTarefaQuery, ListImpedimentoTarefaViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListImpedimentoTarefaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListImpedimentoTarefaViewModel> Handle(ListImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        var impedimentoTarefas = await _context.ImpedimentoTarefas
            .AsNoTracking()
            .Include(x => x.Tarefa)
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (impedimentoTarefas is null)
        {
            return new ListImpedimentoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListImpedimentoTarefaViewModel { ImpedimentoTarefas = impedimentoTarefas, OperationResult = OperationResult.Success };
    }
}