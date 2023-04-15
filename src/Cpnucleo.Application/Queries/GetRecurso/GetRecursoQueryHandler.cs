namespace Cpnucleo.Application.Queries.GetRecurso;

public sealed class GetRecursoQueryHandler : IRequestHandler<GetRecursoQuery, GetRecursoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRecursoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<GetRecursoViewModel> Handle(GetRecursoQuery request, CancellationToken cancellationToken)
    {
        var recurso = await _context.Recursos
            .Where(x => x.Id == request.Id && x.Ativo)
            .Select(x => new RecursoDTO
            {
                Id = x.Id,
                Nome = x.Nome,
                Login = x.Login,
                DataInclusao = x.DataInclusao,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (recurso is null)
        {
            return new GetRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetRecursoViewModel { Recurso = recurso, OperationResult = OperationResult.Success };
    }
}