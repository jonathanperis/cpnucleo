using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Queries.ListRecursoProjetoByProjeto;

namespace Cpnucleo.Application.Queries.ListRecursoProjetoByProjeto;

public sealed class ListRecursoProjetoByProjetoQueryHandler : IRequestHandler<ListRecursoProjetoByProjetoQuery, ListRecursoProjetoByProjetoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListRecursoProjetoByProjetoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListRecursoProjetoByProjetoViewModel> Handle(ListRecursoProjetoByProjetoQuery request, CancellationToken cancellationToken)
    {
        var recursoProjetos = await _context.RecursoProjetos
            .Where(x => x.IdProjeto == request.IdProjeto && x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<RecursoProjetoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (recursoProjetos is null)
        {
            return new ListRecursoProjetoByProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoProjetoByProjetoViewModel { RecursoProjetos = recursoProjetos, OperationResult = OperationResult.Success };
    }
}