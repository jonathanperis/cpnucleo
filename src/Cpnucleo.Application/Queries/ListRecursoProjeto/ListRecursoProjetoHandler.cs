using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Queries.ListRecursoProjeto;

namespace Cpnucleo.Application.Queries.ListRecursoProjeto;

public sealed class ListRecursoProjetoQueryHandler : IRequestHandler<ListRecursoProjetoQuery, ListRecursoProjetoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListRecursoProjetoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListRecursoProjetoViewModel> Handle(ListRecursoProjetoQuery request, CancellationToken cancellationToken)
    {
        var recursoProjetos = await _context.RecursoProjetos
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<RecursoProjetoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (recursoProjetos is null)
        {
            return new ListRecursoProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoProjetoViewModel { RecursoProjetos = recursoProjetos, OperationResult = OperationResult.Success };
    }
}