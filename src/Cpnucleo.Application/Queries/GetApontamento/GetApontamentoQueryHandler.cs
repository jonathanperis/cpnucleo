namespace Cpnucleo.Application.Queries.GetApontamento;

public sealed class GetApontamentoQueryHandler : IRequestHandler<GetApontamentoQuery, GetApontamentoViewModel>
{
    private readonly IApplicationDbContext _context;

    public GetApontamentoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<GetApontamentoViewModel> Handle(GetApontamentoQuery request, CancellationToken cancellationToken)
    {
        var apontamento = await _context.Apontamentos
            .AsNoTracking()
            .Include(x => x.Tarefa)
            .Where(x => x.Id == request.Id && x.Ativo)
            .Select(x => x.MapToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (apontamento is null)
        {
            return new GetApontamentoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetApontamentoViewModel { Apontamento = apontamento, OperationResult = OperationResult.Success };
    }
}