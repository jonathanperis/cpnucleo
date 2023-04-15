namespace Cpnucleo.Application.Queries.ListApontamento;

public sealed class ListApontamentoQueryHandler : IRequestHandler<ListApontamentoQuery, ListApontamentoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListApontamentoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<ListApontamentoViewModel> Handle(ListApontamentoQuery request, CancellationToken cancellationToken)
    {
        var apontamentos = await _context.Apontamentos
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<ApontamentoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (apontamentos is null)
        {
            return new ListApontamentoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListApontamentoViewModel { Apontamentos = apontamentos, OperationResult = OperationResult.Success };
    }
}