using Microsoft.EntityFrameworkCore;

namespace Cpnucleo.Application.Queries.ListProjeto;

public sealed class ListProjetoQueryHandler : IRequestHandler<ListProjetoQuery, ListProjetoViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListProjetoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListProjetoViewModel> Handle(ListProjetoQuery request, CancellationToken cancellationToken)
    {
        var projetos = await _context.Projetos
            .AsNoTracking()
            .Include(x => x.Sistema)
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (projetos is null)
        {
            return new ListProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListProjetoViewModel { Projetos = projetos, OperationResult = OperationResult.Success };
    }
}