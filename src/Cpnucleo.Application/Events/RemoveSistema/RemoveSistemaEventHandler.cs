namespace Cpnucleo.Application.Events.RemoveSistema;

public sealed class RemoveSistemaEventHandler : IMessageReceptionHandler<RemoveSistemaEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RemoveSistemaEventHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Handle(RemoveSistemaEvent @event, CancellationToken cancellationToken)
    {
        //Some business logic here.
        var sistemas = await _context.Sistemas
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<SistemaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
