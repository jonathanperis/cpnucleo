namespace Cpnucleo.Application.Queries;

public sealed class GetProjetoQueryHandler : IRequestHandler<GetProjetoQuery, GetProjetoViewModel>
{
    private readonly IApplicationDbContext _context;

    public GetProjetoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<GetProjetoViewModel> Handle(GetProjetoQuery request, CancellationToken cancellationToken)
    {
        var projeto = await _context.Projetos
            .AsNoTracking()
            .Include(x => x.Sistema)
            .Where(x => x.Id == request.Id && x.Ativo)
            .Select(x => x.MapToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (projeto is null)
        {
            return new GetProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetProjetoViewModel { Projeto = projeto, OperationResult = OperationResult.Success };
    }
}