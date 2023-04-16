namespace Cpnucleo.Application.Queries;

public sealed class ListApontamentoQueryHandler : IRequestHandler<ListApontamentoQuery, ListApontamentoViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListApontamentoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListApontamentoViewModel> Handle(ListApontamentoQuery request, CancellationToken cancellationToken)
    {
        var apontamentos = await _context.Apontamentos
            .AsNoTracking()
            .Include(x => x.Tarefa)
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (apontamentos is null)
        {
            return new ListApontamentoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListApontamentoViewModel { Apontamentos = apontamentos, OperationResult = OperationResult.Success };
    }
}