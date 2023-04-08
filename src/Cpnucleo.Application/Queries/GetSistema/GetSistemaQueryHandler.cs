using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Queries.GetSistema;

namespace Cpnucleo.Application.Queries.GetSistema;

public sealed class GetSistemaQueryHandler : IRequestHandler<GetSistemaQuery, GetSistemaViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSistemaQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetSistemaViewModel> Handle(GetSistemaQuery request, CancellationToken cancellationToken)
    {
        var sistema = await _context.Sistemas
            .Where(x => x.Id == request.Id && x.Ativo)
            .ProjectTo<SistemaDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (sistema is null)
        {
            return new GetSistemaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetSistemaViewModel { Sistema = sistema, OperationResult = OperationResult.Success };
    }
}