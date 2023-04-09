namespace Cpnucleo.Application.Queries.GetApontamento;

public sealed class GetApontamentoQueryHandler : IRequestHandler<GetApontamentoQuery, GetApontamentoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetApontamentoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetApontamentoViewModel> Handle(GetApontamentoQuery request, CancellationToken cancellationToken)
    {
        var apontamento = await _context.Apontamentos
            .Where(x => x.Id == request.Id && x.Ativo)
            .ProjectTo<ApontamentoDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (apontamento is null)
        {
            return new GetApontamentoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetApontamentoViewModel { Apontamento = apontamento, OperationResult = OperationResult.Success };
    }
}