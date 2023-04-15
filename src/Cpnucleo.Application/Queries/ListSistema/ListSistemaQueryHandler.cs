namespace Cpnucleo.Application.Queries.ListSistema;

public sealed class ListSistemaQueryHandler : IRequestHandler<ListSistemaQuery, ListSistemaViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<ApplicationHub> _hubContext;

    public ListSistemaQueryHandler(IApplicationDbContext context, IMapper mapper, IHubContext<ApplicationHub> hubContext)
    {
        _context = context;
        _mapper = mapper;
        _hubContext = hubContext;
    }

    public async ValueTask<ListSistemaViewModel> Handle(ListSistemaQuery request, CancellationToken cancellationToken)
    {
        var sistemas = await _context.Sistemas
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<SistemaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (sistemas is null)
        {
            return new ListSistemaViewModel { OperationResult = OperationResult.NotFound };
        }

        await _hubContext.Clients.All.SendAsync("broadcastMessage", "Broadcast: Test Message.", "Lorem ipsum dolor sit amet", cancellationToken);

        return new ListSistemaViewModel { Sistemas = sistemas, OperationResult = OperationResult.Success };
    }
}