namespace Cpnucleo.Application.Queries;

public sealed class ListTipoTarefaQueryHandler : IRequestHandler<ListTipoTarefaQuery, ListTipoTarefaViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListTipoTarefaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListTipoTarefaViewModel> Handle(ListTipoTarefaQuery request, CancellationToken cancellationToken)
    {
        var tipoTarefas = await _context.TipoTarefas
            .AsNoTracking()
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (tipoTarefas is null)
        {
            return new ListTipoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListTipoTarefaViewModel { TipoTarefas = tipoTarefas, OperationResult = OperationResult.Success };
    }
}