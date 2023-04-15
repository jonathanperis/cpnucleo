namespace Cpnucleo.Application.Queries.ListSistema;

public sealed class ListSistemaQueryHandler : IRequestHandler<ListSistemaQuery, ListSistemaViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IHubContext<ApplicationHub> _hubContext;

    public ListSistemaQueryHandler(IApplicationDbContext context, IHubContext<ApplicationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async ValueTask<ListSistemaViewModel> Handle(ListSistemaQuery request, CancellationToken cancellationToken)
    {
        var sistemas = await _context.Sistemas
            .AsNoTracking()
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (sistemas is null)
        {
            return new ListSistemaViewModel { OperationResult = OperationResult.NotFound };
        }

        await _hubContext.Clients.All.SendAsync("broadcastMessage", "Broadcast: Test Message.", "Lorem ipsum dolor sit amet", cancellationToken);

        return new ListSistemaViewModel { Sistemas = sistemas, OperationResult = OperationResult.Success };
    }
}