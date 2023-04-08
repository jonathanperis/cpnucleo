using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Queries.GetRecursoProjeto;

namespace Cpnucleo.Application.Queries.GetRecursoProjeto;

public sealed class GetRecursoProjetoQueryHandler : IRequestHandler<GetRecursoProjetoQuery, GetRecursoProjetoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRecursoProjetoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetRecursoProjetoViewModel> Handle(GetRecursoProjetoQuery request, CancellationToken cancellationToken)
    {
        var recursoProjeto = await _context.RecursoProjetos
            .Where(x => x.Id == request.Id && x.Ativo)
            .ProjectTo<RecursoProjetoDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (recursoProjeto is null)
        {
            return new GetRecursoProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetRecursoProjetoViewModel { RecursoProjeto = recursoProjeto, OperationResult = OperationResult.Success };
    }
}