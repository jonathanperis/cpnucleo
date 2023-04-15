namespace Cpnucleo.Application.Queries.ListImpedimento;

public sealed class ListImpedimentoQueryHandler : IRequestHandler<ListImpedimentoQuery, ListImpedimentoViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListImpedimentoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListImpedimentoViewModel> Handle(ListImpedimentoQuery request, CancellationToken cancellationToken)
    {
        var impedimentos = await _context.Impedimentos
            .AsNoTracking()
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (impedimentos is null)
        {
            return new ListImpedimentoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListImpedimentoViewModel { Impedimentos = impedimentos, OperationResult = OperationResult.Success };
    }
}