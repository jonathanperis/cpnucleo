namespace Cpnucleo.Application.Commands.RemoveImpedimento;

public sealed class RemoveImpedimentoCommandHandler : IRequestHandler<RemoveImpedimentoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveImpedimentoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(RemoveImpedimentoCommand request, CancellationToken cancellationToken)
    {
        var impedimento = await _context.Impedimentos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (impedimento is null)
        {
            return OperationResult.NotFound;
        }

        impedimento = Impedimento.Remove(impedimento);
        _context.Impedimentos.Update(impedimento); //JONATHAN - Soft Delete.

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
