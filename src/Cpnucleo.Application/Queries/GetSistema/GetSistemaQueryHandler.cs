namespace Cpnucleo.Application.Queries.GetSistema;

public sealed class GetSistemaQueryHandler : IRequestHandler<GetSistemaQuery, GetSistemaViewModel>
{
    private readonly IApplicationDbContext _context;

    public GetSistemaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<GetSistemaViewModel> Handle(GetSistemaQuery request, CancellationToken cancellationToken)
    {
        var sistema = await _context.Sistemas
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.Ativo)
            .Select(x => x.MapToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (sistema is null)
        {
            return new GetSistemaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetSistemaViewModel { Sistema = sistema, OperationResult = OperationResult.Success };
    }
}