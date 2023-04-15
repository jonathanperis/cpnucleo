namespace Cpnucleo.Application.Queries.GetProjeto;

public sealed class GetProjetoQueryHandler : IRequestHandler<GetProjetoQuery, GetProjetoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjetoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<GetProjetoViewModel> Handle(GetProjetoQuery request, CancellationToken cancellationToken)
    {
        var projeto = await _context.Projetos
            .Where(x => x.Id == request.Id && x.Ativo)
            .Include(x => x.Sistema)
            .ProjectTo<ProjetoDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (projeto is null)
        {
            return new GetProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetProjetoViewModel { Projeto = projeto, OperationResult = OperationResult.Success };
    }
}