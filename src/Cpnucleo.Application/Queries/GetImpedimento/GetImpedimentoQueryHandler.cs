using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Queries.GetImpedimento;

namespace Cpnucleo.Application.Queries.GetImpedimento;

public sealed class GetImpedimentoQueryHandler : IRequestHandler<GetImpedimentoQuery, GetImpedimentoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetImpedimentoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetImpedimentoViewModel> Handle(GetImpedimentoQuery request, CancellationToken cancellationToken)
    {
        var impedimento = await _context.Impedimentos
            .Where(x => x.Id == request.Id && x.Ativo)
            .ProjectTo<ImpedimentoDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (impedimento is null)
        {
            return new GetImpedimentoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetImpedimentoViewModel { Impedimento = impedimento, OperationResult = OperationResult.Success };
    }
}