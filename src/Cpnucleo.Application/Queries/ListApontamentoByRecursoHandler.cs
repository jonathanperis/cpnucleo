namespace Cpnucleo.Application.Queries;

public sealed class ListApontamentoByRecursoQueryHandler : IRequestHandler<ListApontamentoByRecursoQuery, ListApontamentoByRecursoViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListApontamentoByRecursoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListApontamentoByRecursoViewModel> Handle(ListApontamentoByRecursoQuery request, CancellationToken cancellationToken)
    {
        var apontamentos = await _context.Apontamentos
            .AsNoTracking()
            .Include(x => x.Tarefa)
            .Where(x => x.IdRecurso == request.IdRecurso && x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (apontamentos is null)
        {
            return new ListApontamentoByRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListApontamentoByRecursoViewModel { Apontamentos = apontamentos, OperationResult = OperationResult.Success };
    }
}