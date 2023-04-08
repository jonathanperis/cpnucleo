using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Queries.GetRecurso;

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

    public async Task<GetRecursoViewModel> Handle(GetRecursoQuery request, CancellationToken cancellationToken)
    {
        var recurso = await _context.Recursos
            .Where(x => x.Id == request.Id && x.Ativo)
            .ProjectTo<RecursoDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (recurso is null)
        {
            return new GetRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        recurso.Senha = null;

        return new GetRecursoViewModel { Recurso = recurso, OperationResult = OperationResult.Success };
    }
}