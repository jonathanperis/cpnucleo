namespace Cpnucleo.Application.Commands.UpdateImpedimento;

public sealed class UpdateImpedimentoCommandHandler : IRequestHandler<UpdateImpedimentoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateImpedimentoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(UpdateImpedimentoCommand request, CancellationToken cancellationToken)
    {
        var impedimento = await _context.Impedimentos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (impedimento is null)
        {
            return OperationResult.NotFound;
        }

        impedimento = Domain.Entities.Impedimento.Update(impedimento, request.Nome);
        _context.Impedimentos.Update(impedimento);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
