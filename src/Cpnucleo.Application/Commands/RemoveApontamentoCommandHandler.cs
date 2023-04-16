namespace Cpnucleo.Application.Commands;

public sealed class RemoveApontamentoCommandHandler : IRequestHandler<RemoveApontamentoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveApontamentoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(RemoveApontamentoCommand request, CancellationToken cancellationToken)
    {
        var apontamento = await _context.Apontamentos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (apontamento is null)
        {
            return OperationResult.NotFound;
        }

        apontamento = Apontamento.Remove(apontamento);
        _context.Apontamentos.Update(apontamento); //JONATHAN - Soft Delete.

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
