namespace Cpnucleo.Application.Events.RemoveSistema;

public sealed class RemoveSistemaEventHandler : IMessageReceptionHandler<RemoveSistemaEvent>
{
    private readonly IApplicationDbContext _context;

    public RemoveSistemaEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(RemoveSistemaEvent @event, CancellationToken cancellationToken)
    {
        //Some business logic here.
        var sistemas = await _context.Sistemas
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);
    }
}
