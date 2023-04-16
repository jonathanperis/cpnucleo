namespace Cpnucleo.Application.Commands.RemoveSistema;

public sealed class RemoveSistemaCommandHandler : IRequestHandler<RemoveSistemaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IEventManager _eventHandler;

    public RemoveSistemaCommandHandler(IApplicationDbContext context, IEventManager eventHandler)
    {
        _context = context;
        _eventHandler = eventHandler;
    }

    public async ValueTask<OperationResult> Handle(RemoveSistemaCommand request, CancellationToken cancellationToken)
    {
        var sistema = await _context.Sistemas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (sistema is null)
        {
            return OperationResult.NotFound;
        }

        sistema = Sistema.Remove(sistema);
        _context.Sistemas.Update(sistema); //JONATHAN - Soft Delete.

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        if (result == OperationResult.Success)
        {
            await _eventHandler.PublishEventAsync(new RemoveSistemaEvent(request.Id));
        }

        return result;
    }
}
