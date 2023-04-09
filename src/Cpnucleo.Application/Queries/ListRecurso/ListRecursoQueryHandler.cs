namespace Cpnucleo.Application.Queries.ListRecurso;

public sealed class ListRecursoQueryHandler : IRequestHandler<ListRecursoQuery, ListRecursoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListRecursoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListRecursoViewModel> Handle(ListRecursoQuery request, CancellationToken cancellationToken)
    {
        var recursos = await _context.Recursos
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => new RecursoDTO
            {
                Id = x.Id,
                Nome = x.Nome,
                Login = x.Login,
                DataInclusao = x.DataInclusao,
            })
            .ToListAsync(cancellationToken);

        if (recursos is null)
        {
            return new ListRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoViewModel { Recursos = recursos, OperationResult = OperationResult.Success };
    }
}