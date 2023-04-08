using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Queries.ListApontamentoByRecurso;

namespace Cpnucleo.Application.Queries.ListApontamentoByRecurso;

public sealed class ListApontamentoByRecursoQueryHandler : IRequestHandler<ListApontamentoByRecursoQuery, ListApontamentoByRecursoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListApontamentoByRecursoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListApontamentoByRecursoViewModel> Handle(ListApontamentoByRecursoQuery request, CancellationToken cancellationToken)
    {
        var apontamentos = await _context.Apontamentos
            .Where(x => x.IdRecurso == request.IdRecurso && x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<ApontamentoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (apontamentos is null)
        {
            return new ListApontamentoByRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListApontamentoByRecursoViewModel { Apontamentos = apontamentos, OperationResult = OperationResult.Success };
    }
}