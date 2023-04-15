namespace Cpnucleo.Application.Queries.ListImpedimento;

public sealed class ListImpedimentoQueryHandler : IRequestHandler<ListImpedimentoQuery, ListImpedimentoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListImpedimentoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<ListImpedimentoViewModel> Handle(ListImpedimentoQuery request, CancellationToken cancellationToken)
    {
        var impedimentos = await _context.Impedimentos
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<ImpedimentoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (impedimentos is null)
        {
            return new ListImpedimentoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListImpedimentoViewModel { Impedimentos = impedimentos, OperationResult = OperationResult.Success };
    }
}