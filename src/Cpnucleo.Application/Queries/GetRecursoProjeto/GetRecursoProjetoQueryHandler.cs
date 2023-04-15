namespace Cpnucleo.Application.Queries.GetRecursoProjeto;

public sealed class GetRecursoProjetoQueryHandler : IRequestHandler<GetRecursoProjetoQuery, GetRecursoProjetoViewModel>
{
    private readonly IApplicationDbContext _context;

    public GetRecursoProjetoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<GetRecursoProjetoViewModel> Handle(GetRecursoProjetoQuery request, CancellationToken cancellationToken)
    {
        var recursoProjeto = await _context.RecursoProjetos
            .AsNoTracking()
            .Include(x => x.Recurso)
            .Include(x => x.Projeto)
            .Where(x => x.Id == request.Id && x.Ativo)
            .Select(x => x.MapToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (recursoProjeto is null)
        {
            return new GetRecursoProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetRecursoProjetoViewModel { RecursoProjeto = recursoProjeto, OperationResult = OperationResult.Success };
    }
}