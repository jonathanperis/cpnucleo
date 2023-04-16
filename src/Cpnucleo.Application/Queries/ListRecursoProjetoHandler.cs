namespace Cpnucleo.Application.Queries;

public sealed class ListRecursoProjetoQueryHandler : IRequestHandler<ListRecursoProjetoQuery, ListRecursoProjetoViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListRecursoProjetoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListRecursoProjetoViewModel> Handle(ListRecursoProjetoQuery request, CancellationToken cancellationToken)
    {
        var recursoProjetos = await _context.RecursoProjetos
            .AsNoTracking()
            .Include(x => x.Recurso)
            .Include(x => x.Projeto)
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (recursoProjetos is null)
        {
            return new ListRecursoProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoProjetoViewModel { RecursoProjetos = recursoProjetos, OperationResult = OperationResult.Success };
    }
}