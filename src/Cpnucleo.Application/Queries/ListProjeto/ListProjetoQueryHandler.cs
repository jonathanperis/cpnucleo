namespace Cpnucleo.Application.Queries.ListProjeto;

public sealed class ListProjetoQueryHandler : IRequestHandler<ListProjetoQuery, ListProjetoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListProjetoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListProjetoViewModel> Handle(ListProjetoQuery request, CancellationToken cancellationToken)
    {
        var projetos = await _context.Projetos
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<ProjetoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (projetos is null)
        {
            return new ListProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListProjetoViewModel { Projetos = projetos, OperationResult = OperationResult.Success };
    }
}