namespace Cpnucleo.Application.Queries.GetImpedimento;

public sealed class GetImpedimentoQueryHandler : IRequestHandler<GetImpedimentoQuery, GetImpedimentoViewModel>
{
    private readonly IApplicationDbContext _context;

    public GetImpedimentoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<GetImpedimentoViewModel> Handle(GetImpedimentoQuery request, CancellationToken cancellationToken)
    {
        var impedimento = await _context.Impedimentos
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.Ativo)
            .Select(x => x.MapToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (impedimento is null)
        {
            return new GetImpedimentoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetImpedimentoViewModel { Impedimento = impedimento, OperationResult = OperationResult.Success };
    }
}