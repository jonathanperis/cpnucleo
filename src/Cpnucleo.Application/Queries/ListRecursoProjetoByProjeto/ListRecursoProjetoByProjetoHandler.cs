namespace Cpnucleo.Application.Queries.ListRecursoProjetoByProjeto;

public sealed class ListRecursoProjetoByProjetoQueryHandler : IRequestHandler<ListRecursoProjetoByProjetoQuery, ListRecursoProjetoByProjetoViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListRecursoProjetoByProjetoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListRecursoProjetoByProjetoViewModel> Handle(ListRecursoProjetoByProjetoQuery request, CancellationToken cancellationToken)
    {
        var recursoProjetos = await _context.RecursoProjetos
            .AsNoTracking()
            .Include(x => x.Recurso)
            .Include(x => x.Projeto)
            .Where(x => x.IdProjeto == request.IdProjeto && x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (recursoProjetos is null)
        {
            return new ListRecursoProjetoByProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoProjetoByProjetoViewModel { RecursoProjetos = recursoProjetos, OperationResult = OperationResult.Success };
    }
}