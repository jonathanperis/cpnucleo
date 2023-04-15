namespace Cpnucleo.Application.Queries.ListRecurso;

public sealed class ListRecursoQueryHandler : IRequestHandler<ListRecursoQuery, ListRecursoViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListRecursoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListRecursoViewModel> Handle(ListRecursoQuery request, CancellationToken cancellationToken)
    {
        var recursos = await _context.Recursos
            .AsNoTracking()
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (recursos is null)
        {
            return new ListRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoViewModel { Recursos = recursos, OperationResult = OperationResult.Success };
    }
}